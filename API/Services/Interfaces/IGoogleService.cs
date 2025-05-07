using Shared.DTOs;

namespace API.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleDistanceResponse> CalculateDistance(string inputAddress, string shopAddress, string postarea);
    }
}
