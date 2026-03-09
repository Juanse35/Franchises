using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using product.Models;

public static class ProductRoutes
{
    public static void MapProductRoutes(this WebApplication app)
    {
        app.MapGet("/getProducts", () =>
        {
            var controller = new ProductControllers();
            var products = controller.GetProduct();
            return Results.Ok(products);
        });

        app.MapPost("/createProduct", (Product product) =>
        {
            if (product == null || string.IsNullOrEmpty(product.Name_product))
                return Results.BadRequest("Invalid product data");

            var controller = new ProductControllers();
            var createdProduct = controller.CreateProduct(product);
            if (createdProduct != null)
            {
                return Results.Created($"/getProduct/{createdProduct.Id_product}", createdProduct);
            }
            else
            {
                return Results.BadRequest("Error creating product. Please check if the branch ID is valid.");
            }
        });

        app.MapGet("/getProduct/{id:int}", (int id) =>
        {
            var controller = new ProductControllers();
            var product = controller.GetProductById(id);
            if (product != null)
            {
                return Results.Ok(product);
            }
            else
            {
                return Results.NotFound("Product not found.");
            }
        });

        app.MapGet("/getProductByIdBranch/{branchId:int}", (int branchId) =>
        {
            var controller = new ProductControllers();
            var products = controller.GetProductByIdBranch(branchId);
            return Results.Ok(products);
        });

        app.MapPut("/updateProduct/{id:int}", (int id, Product product) =>
        {
            if (product == null || string.IsNullOrEmpty(product.Name_product))
                return Results.BadRequest("Invalid product data");

            product.Id_product = id;
            var controller = new ProductControllers();
            var updatedProduct = controller.UpdateProduct(product);
            if (updatedProduct != null)
            {
                return Results.Ok(updatedProduct);
            }
            else
            {
                return Results.NotFound("Product not found or update failed.");
            }
        });

        app.MapDelete("/deleteProduct/{id:int}", (int id) =>
        {
            var controller = new ProductControllers();
            var deleted = controller.DeleteProduct(id);
            if (deleted)
            {
                return Results.Ok($"Product with ID {id} deleted successfully.");
            }
            else
            {
                return Results.NotFound("Product not found.");
            }
        });
    }
}
