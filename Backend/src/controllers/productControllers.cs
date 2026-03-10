// This file contains the ProductControllers class which is responsible for handling 
// all the operations related to products in the application
using Microsoft.Data.SqlClient;
using product.Models;

public class ProductControllers
{
    // Retrieves all products stored in the database.
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

                Console.WriteLine("Product list retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving products: {ex.Message}");
        }

        return products;
    }

    // Retrieves a product by its unique identifier.
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

                    Console.WriteLine($"Product with ID {id} retrieved successfully.");
                }
                else
                {
                    Console.WriteLine($"No product found with ID {id}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving product by ID: {ex.Message}");
        }

        return product;
    }

    // Retrieves all products associated with a specific branch.
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

                Console.WriteLine($"Products for branch ID {branchId} retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving products by branch ID: {ex.Message}");
        }

        return products;
    }

    // Creates a new product and validates the associated branch exists.
    public Product CreateProduct(Product product)
    {
        var branchController = new BranchControllers();
        var existingBranch = branchController.GetBranchById(product.BranchId);
        if (existingBranch == null)
        {
            Console.WriteLine($"Branch with ID {product.BranchId} does not exist.");
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

                Console.WriteLine($"Product created successfully with ID {insertedId}.");
                return product;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating product: {ex.Message}");
            return null;
        }
    }

    // Updates an existing product using its unique identifier.
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
                    Console.WriteLine($"Product with ID {product.Id_product} updated successfully.");
                    return product;
                }
                else
                {
                    Console.WriteLine($"Product with ID {product.Id_product} not found for update.");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product: {ex.Message}");
            return null;
        }
    }

    // Deletes a product from the database by its unique identifier.
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

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Product with ID {id} deleted successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Product with ID {id} not found for deletion.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product: {ex.Message}");
            return false;
        }
    }
}