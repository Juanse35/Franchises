// Controller responsible for handling all operations related to products
// Includes validation rules to ensure data integrity before interacting with the database.

using Microsoft.Data.SqlClient;
using product.Models;

public class ProductControllers
{
    // Retrieves all products including their branch and franchise information
    public List<Product> GetProduct()
    {
        // List used to store products retrieved from the database
        List<Product> products = new List<Product>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                // Query joins products, branches, and franchises to return complete product information
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

                // Execute reader to iterate through query results
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
            // Log SQL errors
            Console.WriteLine("SQL ERROR:");
            Console.WriteLine(ex.Message);
        }

        return products;
    }

    // Retrieves a single product by its unique identifier
    public Product GetProductById(int id)
    {
        Product product = null;

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                // Query retrieves the product together with its branch and franchise
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

                // Prevent SQL injection by using parameters
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

    // Retrieves all products that belong to a specific branch
    public List<Product> GetProductByIdBranch(int branchId)
    {
        List<Product> products = new List<Product>();

        try
        {
            using (SqlConnection connection = ConnectionServer.GetConnection())
            {
                // Query filters products by branch ID
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

    // Creates a new product after validating business rules
    public Product CreateProduct(Product product)
    {
        // Validate that the branch exists before inserting the product
        var branchController = new BranchControllers();
        var existingBranch = branchController.GetBranchById(product.BranchId);

        if (existingBranch == null)
        {
            Console.WriteLine($"Branch with ID {product.BranchId} does not exist.");
            return null;
        }

        // Validation: stock must not be negative
        if (product.Stock < 0)
        {
            Console.WriteLine("Stock cannot be negative.");
            return null;
        }

        // Validation: product name must not be empty
        if (string.IsNullOrWhiteSpace(product.Name_product))
        {
            Console.WriteLine("Product name cannot be empty.");
            return null;
        }

        // Standardize product name (remove spaces and convert to lowercase)
        product.Name_product = product.Name_product.Trim().ToLower();

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

                // Execute insert and retrieve generated ID
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

    // Updates an existing product after validating input values
    public Product UpdateProduct(Product product)
    {
        // Validation: stock cannot be negative
        if (product.Stock < 0)
        {
            Console.WriteLine("Stock cannot be negative.");
            return null;
        }

        // Validation: product name cannot be empty
        if (string.IsNullOrWhiteSpace(product.Name_product))
        {
            Console.WriteLine("Product name cannot be empty.");
            return null;
        }

        // Normalize product name before updating
        product.Name_product = product.Name_product.Trim().ToLower();

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

                if (rowsAffected > 0)
                    return product;

                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product: {ex.Message}");
            return null;
        }
    }

    // Deletes a product by its ID
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