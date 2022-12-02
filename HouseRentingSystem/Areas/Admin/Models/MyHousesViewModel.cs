using HouseRentingSystem.Core.Models.House;

namespace HouseRentingSystem.Areas.Admin.Models
{
    public class MyHousesViewModel
    {
        public IEnumerable<HouseServiceModel> AddedHouses { get; set; } 
            = Enumerable.Empty<HouseServiceModel>();

        public IEnumerable<HouseServiceModel> RentedHouses { get; set; }
            = Enumerable.Empty<HouseServiceModel>();
    }
}
