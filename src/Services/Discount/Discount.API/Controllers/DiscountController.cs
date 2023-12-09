using Discount.API.Context;
using Discount.API.DTOs;
using Discount.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly DiscountContext _discountContext;

    public DiscountController(DiscountContext discountContext)
    {
        _discountContext = discountContext;
    }

    [HttpGet(template: "{productName}")]
    public async Task<ActionResult> GetDiscount([FromRoute] string productName, CancellationToken cancellationToken)
    {
        Coupon? dbCoupon = await _discountContext.Coupons.FirstOrDefaultAsync(
            predicate: coupon => coupon.ProductName == productName,
            cancellationToken: cancellationToken);

        if (dbCoupon is null)
        {
            return Ok(new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            });
        }

        return Ok(dbCoupon);
    }

    [HttpPost]
    public async Task<ActionResult> CreateDiscount([FromBody] CreateCouponDto createCoupon,
        CancellationToken cancellationToken)
    {
        Coupon newCoupon = new()
        {
            ProductName = createCoupon.ProductName,
            Description = createCoupon.Description,
            Amount = createCoupon.Amount
        };

        await _discountContext.Coupons.AddAsync(newCoupon, cancellationToken);
        await _discountContext.SaveChangesAsync(cancellationToken);

        return Created(uri: "", newCoupon);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateDiscount([FromBody] UpdateCouponDto updateCoupon,
        CancellationToken cancellationToken)
    {
        Coupon? dbCoupon = await _discountContext.Coupons.FirstOrDefaultAsync(
            predicate: coupon => coupon.ProductName == updateCoupon.ProductName,
            cancellationToken: cancellationToken);

        if (dbCoupon is null)
            return NotFound("Discount with the specified ProductName does not exist.");

        dbCoupon.ProductName = updateCoupon.ProductName;
        dbCoupon.Description = updateCoupon.Description;
        dbCoupon.Amount = updateCoupon.Amount;

        _discountContext.Coupons.Update(dbCoupon);
        await _discountContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpDelete(template: "{productName}")]
    public async Task<ActionResult> DeleteDiscount(string productName, CancellationToken cancellationToken)
    {
        Coupon? dbCoupon = await _discountContext.Coupons.FirstOrDefaultAsync(
            predicate: coupon => coupon.ProductName == productName,
            cancellationToken: cancellationToken);

        if (dbCoupon is null)
            return NotFound("Discount with the specified ProductName does not exist.");

        _discountContext.Coupons.Remove(dbCoupon);
        await _discountContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}