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

        public List<ProductsResponse> GetProducts(string searchTerm)
        {
            var products = new List<ProductsResponse>();

            ProductsResponse productsResponse = new ProductsResponse
            {
                ProductId = 1,
                ProductSku = "hg738h34h439f34",
                ProductName = "Møtrækker",
                Brand = "BOSCH",
                Price = 453.4,
                Stock = 3,
                PictureURL = "staerk/bahco/10.jpg",
                ChainLogoURL = "/fjeife/fejifje/jie.png",
                ShopName = "Stærk",
                ShopAddress = "Toften 7",
                ShopPostArea = "4100",
                ShopCity = "Ringsted",
                ShopPhoneNo = "69696969",
                ShopOpeningHours = "Man: 09:00-18:00"
            };

            products.Add(productsResponse);

            ProductsResponse productResponse = new ProductsResponse
            {
                ProductId = 2,
                ProductSku = "hg7g433g348ggh34gf34",
                ProductName = "Monteringshammer",
                Brand = "Bacho",
                Price = 14233.4,
                Stock = 12,
                PictureURL = "harrag_nybold/bahco/11.jpg",
                ChainLogoURL = "/fjeifje/jie.png",
                ShopName = "Harrag Nybold",
                ShopAddress = "Teglovnsvej 39",
                ShopPostArea = "4100",
                ShopCity = "Ringsted",
                ShopPhoneNo = "12345678",
                ShopOpeningHours = "tirsd: 09:00-18:00"
            };

            products.Add(productResponse);

            return products;

            /*
            var products = new List<ProductsResponse>();

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
                    command.Parameters.AddWithValue("@search_term", searchTerm);

                    // Execute the query
                    using (var reader = command.ExecuteReader())
                    {
                        // Read the results and populate the buttons list
                        while (reader.Read())
                        {
                            ProductsResponse productsResponse = new ProductsResponse
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
                return products;*/
        }
    }
}

