using HouseRentingSystem.Core.Models.House;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IHouseService
    {
        Task<IEnumerable<HouseHomeModel>> GetLastThreeHousesAsync();

        Task<IEnumerable<HouseCategoryModel>> AllCategoriesAsync();

        Task<bool> CategoryExistsAsync(int categoryId);

        Task<int> CreateAsync(HouseModel model, int agentId);

        Task<HousesQueryModel> AllAsync(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNamesAsync();

        Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int id);

        Task<IEnumerable<HouseServiceModel>> AllHousesByUSerId(string userId);

        Task<HouseDetailsModel> HouseDetailsById(int id);

        Task<bool> Exists(int id);

        Task Edit(int houseId, HouseModel model);

        Task<bool> HasAgentWithId(int houseId, string currentUserId);

        Task<int> GetHouseCategoryId(int houseId);

        Task Delete(int houseId);

        Task<bool> IsRented(int houseId);

        Task<bool> IsRentedByUserWithId(int houseId, string userId);

        Task Rent(int houseId, string userId);

        Task Leave(int houseId);
    }
}
