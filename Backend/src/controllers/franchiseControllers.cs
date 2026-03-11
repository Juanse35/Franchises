using Microsoft.Data.SqlClient;
using franchise.Models;

public class FranchiseControllers
{
    // Retrieves all franchises from the database
    public List<Franchise> GetFranchise()
    {
        List<Franchise> franchises = new List<Franchise>();

        try
        {
            // Open database connection
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                // SQL query to retrieve franchise information
                string query = "SELECT id, name, registration_date FROM tbl_franchise";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                // Execute query and read results
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Map database values to Franchise model
                    franchises.Add(new Franchise
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["registration_date"])
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // Log error if retrieval fails
            Console.WriteLine($"Error retrieving franchises: {ex.Message}");
        }

        return franchises;
    }

    // Retrieves a single franchise by its ID
    public Franchise GetFranchiseById(int id)
    {
        Franchise franchise = null;

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                // SQL query with parameter to prevent SQL injection
                string query = "SELECT id, name, registration_date FROM tbl_franchise WHERE id=@id";

                SqlCommand command = new SqlCommand(query, connection);

                // Bind ID parameter
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Map result if franchise exists
                if (reader.Read())
                {
                    franchise = new Franchise
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["registration_date"])
                    };
                }
            }
        }
        catch (Exception ex)
        {
            // Log error if retrieval fails
            Console.WriteLine($"Error retrieving franchise: {ex.Message}");
        }

        return franchise;
    }

    // Creates a new franchise in the database
    public Franchise CreateFranchise(Franchise franchise)
    {
        // Validate franchise name
        if (string.IsNullOrWhiteSpace(franchise.Name))
        {
            Console.WriteLine("Franchise name is required");
            return null;
        }

        // Normalize name (trim spaces and convert to lowercase)
        franchise.Name = franchise.Name.Trim().ToLower();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                // Insert franchise and return generated ID
                string query = @"INSERT INTO tbl_franchise(name)
                                OUTPUT INSERTED.id
                                VALUES(@name)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@name", franchise.Name);

                connection.Open();

                // Execute insert and retrieve generated ID
                int insertedId = (int)command.ExecuteScalar();

                franchise.Id = insertedId;
                franchise.CreatedAt = DateTime.Now;

                return franchise;
            }
        }
        catch (Exception ex)
        {
            // Log error if insert fails
            Console.WriteLine($"Error creating franchise: {ex.Message}");
            return null;
        }
    }

    // Updates an existing franchise
    public Franchise UpdateFranchise(Franchise franchise)
    {
        // Validate franchise name
        if (string.IsNullOrWhiteSpace(franchise.Name))
        {
            Console.WriteLine("Franchise name is required");
            return null;
        }

        // Normalize franchise name
        franchise.Name = franchise.Name.Trim().ToLower();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                // SQL update query
                string query = @"UPDATE tbl_franchise
                                 SET name=@name
                                 WHERE id=@id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@name", franchise.Name);
                command.Parameters.AddWithValue("@id", franchise.Id);

                connection.Open();

                // Execute update
                int rows = command.ExecuteNonQuery();

                // Return franchise if update succeeded
                if (rows > 0)
                    return franchise;

                return null;
            }
        }
        catch (Exception ex)
        {
            // Log error if update fails
            Console.WriteLine($"Error updating franchise: {ex.Message}");
            return null;
        }
    }

    // Deletes a franchise and all related branches and products
    public bool DeleteFranchise(int id)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                connection.Open();

                // Begin transaction to ensure atomic cascade delete
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Step 1: Retrieve all branches belonging to the franchise
                        List<int> branchIds = new List<int>();

                        string getBranchesQuery = "SELECT id_branch FROM tbl_branch WHERE franchise_id = @id";

                        SqlCommand getBranchesCmd = new SqlCommand(getBranchesQuery, connection, transaction);

                        getBranchesCmd.Parameters.AddWithValue("@id", id);

                        SqlDataReader reader = getBranchesCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            branchIds.Add(Convert.ToInt32(reader["id_branch"]));
                        }

                        reader.Close();


                        // Step 2: Delete all products associated with those branches
                        foreach (int branchId in branchIds)
                        {
                            string deleteProductsQuery = "DELETE FROM tbl_product WHERE branch_id = @branchId";

                            SqlCommand deleteProductsCmd = new SqlCommand(deleteProductsQuery, connection, transaction);

                            deleteProductsCmd.Parameters.AddWithValue("@branchId", branchId);

                            deleteProductsCmd.ExecuteNonQuery();
                        }


                        // Step 3: Delete branches linked to the franchise
                        string deleteBranchesQuery = "DELETE FROM tbl_branch WHERE franchise_id = @id";

                        SqlCommand deleteBranchesCmd = new SqlCommand(deleteBranchesQuery, connection, transaction);

                        deleteBranchesCmd.Parameters.AddWithValue("@id", id);

                        deleteBranchesCmd.ExecuteNonQuery();


                        // Step 4: Delete the franchise itself
                        string deleteFranchiseQuery = "DELETE FROM tbl_franchise WHERE id = @id";

                        SqlCommand deleteFranchiseCmd = new SqlCommand(deleteFranchiseQuery, connection, transaction);

                        deleteFranchiseCmd.Parameters.AddWithValue("@id", id);

                        int rows = deleteFranchiseCmd.ExecuteNonQuery();


                        // Commit transaction if franchise was deleted successfully
                        if (rows > 0)
                        {
                            transaction.Commit();
                            Console.WriteLine($"Franchise {id} and all related data deleted successfully.");
                            return true;
                        }
                        else
                        {
                            // Rollback if franchise was not found
                            transaction.Rollback();
                            Console.WriteLine("Franchise not found.");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if any step fails
                        transaction.Rollback();
                        Console.WriteLine($"Cascade delete error: {ex.Message}");
                        return false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log connection or transaction errors
            Console.WriteLine($"Delete error: {ex.Message}");
            return false;
        }
    }
}