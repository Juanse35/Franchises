using Microsoft.Data.SqlClient;
using franchise.Models;

public class FranchiseControllers
{
    // Get all franchises
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving franchises: {ex.Message}");
        }

        return franchises;
    }

    // Get franchise by ID
    public Franchise GetFranchiseById(int id)
    {
        Franchise franchise = null;

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id, name, registration_date FROM tbl_franchise WHERE id=@id";

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
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving franchise: {ex.Message}");
        }

        return franchise;
    }

    // Create franchise
    public Franchise CreateFranchise(Franchise franchise)
    {
        if (string.IsNullOrWhiteSpace(franchise.Name))
        {
            Console.WriteLine("Franchise name is required");
            return null;
        }

        franchise.Name = franchise.Name.Trim().ToLower();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"INSERT INTO tbl_franchise(name)
                                OUTPUT INSERTED.id
                                VALUES(@name)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", franchise.Name);

                connection.Open();

                int insertedId = (int)command.ExecuteScalar();
                franchise.Id = insertedId;
                franchise.CreatedAt = DateTime.Now;

                return franchise;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating franchise: {ex.Message}");
            return null;
        }
    }

    // Update franchise
    public Franchise UpdateFranchise(Franchise franchise)
    {
        if (string.IsNullOrWhiteSpace(franchise.Name))
        {
            Console.WriteLine("Franchise name is required");
            return null;
        }

        franchise.Name = franchise.Name.Trim().ToLower();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"UPDATE tbl_franchise
                                 SET name=@name
                                 WHERE id=@id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@name", franchise.Name);
                command.Parameters.AddWithValue("@id", franchise.Id);

                connection.Open();
                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                    return franchise;

                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating franchise: {ex.Message}");
            return null;
        }
    }

    // Delete franchise with full cascade
    public bool DeleteFranchise(int id)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Get all branches associated with the franchise
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


                        //Delete products associated with those branches
                        foreach (int branchId in branchIds)
                        {
                            string deleteProductsQuery = "DELETE FROM tbl_product WHERE branch_id = @branchId";

                            SqlCommand deleteProductsCmd = new SqlCommand(deleteProductsQuery, connection, transaction);
                            deleteProductsCmd.Parameters.AddWithValue("@branchId", branchId);

                            deleteProductsCmd.ExecuteNonQuery();
                        }


                        //Delete branches
                        string deleteBranchesQuery = "DELETE FROM tbl_branch WHERE franchise_id = @id";

                        SqlCommand deleteBranchesCmd = new SqlCommand(deleteBranchesQuery, connection, transaction);
                        deleteBranchesCmd.Parameters.AddWithValue("@id", id);

                        deleteBranchesCmd.ExecuteNonQuery();


                        //Delete franchise
                        string deleteFranchiseQuery = "DELETE FROM tbl_franchise WHERE id = @id";

                        SqlCommand deleteFranchiseCmd = new SqlCommand(deleteFranchiseQuery, connection, transaction);
                        deleteFranchiseCmd.Parameters.AddWithValue("@id", id);

                        int rows = deleteFranchiseCmd.ExecuteNonQuery();


                        if (rows > 0)
                        {
                            transaction.Commit();
                            Console.WriteLine($"Franchise {id} and all related data deleted successfully.");
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            Console.WriteLine("Franchise not found.");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Cascade delete error: {ex.Message}");
                        return false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Delete error: {ex.Message}");
            return false;
        }
    }
}