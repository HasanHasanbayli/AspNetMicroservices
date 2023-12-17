using System.Text.Json;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Discount.Grpc.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Controllers;

[ApiController]
[Route(template: "api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IDistributedCache distributedCache, DiscountGrpcService discountGrpcService)
    {
        _distributedCache = distributedCache;
        _discountGrpcService = discountGrpcService;
    }

    [HttpGet(template: "{userName}")]
    public async Task<IActionResult> Get(string userName, CancellationToken cancellationToken)
    {
        string? jsonData = await _distributedCache.GetStringAsync(key: userName, token: cancellationToken);

        return jsonData is null
            ? Ok(new ShoppingCart(userName))
            : Ok(JsonSerializer.Deserialize<ShoppingCart>(jsonData)!);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ShoppingCart basket, CancellationToken cancellationToken)
    {
        foreach (ShoppingCartItem shoppingCartItem in basket.Items)
        {
            GetDiscountResponse coupon = await _discountGrpcService.GetDiscount(shoppingCartItem.ProductName);
            shoppingCartItem.Price -= (decimal) coupon.Amount;
        }

        await _distributedCache.SetStringAsync(
            key: basket.Username,
            value: JsonSerializer.Serialize(basket),
            token: cancellationToken);

        return Ok();
    }

    [HttpDelete(template: "{userName}")]
    public async Task<IActionResult> Delete(string userName, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key: userName, token: cancellationToken);

        return Ok();
    }
}