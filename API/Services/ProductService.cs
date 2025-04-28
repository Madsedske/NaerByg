using MySqlConnector;
using API.Services.Helpers;
using Shared.DTOs;
using Shared.Models;
using System.Data;

namespace API.Services
{
    public class ProductService : IProductService
    {

        private readonly DatabaseContext _databaseContext;

        public ProductService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ProductsResponse GetProducts(string searchTerm)
        {

            var products = new List<Product>();

            using (var connection = _databaseContext.CreateConnection())
            {
                // Open the connection
                connection.Open();

                // Prepare the SELECT query
                string storedProcedureName = "GetProducts";
                //string query = "SELECT d.b1_event, d.b2_event, d.b3_event, d.b4_event FROM Device d, User u WHERE u.username = @username and u.device_id = d.device_id";

                // Create a MySqlCommand object
                using (var command = new MySqlCommand(storedProcedureName, (MySqlConnection)connection))
                {
                    // Adding type, to specify it's a Stored Procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameter to avoid SQL injection
                    command.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    // Execute the query
                    using (var reader = command.ExecuteReader())
                    {
                        // Read the results and populate the buttons list
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = reader.GetInt32("ProductId"),
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
                            });
                            }
                        }
                    }
                    // Close the connection
                    connection.Close();
                }
                return new ProductsResponse
                {
                    Products = products
                };
            
        }
    }
    
}
