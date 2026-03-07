using System;
using Microsoft.Data.SqlClient; // library for SQL Server database connection

//method that contains the logic to connect to the database and verify the connection.
public class ConnectionServer
{

    // credentials and connection string for the database connection
    private static string connectionString =
        "Server=database-sql.database.windows.net;" +
        "Database=db_Franchise;" +
        "User Id=sqlAdmin;" +
        "Password=Admin13524;" +
        "Encrypt=True;" +
        "Connection Timeout=30;";

    // This method verifies that the connection to the database was successful and returns a boolean value.
    public static bool Connect()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error connecting to the database:");
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}