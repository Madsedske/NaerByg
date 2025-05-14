using MySqlConnector;
using API.Services.Helpers;
using Shared.DTOs;
using System.Data;
using System.Reflection.PortableExecutable;
using API.Services.Interfaces;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _databaseContext;

        public ProductService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<ProductResponse> GetProducts(string searchTerm, int category)
        {
            var products = new List<ProductResponse>();

            using var connection = _databaseContext.CreateConnection();
            {
                using var command = new MySqlCommand("GetProducts", (MySqlConnection)connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                // Add parameter to avoid SQL injection
                command.Parameters.AddWithValue("@search_term", searchTerm);
                command.Parameters.AddWithValue("@sys_category_mapping_id", category);

                // Execute the query
                using (var reader = command.ExecuteReader())
                {
                    // Read the results and populate the buttons list
                    while (reader.Read())
                    {
                        ProductResponse productsResponse = new ProductResponse
                        {
                            ProductId = reader.GetInt32("ProductId"),
                            ProductSku = reader.GetString("ProductSku"),
                            ProductName = reader.GetString("ProductName"),
                            Brand = reader.GetString("Brand"),
                            Price = reader.GetDouble("Price"),
                            Stock = reader.GetInt32("Stock"),
                            PictureURL = reader.GetString("PictureURL"),
                            ChainLogoURL = reader.GetString("ChainLogoURL"),
                            ShopName = reader.GetString("ShopName"),
                            ShopAddress = reader.GetString("ShopAddress"),
                            ShopPostArea = reader.GetString("ShopPostArea"),
                            ShopCity = reader.GetString("ShopCity"),
                            ShopPhoneNo = reader.GetString("ShopPhoneNo"),
                            ShopOpeningHours = reader.GetString("ShopOpeningHours")
                        };
                        products.Add(productsResponse);
                    }
                }
                // Close the connection
                connection.Close();
            }
            return products;
        }
    }
}

