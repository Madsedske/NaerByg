using Shared.DTOs;

namespace API.Services
{
    public interface IProductService
    {
        List<ProductResponse> GetProducts(string seachTerm);
    }
}
