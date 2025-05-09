using bmAPI.DTO;
using bmAPI.Enums;
using bmAPI.Extensions;
using bmAPI.Services.Helpers;
using MySqlConnector;
using System.Data;
using System.Reflection.PortableExecutable;

namespace bmAPI.Services
{
    public class DataService : IDataService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory _dbContextFactory;

        public DataService(IConfiguration configuration, IDbContextFactory dbContextFactory)
        {
            _configuration = configuration;
            _dbContextFactory = dbContextFactory;
        }

        public List<ProductResponse> ReturnProducts(int chainId, DateTime lastUpdated)
        {
            return ReturnData(chainId, lastUpdated, DataEndpoint.Product, reader => new ProductResponse
            {
                ProductId = reader.GetInt32("product_id"),
                Sku = reader.GetString("sku"),
                Name = reader.GetString("name"),
                CategoryId = reader.GetInt32("category_id"),
                BrandId = reader.GetInt32("brand_id"),
                ImageURL = reader.GetString("image_url"),
                IsActive = reader.GetString("is_active"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            }
            );
        }

        public List<ShopResponse> ReturnShops(int chainId, DateTime lastUpdated)
        {
            return ReturnData(chainId, lastUpdated, DataEndpoint.Shop, reader => new ShopResponse
            {
                ShopId = reader.GetInt32("shop_id"),
                Name = reader.GetString("name"),
                Address = reader.GetString("address"),
                PostAreaId = reader.GetInt32("post_area_id"),
                PhoneNo = reader.GetString("phone_no"),
                OpeningHours = reader.GetString("opening_hours"),
                IsActive = reader.GetString("is_active"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            }
            );
        }

        public List<MtmShopProductResponse> ReturnShopProducts(int chainId, DateTime lastUpdated)
        {
            return ReturnData(chainId, lastUpdated, DataEndpoint.MtmShopProduct, reader => new MtmShopProductResponse
            {
                ShopProductId = reader.GetInt32("shop_product_id"),
                ShopId = reader.GetInt32("shop_id"),
                ProductId = reader.GetInt32("product_id"),
                Quantity = reader.GetInt32("quantity"),
                Price = reader.GetDouble("price"),
                IsActive = reader.GetString("is_active"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            }
            );
        }

        public List<PostAreaResponse> ReturnPostAreas(int chainId, DateTime lastUpdated)
        {
            return ReturnData(chainId, lastUpdated, DataEndpoint.PostArea, reader => new PostAreaResponse
            {
                PostAreaId = reader.GetInt32("post_area_id"),
                Code = reader.GetInt32("code"),
                City = reader.GetString("city"),
                IsActive = reader.GetString("is_active"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            }
            );
        }

        public List<BrandResponse> ReturnBrands(int chainId, DateTime lastUpdated)
        {
            return ReturnData(chainId, lastUpdated, DataEndpoint.Brand, reader => new BrandResponse
            {
                BrandId = reader.GetInt32("brand_id"),
                Name = reader.GetString("name"),
                IsActive = reader.GetString("is_active"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            }
            );
        }

        public List<CategoryResponse> ReturnCategories(int chainId, DateTime lastUpdated)
        {
            return ReturnData(chainId, lastUpdated, DataEndpoint.Category, reader => new CategoryResponse
                {
                    CategoryId = reader.GetInt32("category_id"),
                    Name = reader.GetString("name"),
                    ParentId = reader.IsDBNull("parent_id") ? (int?)null : reader.GetInt32("parent_id"),
                    IsActive = reader.GetString("is_active"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    UpdatedAt = reader.GetDateTime("updated_at")
                }
            );
        }
       
        public List<TResponse> ReturnData<TResponse>(
            int chainId,
            DateTime lastUpdated,
            DataEndpoint endpoint,
            Func<MySqlDataReader,TResponse> mapFunc
        )
        {
            var storedProcedureName = endpoint.ToStoredProcedureName();
            var data = new List<TResponse>();

            using (var context = _dbContextFactory.Create(chainId)) {
                using (var connection = context.CreateConnection())
                {
                    connection.Open();

                    using var command = new MySqlCommand(storedProcedureName, (MySqlConnection)connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@last_update", lastUpdated);

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = mapFunc(reader);
                        data.Add(item);
                    }

                    return data;
                }
            }
            
        }
    }
}
