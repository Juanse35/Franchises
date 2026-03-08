using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using franchise.Models;

public static class AppRoutes
{
    public static void MapRoutes(this WebApplication app)
    {
        app.MapGet("/", () =>
        {
            return Results.Ok("Welcome to the backend server! The database connection is successful.");
        });

        app.MapFranchiseRoutes();
    }
}