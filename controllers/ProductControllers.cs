using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Reccora.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var products = new List<Product>
            {
                new Product(1, "Laptop", 999.99m),
                new Product(2, "Phone", 599.99m)
            };

            app.MapGet("/api/products", () => products);

            app.MapGet("/api/products/{id}", (int id) =>
            {
                var product = products.FirstOrDefault(p => p.Id == id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });

            app.MapPost("/api/products", (Product product) =>
            {
                var newId = products.Any() ? products.Max(p => p.Id) + 1 : 1;
                var newProduct = product with { Id = newId };
                products.Add(newProduct);
                return Results.Created($"/api/products/{newProduct.Id}", newProduct);
            });

            app.MapPut("/api/products/{id}", (int id, Product updatedProduct) =>
            {
                var index = products.FindIndex(p => p.Id == id);
                if (index == -1) return Results.NotFound();

                products[index] = updatedProduct with { Id = id };
                return Results.NoContent();
            });

            app.MapDelete("/api/products/{id}", (int id) =>
            {
                var product = products.FirstOrDefault(p => p.Id == id);
                if (product is null) return Results.NotFound();

                products.Remove(product);
                return Results.NoContent();
            });
        }
    }

    public record Product(int Id, string Name, decimal Price);
}
