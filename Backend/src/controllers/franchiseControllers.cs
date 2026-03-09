// This file contains the FranchiseControllers class which is responsible for handling all operations related to franchises,
// such as creating, retrieving, updating, and deleting franchise records in the database.

using Microsoft.Data.SqlClient;
using franchise.Models;

public class FranchiseControllers
{
    // Retrieves all franchises stored in the database.
    public List<Franchise> GetFranchise()
    {
        List<Franchise> franchises = new List<Franchise>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id, name, registration_date FROM tbl_franchise";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    franchises.Add(new Franchise
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["registration_date"])
                    });
                }

                Console.WriteLine("Franchise list retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving franchises from database: {ex.Message}");
        }

        return franchises;
    }

    // Retrieves a specific franchise by its unique identifier.
    public Franchise GetFranchiseById(int id)
    {
        Franchise franchise = null;

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id, name, registration_date FROM tbl_franchise WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    franchise = new Franchise
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["registration_date"])
                    };

                    Console.WriteLine($"Franchise with ID {id} retrieved successfully.");
                }
                else
                {
                    Console.WriteLine($"No franchise found with ID {id}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving franchise by ID: {ex.Message}");
        }

        return franchise;
    }

    // Creates a new franchise and saves it into the database.
    public Franchise CreateFranchise(Franchise franchise)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"INSERT INTO tbl_franchise (name)
                                OUTPUT INSERTED.id
                                VALUES (@name)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", franchise.Name);

                connection.Open();

                int insertedId = (int)command.ExecuteScalar();
                franchise.Id = insertedId;
                franchise.CreatedAt = DateTime.Now;

                Console.WriteLine($"Franchise created successfully with ID {insertedId}.");

                return franchise;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating franchise: {ex.Message}");
            return null;
        }
    }

    // Updates the information of an existing franchise in the database.
    public Franchise UpdateFranchise(Franchise franchise)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"UPDATE tbl_franchise
                                SET name = @name
                                WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", franchise.Name);
                command.Parameters.AddWithValue("@id", franchise.Id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    franchise.CreatedAt = GetFranchiseById(franchise.Id)?.CreatedAt ?? DateTime.Now;

                    Console.WriteLine($"Franchise with ID {franchise.Id} updated successfully.");

                    return franchise;
                }
                else
                {
                    Console.WriteLine($"Franchise with ID {franchise.Id} was not found for update.");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating franchise: {ex.Message}");
            return null;
        }
    }

    // Deletes a franchise record from the database by its identifier.
    public bool DeleteFranchise(int id)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "DELETE FROM tbl_franchise WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Franchise with ID {id} deleted successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Franchise with ID {id} was not found for deletion.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting franchise: {ex.Message}");
            return false;
        }
    }
}