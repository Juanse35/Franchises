using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public static class AppRoutes
{
    public static void MapRoutes(this WebApplication app)
    {
        app.MapGet("/", () =>
        {
            return Results.Ok("Welcome to the backend server! The database connection is successful.");
        });

        app.MapGet("/check-data", () =>
        {
            var franquicias = new[]
            {
                new { Id = 1, Nombre = "Franquicia A", Ciudad = "Medellín" },
                new { Id = 2, Nombre = "Franquicia B", Ciudad = "Bogotá" },
                new { Id = 3, Nombre = "Franquicia C", Ciudad = "Cali" }
            };

            return Results.Ok(franquicias);
        });
    }
}