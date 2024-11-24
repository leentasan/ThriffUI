using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace ThriffSignUp.Model
{
    public class ProductService
    {
        private readonly string connectionString;

        public ProductService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
        }

        public void AddProduct(Product product, int SellerId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("INSERT INTO Product (Name, Price, Image_path, Description, sellerid, category) VALUES (@name, @price, @imagePath, @description, @sellerId, @category::product_category)", connection);
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@imagePath", product.ImagePath);
                command.Parameters.AddWithValue("@description", product.Description);
                command.Parameters.AddWithValue("@sellerId", product.SellerId);
                command.ExecuteNonQuery();

            }
        }
        public void UpdateProduct(Product product)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("UPDATE Product SET Name = @name, Price = @price, Image_path = @imagePath, Description = @description, category=@category::product_category WHERE ProductId = @id", connection);
                    command.Parameters.AddWithValue("@name", product.Name);
                    command.Parameters.AddWithValue("@price", product.Price);
                    command.Parameters.AddWithValue("@imagePath", product.ImagePath);
                    command.Parameters.AddWithValue("@description", product.Description);
                    command.Parameters.AddWithValue("@id", product.ProductId);
                    command.Parameters.AddWithValue("@category", product.Category);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update product{ex.Message}");
            }
        }
        public Product GetProductById(int productId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("SELECT ProductId, Name, Price, Image_path, Description, category FROM Product WHERE ProductId = @id", connection);
                    command.Parameters.AddWithValue("@id", productId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDouble(2),
                                ImagePath = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Category = reader.GetString(5)
                            };
                        }
                        else
                        {
                            MessageBox.Show($"Product not found in database");
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error retrieving product with ID {productId}: {ex.Message}");
            }
            return null;
        }
        public void DeleteProduct(int productId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("DELETE FROM Product WHERE ProductId = @id", connection);
                    command.Parameters.AddWithValue("@id", productId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete product: {ex.Message}");
            }
        }
        public List<Product> GetAllProducts(int sellerid)
        {
            var products = new List<Product>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT productid, Name, Price, Image_path, Description, category::product_category FROM Product WHERE SellerId = @SellerId", connection);
                command.Parameters.AddWithValue("@SellerId", sellerid);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            ProductId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2),
                            ImagePath = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Category = reader.GetString(5)
                        };
                        products.Add(product);
                    }
                }
            }
            return products;
        }
    }
}
