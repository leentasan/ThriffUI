using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ThriffSignUp.Model
{
    public class ProductService
    {
        private readonly string connectionString;

        public ProductService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
        }

        public void AddProduct(Product product)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("INSERT INTO Product (Name, Price, ImagePath, Description) VALUES (@name, @price, @imagePath, @description)", connection);
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@imagePath", product.ImagePath);
                command.Parameters.AddWithValue("@description", product.Description);
                command.ExecuteNonQuery();
            }
        }
        public void UpdateProduct(Product product)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("UPDATE Product SET Name = @name, Price = @price, ImagePath = @imagePath, Description = @description WHERE ProductId = @id", connection);
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@imagePath", product.ImagePath);
                command.Parameters.AddWithValue("@description", product.Description);
                command.Parameters.AddWithValue("@id", product.ProductId);
                command.ExecuteNonQuery();
            }
        }
        public Product GetProductById(int productId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT ProductId, Name, Price, ImagePath, Description FROM Product WHERE ProductId = @id", connection);
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
                            Description = reader.IsDBNull(4) ? null : reader.GetString(4)
                        };
                    }
                }
            }
            return null;
        }

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT Name, Price, ImagePath, Description FROM Product", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            Name = reader.GetString(0),
                            Price = reader.GetDouble(1),
                            ImagePath = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                        products.Add(product);
                    }
                }
            }
            return products;
        }
    }
}
