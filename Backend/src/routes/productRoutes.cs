// this file defines the routes for the product endpoints in the application,
using product.Models;

public static class ProductRoutes
{
    public static void MapProductRoutes(this WebApplication app)
    {
        // Retrieves all products stored in the system.
        app.MapGet("/getProducts", () =>
        {
            var controller = new ProductControllers();
            var products = controller.GetProduct();

            return Results.Ok(new
            {
                message = "Product list retrieved successfully.",
                data = products
            });
        });

        // Retrieves a specific product by its unique identifier.
        app.MapGet("/getProduct/{id:int}", (int id) =>
        {
            var controller = new ProductControllers();
            var product = controller.GetProductById(id);

            if (product != null)
            {
                return Results.Ok(new
                {
                    message = $"Product with ID {id} retrieved successfully.",
                    data = product
                });
            }

            return Results.NotFound(new
            {
                message = $"Product with ID {id} not found."
            });
        });

        // Retrieves all products associated with a specific branch.
        app.MapGet("/getProductByIdBranch/{branchId:int}", (int branchId) =>
        {
            var controller = new ProductControllers();
            var products = controller.GetProductByIdBranch(branchId);

            return Results.Ok(new
            {
                message = $"Products for branch ID {branchId} retrieved successfully.",
                data = products
            });
        });

        // Creates a new product and associates it with a branch.
        app.MapPost("/createProduct", (Product product) =>
        {
            if (product == null || string.IsNullOrEmpty(product.Name_product))
            {
                return Results.BadRequest(new
                {
                    message = "Invalid product data. Name is required."
                });
            }

            var controller = new ProductControllers();
            var createdProduct = controller.CreateProduct(product);

            if (createdProduct != null)
            {
                return Results.Created($"/getProduct/{createdProduct.Id_product}", new
                {
                    message = "Product created successfully.",
                    data = createdProduct
                });
            }

            return Results.BadRequest(new
            {
                message = "Error creating product. Please ensure the branch ID is valid."
            });
        });

        // Updates an existing product by its unique identifier.
        app.MapPut("/updateProduct/{id:int}", (int id, Product product) =>
        {
            if (product == null || string.IsNullOrEmpty(product.Name_product))
            {
                return Results.BadRequest(new
                {
                    message = "Invalid product data. Name is required."
                });
            }

            product.Id_product = id;
            var controller = new ProductControllers();
            var updatedProduct = controller.UpdateProduct(product);

            if (updatedProduct != null)
            {
                return Results.Ok(new
                {
                    message = $"Product with ID {id} updated successfully.",
                    data = updatedProduct
                });
            }

            return Results.NotFound(new
            {
                message = $"Product with ID {id} not found or update failed."
            });
        });

        // Deletes a product from the system by its unique identifier.
        app.MapDelete("/deleteProduct/{id:int}", (int id) =>
        {
            var controller = new ProductControllers();
            var deleted = controller.DeleteProduct(id);

            if (deleted)
            {
                return Results.Ok(new
                {
                    message = $"Product with ID {id} deleted successfully."
                });
            }

            return Results.NotFound(new
            {
                message = $"Product with ID {id} not found."
            });
        });
    }
}