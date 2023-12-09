using System.Text.Json;
using Basket.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Controllers;

[ApiController]
[Route(template: "api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;

    public BasketController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
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
        await _distributedCache.SetStringAsync(key: basket.Username,
            value: JsonSerializer.Serialize(basket), token: cancellationToken);

        return Ok();
    }

    [HttpDelete(template: "{userName}")]
    public async Task<IActionResult> Delete(string userName, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key: userName, token: cancellationToken);

        return Ok();
    }
}