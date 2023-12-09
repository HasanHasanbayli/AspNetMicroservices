using Discount.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Context;

public class DiscountContext : DbContext
{
    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; } = default!;
}