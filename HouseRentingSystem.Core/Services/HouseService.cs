using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Exceptions;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository repo;
        private readonly IGuard guard;

        public HouseService(
            IRepository _repo,
            IGuard _guard)
        {
            repo = _repo;
            guard = _guard;
        }

        public async Task<HousesQueryModel> AllAsync(string? category = null, string? searchTerm = null, HouseSorting sorting = HouseSorting.Newest, int currentPage = 1, int housesPerPage = 1)
        {
            var result = new HousesQueryModel();

            var houses = repo.AllReadonly<House>()
                .Where(h => h.IsActive);

            if (string.IsNullOrEmpty(category) == false)
            {
                houses = houses
                    .Where(h => h.Category.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                houses = houses
                    .Where(h => EF.Functions.Like(h.Title.ToLower(), searchTerm) ||
                     EF.Functions.Like(h.Address.ToLower(), searchTerm) ||
                     EF.Functions.Like(h.Description.ToLower(), searchTerm));
            }

            houses = sorting switch
            {
                HouseSorting.Price => houses
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => houses
                    .OrderBy(h => h.RenterId),
                _ => houses.OrderByDescending(h => h.Id)
            };

            result.Houses = await houses
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel
                {
                    Id = h.Id,
                    Address = h.Address,
                    Title = h.Title,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null
                })
                .ToListAsync();

            result.TotalHousesCount = await houses.CountAsync();

            return result;
        }

        public async Task<IEnumerable<HouseCategoryModel>> AllCategoriesAsync()
        {
            return await repo.AllReadonly<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new HouseCategoryModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> AllCategoriesNamesAsync()
        {
            return await repo
                .AllReadonly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int id)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.AgentId == id)
                .Select(h => new HouseServiceModel
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUSerId(string userId)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.RenterId == userId)
                .Select(h => new HouseServiceModel
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
            => await repo.AllReadonly<Category>()
                         .AnyAsync(c => c.Id == categoryId);

        public async Task<int> CreateAsync(HouseModel model, int agentId)
        {
            var house = new House
            {
                Title = model.Title,
                Address = model.Address,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                AgentId = agentId
            };

            await repo.AddAsync(house);

            await repo.SaveChangesAsync();

            return house.Id;
        }

        public async Task Delete(int houseId)
        {
            var house = await repo.GetByIdAsync<House>(houseId);
            house.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task Edit(int houseId, HouseModel model)
        {
            var house = await repo.GetByIdAsync<House>(houseId);

            house.Title = model.Title;
            house.Address = model.Address;
            house.CategoryId = model.CategoryId;
            house.Description = model.Description;
            house.ImageUrl = model.ImageUrl;
            house.PricePerMonth = model.PricePerMonth;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .AnyAsync(h => h.Id == id);
        }

        public async Task<int> GetHouseCategoryId(int houseId)
        {
            return (await repo.GetByIdAsync<House>(houseId)).CategoryId;
        }

        public async Task<IEnumerable<HouseHomeModel>> GetLastThreeHousesAsync()
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.Id)
                .Select(h => new HouseHomeModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    ImageUrl = h.ImageUrl
                })
                .Take(3)
                .ToListAsync();
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            bool result = false;

            var house = await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == houseId)
                .Include(h => h.Agent)
                .FirstOrDefaultAsync();

            if (house?.Agent != null && house.Agent.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<HouseDetailsModel> HouseDetailsById(int id)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == id)
                .Select(h => new HouseDetailsModel
                {
                    Address = h.Address,
                    Category = h.Category.Name,
                    Description = h.Description,
                    Id = id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title,
                    Agent = new Models.Agent.AgentServiceModel
                    {
                        Email = h.Agent.User.Email,
                        PhoneNumber = h.Agent.PhoneNumber
                    }
                })
                .FirstAsync();
        }

        public async Task<bool> IsRented(int houseId)
        {
            return (await repo.GetByIdAsync<House>(houseId)).RenterId != null;
        }

        public async Task<bool> IsRentedByUserWithId(int houseId, string userId)
        {
            bool result = false;

            var house = await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == houseId)
                .FirstOrDefaultAsync();

            if (house != null && house.RenterId == userId)
            {
                result = true;
            }

            return result;
        }

        public async Task Leave(int houseId)
        {
            var house = await repo.GetByIdAsync<House>(houseId);
            guard.AgainstNull(house, "House cannot be found.");

            house.RenterId = null;

            await repo.SaveChangesAsync();
        }

        public async Task Rent(int houseId, string userId)
        {
            var house = await repo.GetByIdAsync<House>(houseId);

            if (house != null && house.RenterId != null)
            {
                throw new ArgumentException("House is already rented.");
            }

            guard.AgainstNull(house, "House cannot be found.");

            house!.RenterId = userId;

            await repo.SaveChangesAsync();
        }
    }
}
