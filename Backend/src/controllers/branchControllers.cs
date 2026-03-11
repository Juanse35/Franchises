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
                string query = @"
                    SELECT 
                        b.id_branch,
                        b.name_branch,
                        b.franchise_id,
                        f.name AS franchise_name,
                        b.registration_date
                    FROM tbl_branch b
                    INNER JOIN tbl_franchise f 
                        ON b.franchise_id = f.id";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Map database values to Branch model
                    branches.Add(new Branch
                    {
                        Id_branch = Convert.ToInt32(reader["id_branch"]),
                        Name_branch = reader["name_branch"].ToString(),
                        FranchiseId = Convert.ToInt32(reader["franchise_id"]),
                        FranchiseName = reader["franchise_name"].ToString(),
                        RegistrationDate = Convert.ToDateTime(reader["registration_date"])
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
                string query = @"
                    SELECT 
                        b.id_branch,
                        b.name_branch,
                        b.franchise_id,
                        f.name AS franchise_name,
                        b.registration_date
                    FROM tbl_branch b
                    INNER JOIN tbl_franchise f 
                        ON b.franchise_id = f.id
                    WHERE b.id_branch = @id";

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
                        FranchiseId = Convert.ToInt32(reader["franchise_id"]),
                        FranchiseName = reader["franchise_name"].ToString(),
                        RegistrationDate = Convert.ToDateTime(reader["registration_date"])
                    };

                    Console.WriteLine($"Branch with ID {id} retrieved successfully.");
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
                string query = @"
                    SELECT 
                        b.id_branch,
                        b.name_branch,
                        b.franchise_id,
                        f.name AS franchise_name,
                        b.registration_date
                    FROM tbl_branch b
                    INNER JOIN tbl_franchise f 
                        ON b.franchise_id = f.id
                    WHERE b.franchise_id = @franchiseId";

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
                        FranchiseId = Convert.ToInt32(reader["franchise_id"]),
                        FranchiseName = reader["franchise_name"].ToString(),
                        RegistrationDate = Convert.ToDateTime(reader["registration_date"])
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
        // Validate branch name
        if (string.IsNullOrWhiteSpace(branch.Name_branch))
        {
            Console.WriteLine("Branch name is required.");
            return null;
        }

        // Normalize branch name:
        // Trim spaces and convert to lowercase to maintain consistency in the database
        branch.Name_branch = branch.Name_branch.Trim().ToLowerInvariant();

        // Verify that the franchise exists before creating the branch
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
        // Validate branch name
        if (string.IsNullOrWhiteSpace(branch.Name_branch))
        {
            Console.WriteLine("Branch name is required.");
            return null;
        }

        // Normalize branch name before updating
        branch.Name_branch = branch.Name_branch.Trim().ToLowerInvariant();

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
    // Also deletes all products associated with the branch.
    public bool DeleteBranch(int id)
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
                        // Verify if the branch exists
                        string checkBranchQuery = "SELECT COUNT(*) FROM tbl_branch WHERE id_branch = @id";

                        SqlCommand checkCmd = new SqlCommand(checkBranchQuery, connection, transaction);
                        checkCmd.Parameters.AddWithValue("@id", id);

                        int branchExists = (int)checkCmd.ExecuteScalar();

                        if (branchExists == 0)
                        {
                            Console.WriteLine($"Branch with ID {id} does not exist.");
                            transaction.Rollback();
                            return false;
                        }

                        // Delete products associated with the branch
                        string deleteProductsQuery = "DELETE FROM tbl_product WHERE branch_id = @branchId";

                        SqlCommand deleteProductsCmd = new SqlCommand(deleteProductsQuery, connection, transaction);
                        deleteProductsCmd.Parameters.AddWithValue("@branchId", id);

                        deleteProductsCmd.ExecuteNonQuery();

                        // Delete the branch
                        string deleteBranchQuery = "DELETE FROM tbl_branch WHERE id_branch = @id";

                        SqlCommand deleteBranchCmd = new SqlCommand(deleteBranchQuery, connection, transaction);
                        deleteBranchCmd.Parameters.AddWithValue("@id", id);

                        int rowsAffected = deleteBranchCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            Console.WriteLine($"Branch {id} and its products deleted successfully.");
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Branch with ID {id} could not be deleted.");
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