using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using product.Models;
using branch.Models;

public class ProductControllers
{
    public List<Product> GetProduct()
    {
        List<Product> products = new List<Product>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id_product, name_product, imag_url, branch_id FROM tbl_product";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id_product = Convert.ToInt32(reader["id_product"]),
                        Name_product = reader["name_product"].ToString(),
                        Imag_url = reader["imag_url"]?.ToString(),
                        BranchId = Convert.ToInt32(reader["branch_id"])
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener productos: {ex.Message}");
        }

        return products;
    }

    public Product CreateProduct(Product product)
    {
        // Validar que la sucursal exista
        var branchController = new BranchControllers();
        var existingBranch = branchController.GetBranchById(product.BranchId);
        if (existingBranch == null)
        {
            Console.WriteLine("La sucursal con ID " + product.BranchId + " no existe.");
            return null;
        }

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string insertQuery = @"INSERT INTO tbl_product (name_product, imag_url, branch_id)
                                       OUTPUT INSERTED.id_product
                                       VALUES (@name, @imageUrl, @branchId)";

                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@name", product.Name_product);
                insertCommand.Parameters.AddWithValue("@imageUrl", (object)product.Imag_url ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@branchId", product.BranchId);

                connection.Open();
                int insertedId = (int)insertCommand.ExecuteScalar();

                product.Id_product = insertedId;

                return product;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear producto: {ex.Message}");
            return null;
        }
    }

    public Product GetProductById(int id)
    {
        Product product = null;

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id_product, name_product, imag_url, branch_id FROM tbl_product WHERE id_product = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    product = new Product
                    {
                        Id_product = Convert.ToInt32(reader["id_product"]),
                        Name_product = reader["name_product"].ToString(),
                        Imag_url = reader["imag_url"]?.ToString(),
                        BranchId = Convert.ToInt32(reader["branch_id"])
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener producto por ID: {ex.Message}");
        }

        return product;
    }

    public List<Product> GetProductByIdBranch(int branchId)
    {
        List<Product> products = new List<Product>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "SELECT id_product, name_product, imag_url, branch_id FROM tbl_product WHERE branch_id = @branchId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@branchId", branchId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id_product = Convert.ToInt32(reader["id_product"]),
                        Name_product = reader["name_product"].ToString(),
                        Imag_url = reader["imag_url"]?.ToString(),
                        BranchId = Convert.ToInt32(reader["branch_id"])
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener productos por ID de sucursal: {ex.Message}");
        }

        return products;
    }

    public Product UpdateProduct(Product product)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"UPDATE tbl_product
                                SET name_product = @name, imag_url = @imageUrl, branch_id = @branchId
                                WHERE id_product = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", product.Name_product);
                command.Parameters.AddWithValue("@imageUrl", (object)product.Imag_url ?? DBNull.Value);
                command.Parameters.AddWithValue("@branchId", product.BranchId);
                command.Parameters.AddWithValue("@id", product.Id_product);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return product;
                }
                else
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar producto: {ex.Message}");
            return null;
        }
    }

    public bool DeleteProduct(int id)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = "DELETE FROM tbl_product WHERE id_product = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar producto: {ex.Message}");
            return false;
        }
    }
}
