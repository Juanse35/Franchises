using Microsoft.Data.SqlClient;
using product.Models;

public class ProductControllers
{
    // Retrieves all products along with branch and franchise names
    public List<Product> GetProduct()
    {
        List<Product> products = new List<Product>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"
                SELECT 
                    p.id_product,
                    p.name_product,
                    p.stock,
                    p.branch_id,
                    p.registration_date,
                    b.name_branch,
                    f.name AS franchise_name
                FROM tbl_product p
                INNER JOIN tbl_branch b ON p.branch_id = b.id_branch
                INNER JOIN tbl_franchise f ON b.franchise_id = f.id;";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id_product = Convert.ToInt32(reader["id_product"]),
                            Name_product = reader["name_product"].ToString(),
                            Stock = Convert.ToInt32(reader["stock"]),
                            BranchId = Convert.ToInt32(reader["branch_id"]),
                            BranchName = reader["name_branch"].ToString(),
                            FranchiseName = reader["franchise_name"].ToString()
                        };

                        products.Add(product);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("SQL ERROR:");
            Console.WriteLine(ex.Message);
        }

        return products;
    }
    // Retrieves a single product by its ID along with branch and franchise names
    public Product GetProductById(int id)
    {
        Product product = null;

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"
                    SELECT 
                        p.id_product,
                        p.name_product,
                        p.stock,
                        p.branch_id,
                        b.name_branch,
                        f.name AS franchise_name
                    FROM tbl_product p
                    INNER JOIN tbl_branch b ON p.branch_id = b.id_branch
                    INNER JOIN tbl_franchise f ON b.franchise_id = f.id
                    WHERE p.id_product = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            Id_product = Convert.ToInt32(reader["id_product"]),
                            Name_product = reader["name_product"].ToString(),
                            Stock = Convert.ToInt32(reader["stock"]),
                            BranchId = Convert.ToInt32(reader["branch_id"]),
                            BranchName = reader["name_branch"].ToString(),
                            FranchiseName = reader["franchise_name"].ToString()
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving product by ID: {ex.Message}");
        }

        return product;
    }

    // Retrieves all products for a specific branch along with branch and franchise names
    public List<Product> GetProductByIdBranch(int branchId)
    {
        List<Product> products = new List<Product>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"
                    SELECT 
                        p.id_product,
                        p.name_product,
                        p.stock,
                        p.branch_id,
                        b.name_branch,
                        f.name AS franchise_name
                    FROM tbl_product p
                    INNER JOIN tbl_branch b ON p.branch_id = b.id_branch
                    INNER JOIN tbl_franchise f ON b.franchise_id = f.id
                    WHERE p.branch_id = @branchId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@branchId", branchId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id_product = Convert.ToInt32(reader["id_product"]),
                            Name_product = reader["name_product"].ToString(),
                            Stock = Convert.ToInt32(reader["stock"]),
                            BranchId = Convert.ToInt32(reader["branch_id"]),
                            BranchName = reader["name_branch"].ToString(),
                            FranchiseName = reader["franchise_name"].ToString()
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving products by branch ID: {ex.Message}");
        }

        return products;
    }

    // Creates a new product in the database
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
                string insertQuery = @"
                    INSERT INTO tbl_product (name_product, branch_id, stock)
                    OUTPUT INSERTED.id_product
                    VALUES (@name, @branchId, @stock)";

                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@name", product.Name_product);
                insertCommand.Parameters.AddWithValue("@branchId", product.BranchId);
                insertCommand.Parameters.AddWithValue("@stock", product.Stock);

                connection.Open();
                int insertedId = (int)insertCommand.ExecuteScalar();
                product.Id_product = insertedId;

                return product;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating product: {ex.Message}");
            return null;
        }
    }

    // Updates an existing product (only name and stock are allowed to change)
    public Product UpdateProduct(Product product)
    {
        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                string query = @"
                    UPDATE tbl_product
                    SET name_product = @name, stock = @stock
                    WHERE id_product = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", product.Name_product);
                command.Parameters.AddWithValue("@stock", product.Stock);
                command.Parameters.AddWithValue("@id", product.Id_product);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0) return product;
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product: {ex.Message}");
            return null;
        }
    }

    // Deletes a product from the database by its ID
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
                return command.ExecuteNonQuery() > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product: {ex.Message}");
            return false;
        }
    }
}