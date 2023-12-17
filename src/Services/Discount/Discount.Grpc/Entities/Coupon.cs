using System.ComponentModel.DataAnnotations;

namespace Discount.Grpc.Entities;

public class Coupon
{
    public int Id { get; set; }
    [MaxLength(24)] public string ProductName { get; set; } = default!;
    [MaxLength(4000)] public string Description { get; set; } = default!;
    public double Amount { get; set; }
}