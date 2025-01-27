using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Rentbook.DTOs;
using Rentbook.Models;

namespace Rentbook.Services
{
    public class HouseService : IHouseService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HouseService> _logger;

        public HouseService(HttpClient httpClient, ILogger<HouseService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Result<List<HouseDTO>>> GetAllHousesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/houses");
                _logger.LogInformation("Fetched house data with status code: {StatusCode}", response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    // Read the content as a list of HouseDTO
                    var houses = await response.Content.ReadFromJsonAsync<List<HouseDTO>>();
                    return Result<List<HouseDTO>>.Success(houses);
                }
               
                return Result<List<HouseDTO>>.Failure($"Failed to get houses. Status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching houses.");
                return Result<List<HouseDTO>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<HouseDTO>> GetHouse(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/houses/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var house = await response.Content.ReadFromJsonAsync<HouseDTO>();
                    return Result<HouseDTO>.Success(house);
                }
                return Result<HouseDTO>.Failure($"Failed to get house. Status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return Result<HouseDTO>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<HouseDTO>> CreateHouse(CreatedHouseDTO houseDTO, List<string> amenities)
        {
            try
            {
                var createDTO = new CreateHouseDTO
                {
                    Address = houseDTO.Address,
                    Vicinity = houseDTO.Vicinity,
                    City = houseDTO.City,
                    Price = houseDTO.Price,
                    Bedrooms = houseDTO.Bedrooms,
                    Bathrooms = houseDTO.Bathrooms,
                    SquareFootage = houseDTO.SquareFootage,
                    PropertyType = houseDTO.PropertyType,
                    IsFurnished = houseDTO.IsFurnished,
                    PetsAllowed = houseDTO.PetsAllowed,
                    Description = houseDTO.Description,
                    IsAvailable = houseDTO.IsAvailable,
                    Amenities = amenities
                };

                var response = await _httpClient.PostAsJsonAsync("api/houses", createDTO);
                if (response.IsSuccessStatusCode)
                {
                    var createdHouse = await response.Content.ReadFromJsonAsync<HouseDTO>();
                    return Result<HouseDTO>.Success(createdHouse);
                }
                return Result<HouseDTO>.Failure($"Failed to create house. Status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return Result<HouseDTO>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<HouseImageDTO>> UploadHouseImage(int houseId, byte[] imageData, string fileName)
        {
            try
            {
                var content = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(imageData);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Adjust if needed

                content.Add(imageContent, "image", fileName);

                var response = await _httpClient.PostAsync($"api/houses/{houseId}/images", content);

                if (response.IsSuccessStatusCode)
                {
                    var houseImageDTO = await response.Content.ReadFromJsonAsync<HouseImageDTO>();
                    return Result<HouseImageDTO>.Success(houseImageDTO);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result<HouseImageDTO>.Failure($"Failed to upload image. Status code: {response.StatusCode}. Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                return Result<HouseImageDTO>.Failure($"An error occurred while uploading the image: {ex.Message}");
            }
        }

        public async Task<Result<List<House>>> GetUserPropertiesAsync(string userId)
        {
            try
            {
                var houses = await _httpClient.GetFromJsonAsync<List<House>>("api/houses");
                var userHouses = houses.Where(h => h.OwnerId == userId).ToList();
                return Result<List<House>>.Success(userHouses);
            }
            catch (Exception ex)
            {
                return Result<List<House>>.Failure($"Error fetching houses: {ex.Message}");
            }
        }

        public async Task<Result<HouseDTO>> UpdateHouse(int id, UpdatedHouseDTO houseDTO, List<string> amenities)
        {
            try
            {
                var updateDTO = new UpdatedHouseDTO
                {
                    Id = id,
                    Address = houseDTO.Address,
                    Vicinity = houseDTO.Vicinity,
                    City = houseDTO.City,
                    Price = houseDTO.Price,
                    PropertyType = houseDTO.PropertyType,
                    Bedrooms = houseDTO.Bedrooms,
                    Bathrooms = houseDTO.Bathrooms,
                    SquareFootage = houseDTO.SquareFootage,
                    IsFurnished = houseDTO.IsFurnished,
                    PetsAllowed = houseDTO.PetsAllowed,
                    Description = houseDTO.Description,
                    IsAvailable = houseDTO.IsAvailable,
                    LastUpdated = houseDTO.LastUpdated,
                    Amenities = amenities
                };

                var response = await _httpClient.PutAsJsonAsync($"api/houses/{id}", updateDTO);
                if (response.IsSuccessStatusCode)
                {
                    var updatedHouse = await response.Content.ReadFromJsonAsync<HouseDTO>();
                    return Result<HouseDTO>.Success(updatedHouse);
                }
                return Result<HouseDTO>.Failure($"Failed to update house. Status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return Result<HouseDTO>.Failure($"An error occurred: {ex.Message}");
            }
        }
        public async Task<Result<int>> GetHouseCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/houses/count");
                if (response.IsSuccessStatusCode)
                {
                    var count = await response.Content.ReadFromJsonAsync<int>();
                    return Result<int>.Success(count);
                }
                return Result<int>.Failure($"Failed to get house count. Status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteHouseAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/houses/{id}");
               
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Deleted house with code: {StatusCode}", response.StatusCode);
                    return Result<bool>.Success(true);
                }
                _logger.LogInformation("Failed to delete house: {StatusCode}", response.StatusCode);
                return Result<bool>.Failure($"Failed to delete house. Status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"An error occurred while deleting the house: {ex.Message}");
            }
        }

    }
}