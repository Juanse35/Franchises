using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using franchise.Models; 

public class FranchiseControllers
{
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
            Console.WriteLine($"Error al obtener franquicias: {ex.Message}");
        }

        return franchises;
    }

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
                        CreatedAt = Convert.ToDateTime(reader["registration_date"]) // <-- corregido
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener franquicia por ID: {ex.Message}");
        }

        return franchise;
    }

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

                return franchise;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear franquicia: {ex.Message}");
            return null;
        }
    }

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

                return rowsAffected > 0; // Devuelve true si se eliminó alguna fila
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar franquicia: {ex.Message}");
            return false;
        }
    }

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
                    // Si se actualizó correctamente, devolvemos la franquicia con los datos actualizados
                    franchise.CreatedAt = GetFranchiseById(franchise.Id)?.CreatedAt ?? DateTime.Now;
                    return franchise;
                }
                else
                {
                    return null; // No se encontró la franquicia para actualizar
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar franquicia: {ex.Message}");
            return null;
        }
    }
}
