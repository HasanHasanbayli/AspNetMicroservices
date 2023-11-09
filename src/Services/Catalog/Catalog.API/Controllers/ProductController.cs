using Catalog.API.Context;
using Catalog.API.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.API.Controllers;

[ApiController]
[Route(template: "api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ICatalogContext _catalogContext;

    public ProductController(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        IEnumerable<Product> products = await _catalogContext.Products
            .Find(filter: product => true)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet(template: "{id}")]
    public async Task<ActionResult> GetProductById(string id)
    {
        Product? product = await _catalogContext.Products
            .Find(filter: product => product.Id == id)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return NotFound("Product not found");
        }

        return Ok(product);
    }

    [HttpGet(template: "name/{productName}")]
    public async Task<ActionResult> GetProductByName(string productName)
    {
        Product? product = await _catalogContext.Products
            .Find(filter: product => product.Name.ToLower().Contains(productName.ToLower()))
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return NotFound("Product not found");
        }

        return Ok(product);
    }

    [HttpGet(template: "category/{categoryName}")]
    public async Task<ActionResult> GetProductByCategory(string categoryName)
    {
        IEnumerable<Product> products = await _catalogContext.Products
            .Find(filter: product => product.Category.ToLower().Contains(categoryName.ToLower()))
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] Product product)
    {
        product.Id = ObjectId.GenerateNewId().ToString()!;
        
        await _catalogContext.Products.InsertOneAsync(document: product);

        return Created("", product);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProduct([FromBody] Product product)
    {
        ReplaceOneResult result = await _catalogContext.Products
            .ReplaceOneAsync(filter: existProduct => existProduct.Id == product.Id,
                replacement: product);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return Ok(product);
        }

        return NotFound("Product not found");
    }

    [HttpDelete(template: "{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        DeleteResult result = await _catalogContext.Products
            .DeleteOneAsync(filter: product => product.Id == id);

        if (result.IsAcknowledged && result.DeletedCount > 0)
        {
            return Ok();
        }

        return NotFound("Product not found");
    }
}