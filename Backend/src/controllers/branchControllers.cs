// This file contains the BranchControllers class which is responsible for handling 
// all the operations related to branches in the application
using Microsoft.Data.SqlClient;
using branch.Models;

public class BranchControllers
{
    // Retrieves all branches stored in the database.
    public List<Branch> GetBranch()
    {
        List<Branch> branches = new List<Branch>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id_branch, name_branch, franchise_id FROM tbl_branch";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    branches.Add(new Branch
                    {
                        Id_branch = Convert.ToInt32(reader["id_branch"]),
                        Name_branch = reader["name_branch"].ToString(),
                        FranchiseId = Convert.ToInt32(reader["franchise_id"])
                    });
                }

                Console.WriteLine("Branch list retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving branches: {ex.Message}");
        }

        return branches;
    }

    // Retrieves a specific branch by its unique identifier.
    public Branch GetBranchById(int id)
    {
        Branch branch = null;

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id_branch, name_branch, franchise_id FROM tbl_branch WHERE id_branch = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    branch = new Branch
                    {
                        Id_branch = Convert.ToInt32(reader["id_branch"]),
                        Name_branch = reader["name_branch"].ToString(),
                        FranchiseId = Convert.ToInt32(reader["franchise_id"])
                    };

                    Console.WriteLine($"Branch with ID {id} retrieved successfully.");
                }
                else
                {
                    Console.WriteLine($"No branch found with ID {id}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving branch by ID: {ex.Message}");
        }

        return branch;
    }

    // Retrieves all branches associated with a specific franchise.
    public List<Branch> GetBranchByIdFranchise(int franchiseId)
    {
        List<Branch> branches = new List<Branch>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id_branch, name_branch, franchise_id FROM tbl_branch WHERE franchise_id = @franchiseId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@franchiseId", franchiseId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    branches.Add(new Branch
                    {
                        Id_branch = Convert.ToInt32(reader["id_branch"]),
                        Name_branch = reader["name_branch"].ToString(),
                        FranchiseId = Convert.ToInt32(reader["franchise_id"])
                    });
                }

                Console.WriteLine($"Branches for franchise ID {franchiseId} retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving branches by franchise ID: {ex.Message}");
        }

        return branches;
    }

    // Creates a new branch and ensures the associated franchise exists.
    public Branch CreateBranch(Branch branch)
    {
        var franchiseController = new FranchiseControllers();
        var existingFranchise = franchiseController.GetFranchiseById(branch.FranchiseId);

        if (existingFranchise == null)
        {
            Console.WriteLine($"Franchise with ID {branch.FranchiseId} does not exist.");
            return null;
        }

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string insertQuery = @"INSERT INTO tbl_branch (name_branch, franchise_id)
                                       OUTPUT INSERTED.id_branch
                                       VALUES (@name, @franchiseId)";

                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@name", branch.Name_branch);
                insertCommand.Parameters.AddWithValue("@franchiseId", branch.FranchiseId);

                connection.Open();
                int insertedId = (int)insertCommand.ExecuteScalar();

                branch.Id_branch = insertedId;

                Console.WriteLine($"Branch created successfully with ID {insertedId}.");

                return branch;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating branch: {ex.Message}");
            return null;
        }
    }

    // Updates an existing branch using its unique identifier.
    public Branch UpdateBranch(Branch branch)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"UPDATE tbl_branch
                                SET name_branch = @name, franchise_id = @franchiseId
                                WHERE id_branch = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", branch.Name_branch);
                command.Parameters.AddWithValue("@franchiseId", branch.FranchiseId);
                command.Parameters.AddWithValue("@id", branch.Id_branch);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Branch with ID {branch.Id_branch} updated successfully.");
                    return branch;
                }
                else
                {
                    Console.WriteLine($"Branch with ID {branch.Id_branch} not found for update.");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating branch: {ex.Message}");
            return null;
        }
    }

    // Deletes a branch from the database by its unique identifier.
    public bool DeleteBranch(int id)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "DELETE FROM tbl_branch WHERE id_branch = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Branch with ID {id} deleted successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Branch with ID {id} not found for deletion.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting branch: {ex.Message}");
            return false;
        }
    }
}