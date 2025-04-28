using Shared.DTOs;

namespace API.Services
{
    public interface IProductService
    {
        ProductsResponse GetProducts(string seachTerm);
    }
}
