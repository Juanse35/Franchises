//this is class that contains the logic to connect to the database and verify the connection.
public class DatabaseConnections
{
    public static bool ConnectToDatabase()
    {
        // This method verifies that the connection to the database was successful.
        Console.WriteLine("Connecting to the database...");
        bool isConnected = ConnectionServer.Connect();
        if (isConnected)
        {
            Console.WriteLine("Database connection established successfully.");
            return isConnected;
        }
        else
        {
            Console.WriteLine("Failed to establish database connection.");
            return isConnected;
        }
        
    }
}