using Discount.Grpc.Context;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

WebApplication app = builder.Build();

try
{
    IServiceScope scope = app.Services.CreateScope();
    DiscountContext context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
    context.Database.Migrate();
}
catch
{
    // ignored
}

// Configure the HTTP request pipeline.

app.MapGrpcService<DiscountService>();

app.MapGet("/", () =>
{
    _ = "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, " +
        "visit: https://go.microsoft.com/fwlink/?linkid=2086909";
});

app.Run();