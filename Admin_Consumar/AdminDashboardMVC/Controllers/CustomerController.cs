using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboardMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class CustomerController : Controller
    {
        HttpClient client = new HttpClient();
        string Endpoint = "http://localhost:5277/api/User";

       

        public async Task<IActionResult> Index(string cityFilter, string nameFilter, string phoneFilter, string? roleFilter, int pageNumber = 1, int pageSize = 10)
        {
            var customers = await client.GetFromJsonAsync<List<Users>>(Endpoint);
            customers = customers.OrderByDescending(s => s.Id).ToList();
            //var customers = users.Where(u => u.Role == "customer").ToList();
            // Apply filters
            if (!string.IsNullOrEmpty(cityFilter))
            {
                customers = customers.Where(s => s.City != null && s.City.Contains(cityFilter)).ToList();
            }
            if (!string.IsNullOrEmpty(nameFilter))
            {
                customers = customers.Where(s =>
                (s.FirstName != null && s.FirstName.Contains(nameFilter)) ||
                (s.LastName != null && s.LastName.Contains(nameFilter))).ToList();
            }
            if (!string.IsNullOrEmpty(phoneFilter))
            {
                customers = customers.Where(s => s.PhoneNumber != null && s.PhoneNumber.Contains(phoneFilter)).ToList();
            }
            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (roleFilter == "customer")
                {
                    // Filter for customers with a null role
                    customers = customers.Where(s => string.IsNullOrEmpty(s.Role)).ToList();
                }
                else
                {
                    customers = customers.Where(s => s.Role != null && s.Role.Contains(roleFilter)).ToList();
                }
            }
            // Store filters in ViewBag to preserve values in the view
            ViewBag.CityFilter = cityFilter;
            ViewBag.NameFilter = nameFilter;
            ViewBag.PhoneFilter = phoneFilter;
            ViewBag.RoleFilter = roleFilter;

            int totalItems = customers.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            customers = customers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Pass pagination information to the view
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            //return
            return View(customers);
        }


      
        public IActionResult Create(dynamic RoleOptions)
        {
            ViewBag.RoleOptions = RoleOptions;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Users user,int roleId , dynamic RoleOptions)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync($"{Endpoint}/{roleId}", user);
                response.EnsureSuccessStatusCode();
                TempData["CreateUser"] = $"{user.FirstName} {user.LastName}";
                return RedirectToAction("Index");
            }
            // If model state is not valid, reload the view with the form data and validation errors
            ViewBag.RoleOptions = RoleOptions;
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
        public async Task<IActionResult> Edit(int roleId,int id, Users user, dynamic RoleOptions)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await client.PutAsJsonAsync($"{Endpoint}/{id}/{roleId}", user);
                response.EnsureSuccessStatusCode();
                TempData["UpdateUser"] = $"{user.FirstName} {user.LastName}";
                return RedirectToAction("Index");
            }
            ViewBag.RoleOptions = RoleOptions;
            return View(user);

        }


        public async Task<IActionResult> Details(int id)
        {
            var user = await client.GetFromJsonAsync<Users>($"{Endpoint}/{id}");
            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with the specified Id is not found
            }

            return View(user);
        }



        //Delete
        //[HttpPost]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    HttpResponseMessage response = await client.DeleteAsync(Endpoint + "/" + id.ToString());

        //    response.EnsureSuccessStatusCode();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{Endpoint}/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
    }
}
