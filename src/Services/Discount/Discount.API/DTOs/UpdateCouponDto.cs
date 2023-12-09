using System.ComponentModel.DataAnnotations;

namespace Discount.API.DTOs;

public class UpdateCouponDto
{
    public Guid Id { get; set; }
    [MaxLength(24)] public string ProductName { get; set; } = default!;
    [MaxLength(4000)] public string Description { get; set; } = default!;
    public decimal Amount { get; set; }
}