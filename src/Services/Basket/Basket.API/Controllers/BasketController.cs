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
    public async Task<IActionResult> Get(string userName)
    {
        string? jsonData = await _distributedCache.GetStringAsync(key: userName);

        if (string.IsNullOrEmpty(jsonData))
        {
            return Ok(new ShoppingCart(userName));
        }

        ShoppingCart items = JsonSerializer.Deserialize<ShoppingCart>(jsonData)!;

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ShoppingCart basket)
    {
        await _distributedCache.SetStringAsync(key: basket.Username, JsonSerializer.Serialize(basket));

        return Ok();
    }

    [HttpDelete(template: "{userName}")]
    public async Task<IActionResult> Delete(string userName)
    {
        await _distributedCache.RemoveAsync(key: userName);

        return Ok();
    }
}