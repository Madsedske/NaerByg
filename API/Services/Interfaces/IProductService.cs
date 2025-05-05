using Shared.DTOs;

namespace API.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductsResponse> GetProducts(string seachTerm);
    }
}
