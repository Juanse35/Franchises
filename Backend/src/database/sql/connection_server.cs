// this class is responsible for managing the connection to the SQL database
using Microsoft.Data.SqlClient;

public class ConnectionServer
{
    // information to connect to the database, this should be stored in a secure way in a real application
    private static string connectionString =
        "Server=database-sql.database.windows.net;" +
        "Database=db_Franchise;" +
        "User Id=sqlAdmin;" +
        "Password=Admin13524;" +
        "Encrypt=True;" +
        "Connection Timeout=30;";

    // verify connection
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

    // the method to get the connection
    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}