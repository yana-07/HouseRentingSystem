using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Core.Services;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseDetailsModel : HouseServiceModel
    {
        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public AgentServiceModel Agent { get; set; }
    }
}
