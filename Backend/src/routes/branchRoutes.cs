// This file defines the routes for the branch endpoints in the application, 
//it maps the HTTP requests to the corresponding controller methods
using branch.Models;

public static class BranchRoutes
{
    public static void MapBranchRoutes(this WebApplication app)
    {
        // Retrieves all branches stored in the system.
        app.MapGet("/getBranches", () =>
        {
            var controller = new BranchControllers();
            var branches = controller.GetBranch();

            return Results.Ok(new
            {
                message = "Branch list retrieved successfully.",
                data = branches
            });
        });

        // Retrieves a specific branch by its unique identifier.
        app.MapGet("/getBranchById/{id:int}", (int id) =>
        {
            var controller = new BranchControllers();
            var branch = controller.GetBranchById(id);

            if (branch != null)
            {
                return Results.Ok(new
                {
                    message = $"Branch with ID {id} retrieved successfully.",
                    data = branch
                });
            }

            return Results.NotFound(new
            {
                message = $"Branch with ID {id} not found."
            });
        });

        // Retrieves all branches associated with a specific franchise.
        app.MapGet("/getBranchByIdFranchise/{franchiseId:int}", (int franchiseId) =>
        {
            var controller = new BranchControllers();
            var branches = controller.GetBranchByIdFranchise(franchiseId);

            return Results.Ok(new
            {
                message = $"Branches for franchise ID {franchiseId} retrieved successfully.",
                data = branches
            });
        });

        // Creates a new branch and associates it with a franchise.
        app.MapPost("/createBranch", (Branch branch) =>
        {
            var controller = new BranchControllers();
            var createdBranch = controller.CreateBranch(branch);

            if (createdBranch != null)
            {
                return Results.Created($"/getBranchById/{createdBranch.Id_branch}", new
                {
                    message = "Branch created successfully.",
                    data = createdBranch
                });
            }

            return Results.BadRequest(new
            {
                message = "Error creating branch. Please ensure the franchise ID is valid."
            });
        });

        // Updates an existing branch by its unique identifier.
        app.MapPut("/updateBranch/{id:int}", (int id, Branch branch) =>
        {
            if (branch == null || string.IsNullOrEmpty(branch.Name_branch))
            {
                return Results.BadRequest(new
                {
                    message = "Invalid branch data. Name is required."
                });
            }

            branch.Id_branch = id;
            var controller = new BranchControllers();
            var updatedBranch = controller.UpdateBranch(branch);

            if (updatedBranch != null)
            {
                return Results.Ok(new
                {
                    message = $"Branch with ID {id} updated successfully.",
                    data = updatedBranch
                });
            }

            return Results.NotFound(new
            {
                message = $"Branch with ID {id} not found or update failed."
            });
        });

        // Deletes a branch permanently from the system.
        app.MapDelete("/deleteBranch/{id:int}", (int id) =>
        {
            var controller = new BranchControllers();
            var deleted = controller.DeleteBranch(id);

            if (deleted)
            {
                return Results.Ok(new
                {
                    message = $"Branch with ID {id} deleted successfully."
                });
            }

            return Results.BadRequest(new
            {
                message = "Cannot delete branch because it has associated products or does not exist."
            });
        });
    }
}