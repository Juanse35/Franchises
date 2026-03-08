using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using franchise.Models;

public static class FranchiseRoutes
{
    public static void MapFranchiseRoutes(this WebApplication app)
    {

        app.MapGet("/getFranchises", () =>
        {
            var controller = new FranchiseControllers();
            var franchises = controller.GetFranchise();
            return Results.Ok(franchises);
        });

        app.MapGet("/getFranchise/{id:int}", (int id) =>
        {
            var controller = new FranchiseControllers();
            var franchise = controller.GetFranchiseById(id);
            return franchise != null ? Results.Ok(franchise) : Results.NotFound();
        });

        app.MapPost("/createFranchise", (Franchise franchise) =>
        {
            if (franchise == null || string.IsNullOrEmpty(franchise.Name))
                return Results.BadRequest("Invalid franchise data");

            var controller = new FranchiseControllers();
            var createdFranchise = controller.CreateFranchise(franchise); 

            return createdFranchise != null
                ? Results.Created($"/getFranchise/{createdFranchise.Id}", createdFranchise)
                : Results.Problem("Failed to create franchise");
        });

        app.MapPut("/updateFranchise/{id:int}", (int id, Franchise franchise) =>
        {
            if (franchise == null || string.IsNullOrEmpty(franchise.Name))
                return Results.BadRequest("Invalid franchise data");

            franchise.Id = id; 
            var controller = new FranchiseControllers();
            var updatedFranchise = controller.UpdateFranchise(franchise);

            return updatedFranchise != null
                ? Results.Ok(updatedFranchise)
                : Results.NotFound("Franchise not found");
        });

        app.MapDelete("/deleteFranchise/{id:int}", (int id) =>
        {
            var controller = new FranchiseControllers();
            bool deleted = controller.DeleteFranchise(id);

            return deleted
                ? Results.Ok($"Franchise with ID {id} deleted successfully")
                : Results.NotFound("Franchise not found");
        });
    }
}