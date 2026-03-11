// This file defines the routes for managing franchises in the application.
// It includes endpoints for CRUD operations on franchises, allowing clients
// to create, read, update, and delete franchise records through HTTP requests.
using franchise.Models;

public static class FranchiseRoutes
{
    public static void MapFranchiseRoutes(this WebApplication app)
    {

        // Retrieves all franchises stored in the system.
        app.MapGet("/getFranchises", () =>
        {
            var controller = new FranchiseControllers();
            var franchises = controller.GetFranchise();

            return Results.Ok(new
            {
                message = "Franchise list retrieved successfully.",
                data = franchises
            });
        });

        // Retrieves a specific franchise by its unique identifier.
        app.MapGet("/getFranchise/{id:int}", (int id) =>
        {
            var controller = new FranchiseControllers();
            var franchise = controller.GetFranchiseById(id);

            if (franchise != null)
            {
                return Results.Ok(new
                {
                    message = $"Franchise with ID {id} retrieved successfully.",
                    data = franchise
                });
            }

            return Results.NotFound(new
            {
                message = $"Franchise with ID {id} was not found."
            });
        });

        // Creates a new franchise and stores it in the database.
        app.MapPost("/createFranchise", (Franchise franchise) =>
        {
            if (franchise == null || string.IsNullOrEmpty(franchise.Name))
            {
                return Results.BadRequest(new
                {
                    message = "Invalid franchise data. Name is required."
                });
            }

            var controller = new FranchiseControllers();
            var createdFranchise = controller.CreateFranchise(franchise);

            if (createdFranchise != null)
            {
                return Results.Created($"/getFranchise/{createdFranchise.Id}", new
                {
                    message = "Franchise created successfully.",
                    data = createdFranchise
                });
            }

            return Results.Problem("An error occurred while creating the franchise.");
        });

        // Updates an existing franchise using its identifier.
        app.MapPut("/updateFranchise/{id:int}", (int id, Franchise franchise) =>
        {
            if (franchise == null || string.IsNullOrEmpty(franchise.Name))
            {
                return Results.BadRequest(new
                {
                    message = "Invalid franchise data. Name is required."
                });
            }

            franchise.Id = id;

            var controller = new FranchiseControllers();
            var updatedFranchise = controller.UpdateFranchise(franchise);

            if (updatedFranchise != null)
            {
                return Results.Ok(new
                {
                    message = $"Franchise with ID {id} updated successfully.",
                    data = updatedFranchise
                });
            }

            return Results.NotFound(new
            {
                message = $"Franchise with ID {id} was not found."
            });
        });

        // Deletes a franchise permanently from the system.
        app.MapDelete("/deleteFranchise/{id:int}", (int id) =>
        {
            var controller = new FranchiseControllers();
            bool deleted = controller.DeleteFranchise(id);

            if (deleted)
            {
                return Results.Ok(new
                {
                    message = $"Franchise with ID {id} deleted successfully."
                });
            }

            return Results.NotFound(new
            {
                message = $"Franchise with ID {id} was not found."
            });
        });
    }
}