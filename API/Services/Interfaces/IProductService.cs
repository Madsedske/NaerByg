using Shared.DTOs;

namespace API.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductResponse> GetProducts(string seachTerm, int category);
    }
}
