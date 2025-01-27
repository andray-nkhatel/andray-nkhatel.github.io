using Rentbook.DTOs;
using Rentbook.Models;

namespace Rentbook.Services
{
    public interface IHouseService
    {
        Task<Result<int>> GetHouseCountAsync();
        Task<Result<HouseDTO>> GetHouse(int id);
        Task<Result<HouseDTO>> CreateHouse(CreatedHouseDTO houseDTO, List<string> amenities);
        Task<Result<HouseImageDTO>> UploadHouseImage(int houseId, byte[] imageData, string fileName);
        Task<Result<HouseDTO>> UpdateHouse(int id, UpdatedHouseDTO houseDTO, List<string> amenities);
        Task<Result<List<House>>> GetUserPropertiesAsync(string userId);
        Task<Result<bool>> DeleteHouseAsync(int id);
        Task<Result<List<HouseDTO>>> GetAllHousesAsync();
        

    }
}