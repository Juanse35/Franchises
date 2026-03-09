using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using branch.Models;
public static class BranchRoutes
{
    public static void MapBranchRoutes(this WebApplication app)
    {
        app.MapGet("/getBranches", () =>
        {
            var controller = new BranchControllers();
            var branches = controller.GetBranch();
            return Results.Ok(branches);
        });

         app.MapGet("/getBranchById/{id:int}", (int id) =>
        {
            var controller = new BranchControllers();
            var branch = controller.GetBranchById(id);
            if (branch != null)
            {
                return Results.Ok(branch);
            }
            else
            {
                return Results.NotFound("Branch not found.");
            }
        });

        app.MapGet("/getBranchByIdFranchise/{franchiseId:int}", (int franchiseId) =>
        {
            var controller = new BranchControllers();
            var branches = controller.GetBranchByIdFranchise(franchiseId);
            return Results.Ok(branches);
        });


        app.MapPost("/createBranch", (Branch branch) =>
        {
            var controller = new BranchControllers();
            var createdBranch = controller.CreateBranch(branch);
            if (createdBranch != null)
            {
                return Results.Ok(createdBranch);
            }
            else
            {
                return Results.BadRequest("error creating branch. Please check if the franchise ID is valid.");
            }
        });

       
        app.MapPut("/updateBranch/{id:int}", (int id, Branch branch) =>
        {
            if (branch == null || string.IsNullOrEmpty(branch.Name_branch))
                return Results.BadRequest("Invalid branch data");

            branch.Id_branch = id;
            var controller = new BranchControllers();
            var updatedBranch = controller.UpdateBranch(branch);
            if (updatedBranch != null)
            {
                return Results.Ok(updatedBranch);
            }
            else
            {
                return Results.NotFound("Branch not found or update failed.");
            }
        });

        app.MapDelete("/deleteBranch/{id:int}", (int id) =>
        {
            var controller = new BranchControllers();
            var deleted = controller.DeleteBranch(id);
            if (deleted)
            {
                return Results.Ok($"Branch with ID {id} deleted successfully.");
            }
            else
            {
                return Results.NotFound("Branch not found.");
            }
        });
    }
}