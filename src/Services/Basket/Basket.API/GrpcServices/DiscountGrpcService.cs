using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService;
    }

    public async Task<GetDiscountResponse> GetDiscount(string productName)
    {
        GetDiscountRequest discountRequest = new()
        {
            ProductName = productName
        };
        
        return await _discountProtoService.GetDiscountAsync(discountRequest);
    }
}