using HouseRentingSystem.Core.Constants;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentService agentService;

        public AgentController(IAgentService _agentService)
        {
            this.agentService = _agentService;
        }

        public async Task<IActionResult> Become()
        {
            if (await agentService.ExistsByIdAsync(User.Id()))
            {
                TempData[MessageConstants.ErrorMessage] = "Вие вече сте агент";

                return RedirectToAction("Index", "Home");
            }

            var model = new BecomeAgentModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeAgentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Id();

            if (await agentService.ExistsByIdAsync(userId))
            {
                TempData[MessageConstants.ErrorMessage] = "Вие вече сте агент";

                return RedirectToAction("Index", "Home");
            }

            if (await agentService.UserWithPhoneNumerExistsAsync(model.PhoneNumber))
            {
                TempData[MessageConstants.ErrorMessage] = "Телефонът вече съществува.";

                return RedirectToAction("Index", "Home");
            }

            if (await agentService.UserHasRentsAsync(userId))
            {
                TempData[MessageConstants.ErrorMessage] = "За да станете агент, не трябва да имате наеми.";

                return RedirectToAction("Index", "Home");
            }

            await agentService.CreateAsync(userId, model.PhoneNumber);

            return RedirectToAction("All", "House");
        }
    }
}
