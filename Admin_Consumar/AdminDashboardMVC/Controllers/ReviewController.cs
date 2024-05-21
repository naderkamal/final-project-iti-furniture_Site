
using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Net.Http;

namespace AdminDashboardMVC.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        HttpClient client = new HttpClient();
        string EndPoint = "http://localhost:5277/api/ReviewDetaild";

        [Authorize(Roles = "admin,assistant")]

        public async Task<IActionResult> index(DateTime? dateFilter,int pageNumber = 1, int pageSize = 6)
        {
            List<ReviewDetailsDTO> review = await client.GetFromJsonAsync<List<ReviewDetailsDTO>>(EndPoint);
            review = review.OrderByDescending(s => s.Id).ToList();

            if (dateFilter.HasValue)
            {
                review = review.Where(s => s.Date.HasValue && s.Date.Value.Date == dateFilter.Value.Date).ToList();
            }

            ViewBag.DateFilter = dateFilter;

            int totalItems = review.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            review = review.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Pass pagination information to the view
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(review);
        }

        //public async Task<IActionResult> Index( DateTime? reviewDateFilter, int pageNumber = 1, int pageSize = 1)
        //{
        //    // Save filter values in ViewBag for rendering

        //    ViewBag.reviewDateFilter = reviewDateFilter;

        //    ViewBag.PageNumber = pageNumber;
        //    ViewBag.PageSize = pageSize;

        //    var response = await client.GetFromJsonAsync<List<ReviewDetailsDTO >>(EndPoint);
        //    // Sort shipments by ID in descending order
        //    response = response.OrderByDescending(s => s.Id).ToList();
        //    if (response != null)
        //    {
        //        // Apply filtering based on parameters
        //        var filteredResponse = response.Where(review =>

        //            (!reviewDateFilter.HasValue || DateTime.Parse(review.Date).Date == reviewDateFilter.Value.Date) ).ToList();
        //        int totalItems = filteredResponse.Count;
        //        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        //        ViewBag.TotalPages = totalPages;

        //        filteredResponse = filteredResponse.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        //        return View(filteredResponse);
        //    }
        //    else
        //    {
        //        return View(new List<ReviewDetailsDTO>());
        //    }
        //}

        //Details
        public async Task<IActionResult> Details(int? id)
        {
            ReviewDetailsDTO review = await client.GetFromJsonAsync<ReviewDetailsDTO>(EndPoint + "/" + id.ToString());

            return View("Details", review);
        }


        [Authorize(Roles = "admin")]

        //Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(EndPoint + "/" + id.ToString());

            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

       

    }
}
