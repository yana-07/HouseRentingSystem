using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services
{
    public class AgentService : IAgentService
    {
        private readonly IRepository repo;

        public AgentService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task CreateAsync(string userId, string phoneNumber)
        {
            var agent = new Agent
            {
                UserId = userId,
                PhoneNumber = phoneNumber,
            };

            await repo.AddAsync(agent);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(string userId)
        {
            return await repo.AllReadonly<Agent>()
                .AnyAsync(a => a.UserId == userId);
        }

        public async Task<bool> UserHasRentsAsync(string userId)
        {
            return await repo.AllReadonly<House>()
                .AnyAsync(h => h.RenterId == userId);             
        }

        public async Task<bool> UserWithPhoneNumerExistsAsync(string phoneNumber)
        {
            return await repo.AllReadonly<Agent>()
                .AnyAsync(a => a.PhoneNumber == phoneNumber);
        }
    }
}
