using System.ComponentModel.DataAnnotations;

namespace Discount.API.DTOs;

public class CreateCouponDto
{
    [MaxLength(24)] public string ProductName { get; set; } = default!;
    [MaxLength(4000)] public string Description { get; set; } = default!;
    public double Amount { get; set; }
}