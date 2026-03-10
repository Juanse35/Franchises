// This is the main entry point of the application, it will run the application and listen for incoming requests
public class MainApp
{
    public static void Run(string[] args)
    {
        Console.WriteLine("\nStarting the application...");

        // verify database connection before starting the server
        bool dbConnected = DatabaseConnections.ConnectToDatabase();
        if (!dbConnected)
        {
            Console.WriteLine("Failed to connect to the database. Exiting the application.");
            return; // Exit if there's no connection
        }

        var builder = WebApplication.CreateBuilder(args);

        // Add services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || true)
        {
            // Enable Swagger in development and for testing purposes
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enable HTTPS redirection for secure communication
        app.UseHttpsRedirection();

        // Enable CORS
        app.UseCors("AllowFrontend");

        // Map the routes defined in the BranchRoutes class
        app.MapRoutes();

        Console.WriteLine("Server is running on https://localhost:5001 \n");

        app.Run();
    }
}