using admain.partial_operation;
using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admain.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        HttpClient client = new HttpClient();
        string endPoint = "http://localhost:5277/api/Category";
        private readonly IWebHostEnvironment webHostEnvironment;
        public CategoryController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "admin,assistant,shipman")]

        public async Task<IActionResult> Index(int? idFilter, string? nameFilter, int pageNumber = 1, int pageSize = 10)
        {
            List<Category> category = await client.GetFromJsonAsync<List<Category>>(endPoint);
            category = category.OrderByDescending(e=>e.Id).ToList();

            // Apply filters
            if (idFilter.HasValue)
            {
                category = category.Where(e=>e.Id== idFilter).ToList();
            }

            if (!string.IsNullOrEmpty(nameFilter))
            {
                category = category.Where(e => e.Name == nameFilter).ToList();
            }

            int totalItems = category.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            category = category.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.IdFilter = idFilter;
            ViewBag.NameFilter = nameFilter;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(category);
        }

        ////////////////////
        //upload photo
        private string uploadfile(IFormFile uploadimage)
        {
            string filename = null;
            if (uploadimage != null)
            {
                string direction = Path.Combine(webHostEnvironment.WebRootPath, "images");
                filename = Guid.NewGuid().ToString() + "_" + uploadimage.FileName;
                string filepath = Path.Combine(direction, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    uploadimage.CopyTo(filestream);
                }
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string imageUrl = $"{baseUrl}/images/{filename}";
                return imageUrl;
            }
            return filename;
        }
        [Authorize(Roles = "admin")]

        ///////////////////////
        public async Task<ActionResult> Create()
        {
            return View("Create", new CategoryViewModel());
        }
        [Authorize(Roles = "admin")]

        [HttpPost]
        public async Task<ActionResult> Create(CategoryViewModel categoryViewModel)
        {
            string stringfilename = uploadfile(categoryViewModel.Image);
            var category = convertToCategory.convert(categoryViewModel, stringfilename);
            HttpResponseMessage response = await client.PostAsJsonAsync<Category>(endPoint, category);
            ViewBag.location = response.Headers.Location;
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return RedirectToAction("Index");

            }
            else
            {
                return View("Create", new CategoryViewModel());
            }
        }
        [Authorize(Roles = "admin,assistant")]

        ///////////////////////
        public async Task<IActionResult> Edit(int? id)
        {
            Category category = await client.GetFromJsonAsync<Category>(endPoint + "/" + id.ToString());
            var categoryViewModel = convertToCategoryviewmodel.convert(category, null);

            return View("Edit", categoryViewModel);
        }

        [Authorize(Roles = "admin,assistant")]

        [HttpPost]
        public async Task<ActionResult> Edit(CategoryViewModel categoryViewModel)
        {
            string stringfilename = uploadfile(categoryViewModel.Image);
            var category = convertToCategory.convert(categoryViewModel, stringfilename);
            HttpResponseMessage response = await client.PutAsJsonAsync<Category>(endPoint + "/" + category.Id, category);
            ViewBag.location = response.Headers.Location;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            else
            {
                return View("Edit", categoryViewModel);
            }
        }
        [Authorize(Roles = "admin")]

        ///////////////////////
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(endPoint + "/" + id.ToString());

            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin,assistant,shipman")]

        ///////////////////////
        public async Task<IActionResult> Details(int? id)
        {
            Category category = await client.GetFromJsonAsync<Category>(endPoint + "/" + id.ToString());

            return View("Details", category);
        }
    }
}
