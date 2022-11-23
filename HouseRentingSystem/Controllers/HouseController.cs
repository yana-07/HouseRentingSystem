using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Extensions;
using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;

        public HouseController(
            IHouseService _houseService,
            IAgentService _agentService)
        {
            houseService = _houseService;
            agentService = _agentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel query)
        {
            var result = await houseService.AllAsync(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = result.TotalHousesCount;
            query.Categories = await houseService.AllCategoriesNamesAsync();
            query.Houses = result.Houses;

            return View(query);
        }

        public async Task<IActionResult> Mine()
        {
            IEnumerable<HouseServiceModel> myHouses;
            var userId = User.Id();

            if (await agentService.ExistsByIdAsync(userId))
            {
                int agentId = await agentService.GetAgentId(userId);
                myHouses = await houseService.AllHousesByAgentId(agentId);
            }
            else
            {
                myHouses = await houseService.AllHousesByUSerId(userId);
            }

            return View(myHouses);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            var model = await houseService.HouseDetailsById(id);

            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            if ((await agentService.ExistsByIdAsync(User.Id())) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            var model = new HouseModel
            {
                Categories = await houseService.AllCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseModel model)
        {
            if ((await agentService.ExistsByIdAsync(User.Id())) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            if ((await houseService.CategoryExistsAsync(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await houseService.AllCategoriesAsync();

                return View(model);
            }

            int agentId = await agentService.GetAgentId(User.Id());

            int id = await houseService.CreateAsync(model, agentId);

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var house = await houseService.HouseDetailsById(id);
            var categoryId = await houseService.GetHouseCategoryId(id);

            var model = new HouseModel
            {
                Id = house.Id,
                Address = house.Address,
                CategoryId = categoryId,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                Title = house.Title,
                Categories = await houseService.AllCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await houseService.Exists(model.Id)) == false)
            {
                ModelState.AddModelError(string.Empty, "House does not exist.");
                model.Categories = await houseService.AllCategoriesAsync();

                return View(model);
            }

            if ((await houseService.HasAgentWithId(model.Id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await houseService.CategoryExistsAsync(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");
                model.Categories = await houseService.AllCategoriesAsync();

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await houseService.AllCategoriesAsync();

                return View(model);
            }

            await houseService.Edit(model.Id, model);

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var house = await houseService.HouseDetailsById(id);

            var model = new HouseDetailsViewModel
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, HouseDetailsViewModel model)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await houseService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if (await agentService.ExistsByIdAsync(User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (await houseService.IsRented(id))
            {
                return RedirectToAction(nameof(All));
            }

            await houseService.Rent(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if ((await houseService.Exists(id)) == false ||
                (await houseService.IsRented(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.IsRentedByUserWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await houseService.Leave(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
