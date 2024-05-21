using AdminDashboardMVC.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboardMVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint = "http://localhost:5277/api/Order";
        public OrderController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Order/Index
        [Authorize(Roles = "admin,assistant,shipman")]
        public async Task<IActionResult> Index(int? idFilter, string statusFilter, DateTime? orderDateFilter, int? userIdFilter, int? shipmentIdFilter, string phoneNumberFilter, decimal? totalPriceFilter, int pageNumber = 1, int pageSize = 10)
        {
            // Save filter values in ViewBag for rendering
            ViewBag.IdFilter = idFilter;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.OrderDateFilter = orderDateFilter;
            ViewBag.UserIdFilter = userIdFilter;
            ViewBag.ShipmentIdFilter = shipmentIdFilter;
            ViewBag.PhoneNumberFilter = phoneNumberFilter;
            ViewBag.TotalPriceFilter = totalPriceFilter;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            var response = await _httpClient.GetFromJsonAsync<List<OrderDTO>>(_endpoint);
            // Sort shipments by ID in descending order
            response = response.OrderByDescending(s => s.Id).ToList();
            if (response != null)
            {
                // Apply filtering based on parameters
                var filteredResponse = response.Where(order =>
                    (!idFilter.HasValue || order.Id == idFilter) &&
                    (string.IsNullOrEmpty(statusFilter) || order.Statue == statusFilter) &&
                    (!orderDateFilter.HasValue || order.OrderDate?.Date == orderDateFilter.Value.Date) &&
                    (!userIdFilter.HasValue || order.UserID == userIdFilter) &&
                    (!shipmentIdFilter.HasValue || order.ShipmentID == shipmentIdFilter) &&
                     (string.IsNullOrEmpty(phoneNumberFilter) || (order.PhoneNumber != null && order.PhoneNumber.Contains(phoneNumberFilter))) &&
                    (!totalPriceFilter.HasValue || order.TotalPrice == totalPriceFilter)
                // Add similar conditions for other filters
                ).ToList();
                int totalItems = filteredResponse.Count;
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                ViewBag.TotalPages = totalPages;

                filteredResponse = filteredResponse.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return View(filteredResponse);
            }
            else
            {
                return View(new List<OrderDTO>());
            }
        }

        [Authorize(Roles = "admin ,assistant ,shipman")]
        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            OrderDTO response;
            try
            {
                response = await _httpClient.GetFromJsonAsync<OrderDTO>(_endpoint + "/" + id);
            }
            catch
            {
                return NotFound();
            }

            if (response != null)
            {
                return View(response);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDTO orderDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(_endpoint, orderDTO);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(orderDTO);
            }
        }
        [Authorize(Roles = "admin,assistant")]
        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<OrderDTO>($"{_endpoint}/{id}");
            if (response != null)
            {
                return View(response);
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "admin,assistant")]
        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDTO orderDTO)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View(orderDTO);
            }
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{id}", orderDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(orderDTO);
                }
            }
            catch
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin,assistant")]
        // POST: Order/Accept/5
        public async Task<IActionResult> Accept(int id)
        {
            OrderDTO orderDTO;
            // Check if the model is valid
            try
            {
                orderDTO = await _httpClient.GetFromJsonAsync<OrderDTO>($"{_endpoint}/{id}");
            }
            catch
            {
                return NotFound();
            }
            orderDTO.Statue = "inProgress";

            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{id}", orderDTO);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
            return RedirectToAction(nameof(Index));



        }

    }

}
