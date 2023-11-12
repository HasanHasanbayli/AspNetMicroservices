namespace Basket.API.Entities;

public class ShoppingCart
{
    public ShoppingCart()
    {
    }

    public ShoppingCart(string username)
    {
        Username = username;
    }

    public string Username { get; set; } = default!;

    public List<ShoppingCartItem> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}