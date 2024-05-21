using AdminDashboardMVC.Areas.Identity.Pages.Account;
using AdminDashboardMVC.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboardMVC.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        HttpClient client = new HttpClient();
        string Endpoint = "http://localhost:5277/api/User";
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details()
        {
            //id = 21;
            var user = await client.GetFromJsonAsync<Users>($"{Endpoint}/{Areas.Identity.Pages.Account.LoginModel.userId}");
            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with the specified Id is not found
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(int id, dynamic RoleOptions)
        {
            var response = await client.GetFromJsonAsync<Users>($"{Endpoint}/{id}");
            ViewBag.RoleOptions = RoleOptions;
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int roleId, int id, Users user, dynamic RoleOptions)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await client.PutAsJsonAsync($"{Endpoint}/{id}/{roleId}", user);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Details", new { id = user.Id });
            }
            ViewBag.RoleOptions = RoleOptions;
            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{Endpoint}/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Login", "Account"); // Adjust "Login" and "Account" based on your actual login action method and controller
        }

    }
}
