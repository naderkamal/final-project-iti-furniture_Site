using admain.partial_operation;
using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace admain.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        HttpClient client=new HttpClient();
        string endPoint = "http://localhost:5277/api/Product";
        string endPoint2 = "http://localhost:5277/api/Category";
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "admin,assistant,shipman")]

        [HttpGet]
        public async Task<IActionResult> Index(int? idFilter, string? nameFilter, int? idCategoryFilter, int pageNumber = 1, int pageSize = 10)
        {
            List<Product> products = await client.GetFromJsonAsync<List<Product>>(endPoint);
            products = products.OrderByDescending(e => e.Id).ToList();

            // Apply filters
            if (idFilter.HasValue)
            {
                products = products.Where(e => e.Id == idFilter).ToList();
            }

            if (!string.IsNullOrEmpty(nameFilter))
            {
                products = products.Where(e => e.Name == nameFilter).ToList();
            }

            if (idCategoryFilter.HasValue)
            {
                products = products.Where(e => e.CategoryID == idCategoryFilter).ToList();
            }

            int totalItems = products.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            products = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.IdFilter = idFilter;
            ViewBag.NameFilter = nameFilter;
            ViewBag.IdCategoryFilter= idCategoryFilter;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(products);
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
        //
        ////////////////////
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> Create()
        {
            ViewData["CategoryID"] = new SelectList(await client.GetFromJsonAsync<List<Category>>(endPoint2), "Id", "Name");
            return View("Create", new ProductViewModel());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Create(ProductViewModel productviewModel)
        {
            string stringfilename = uploadfile(productviewModel.Image);
            var product = convertToProduct.convert(productviewModel, stringfilename);
            HttpResponseMessage response = await client.PostAsJsonAsync<Product>(endPoint, product);
            ViewBag.location = response.Headers.Location;
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["CategoryID"] = new SelectList(await client.GetFromJsonAsync<List<Category>>(endPoint2), "Id", "Name");
                return View("Create", new ProductViewModel());
            }
        }

        ////////////////////
        [Authorize(Roles = "admin,assistant")]
        public async Task<IActionResult> Edit(int? id)
        {
            Product product = await client.GetFromJsonAsync<Product>(endPoint + "/" + id.ToString());
            var productviewModel = convertToProductviewmodel.convert(product, null);

            ViewData["CategoryID"] = new SelectList(await client.GetFromJsonAsync<List<Category>>(endPoint2), "Id", "Name");
            return View("Edit", productviewModel);
        }
        [Authorize(Roles = "admin,assistant")]
        [HttpPost]
        public async Task<ActionResult> Edit(ProductViewModel productviewModel)
        {
            string stringfilename = uploadfile(productviewModel.Image);
            var product = convertToProduct.convert(productviewModel, stringfilename);
            HttpResponseMessage response = await client.PutAsJsonAsync<Product>(endPoint+"/"+ product.Id, product);
            ViewBag.location = response.Headers.Location;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["CategoryID"] = new SelectList(await client.GetFromJsonAsync<List<Category>>(endPoint2), "Id", "Name");
                return View("Edit", productviewModel);
            }
        }

        ///////////////////////
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(endPoint + "/" + id.ToString());

            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

        ///////////////////////
        [Authorize(Roles = "admin,assistant,shipman")]

        public async Task<IActionResult> Details(int? id)
        {
            Product product = await client.GetFromJsonAsync<Product>(endPoint + "/" + id.ToString());

            return View("Details", product);
        }
    }
}
