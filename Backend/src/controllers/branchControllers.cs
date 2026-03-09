using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using branch.Models;

public class BranchControllers
{
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener sucursales: {ex.Message}");
        }

        return branches;
    }

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
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener sucursal por ID: {ex.Message}");
        }

        return branch;
    }

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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener sucursales por ID de franquicia: {ex.Message}");
        }

        return branches;
    }

    public Branch CreateBranch(Branch branch)
    {
        var franchiseController = new FranchiseControllers();
        var existingFranchise = franchiseController.GetFranchiseById(branch.FranchiseId);
        if (existingFranchise == null)
        {
            Console.WriteLine("La franquicia con ID " + branch.FranchiseId + " no existe.");
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

                return branch;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear sucursal: {ex.Message}");
            return null;
        }
    }

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
                    return branch;
                }
                else
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar sucursal: {ex.Message}");
            return null;
        }
    }

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

                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar sucursal: {ex.Message}");
            return false;
        }
    }
}
