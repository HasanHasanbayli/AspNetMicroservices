using Discount.Grpc.Context;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountContext _discountContext;

    public DiscountService(DiscountContext discountContext)
    {
        _discountContext = discountContext;
    }

    public override async Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        Coupon? dbCoupon = await _discountContext.Coupons
            .Include(navigationPropertyPath: coupon => coupon.Id)
            .FirstOrDefaultAsync(predicate: coupon => coupon.ProductName == request.ProductName);

        if (dbCoupon is null)
        {
            return new GetDiscountResponse
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };
        }

        return new GetDiscountResponse
        {
            Id = dbCoupon.Id,
            Description = dbCoupon.Description,
            Amount = dbCoupon.Amount
        };
    }
}