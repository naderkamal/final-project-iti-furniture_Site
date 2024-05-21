using AdminDashboardMVC.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboardMVC.Controllers
{
    [Authorize]
    public class ShipmentController : Controller
    {
        HttpClient _client = new HttpClient();
        private readonly string _endpoint = "http://localhost:5277/api/Shipment";

        // GET: ShipmentController1
        [Authorize(Roles = "admin,assistant,shipman")]

        public async Task<IActionResult> Index(int? idFilter, bool? isCompletedFilter, DateTime? dateFilter, decimal? costFilter, string addressFilter, int pageNumber = 1, int pageSize = 10)
        {
            var shipments = await _client.GetFromJsonAsync<List<ShipmentDTO>>(_endpoint);

            // Sort shipments by ID in descending order
            shipments = shipments.OrderByDescending(s => s.Id).ToList();

            // Apply filters
            if (idFilter.HasValue)
            {
                shipments = shipments.Where(s => s.Id == idFilter).ToList();
            }

            if (isCompletedFilter.HasValue)
            {
                shipments = shipments.Where(s => s.IsCompleted == isCompletedFilter.Value).ToList();
            }

            if (dateFilter.HasValue)
            {
                shipments = shipments.Where(s => s.Date.HasValue && s.Date.Value.Date == dateFilter.Value.Date).ToList();
            }

            if (costFilter.HasValue)
            {
                shipments = shipments.Where(s => s.Cost.HasValue && s.Cost.Value == costFilter).ToList();
            }
            if (!string.IsNullOrEmpty(addressFilter))
            {
                shipments = shipments.Where(s => s.Address != null && s.Address.Contains(addressFilter)).ToList();
            }


            int totalItems = shipments.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            shipments = shipments.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            // Store filters in ViewBag to preserve values in the view
            ViewBag.IdFilter = idFilter;
            ViewBag.IsCompletedFilter = isCompletedFilter;
            ViewBag.DateFilter = dateFilter;
            ViewBag.CostFilter = costFilter;
            ViewBag.AddressFilter = addressFilter;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(shipments);
        }

        // GET: ShipmentMvc/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _client.GetFromJsonAsync<ShipmentDTO>(_endpoint + "/" + id);
            return View(response);
        }

        // GET: ShipmentController1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShipmentController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync(_endpoint, shipmentDTO);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(shipmentDTO);
        }

        // GET: ShipmentController1/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetFromJsonAsync<ShipmentDTO>($"{_endpoint}/{id}");
            return View(response);
        }

        // POST: ShipmentController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShipmentDTO shipmentDTO)
        {
            if (id != shipmentDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _client.PutAsJsonAsync($"{_endpoint}/{id}", shipmentDTO);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(shipmentDTO);
        }

        // GET: ShipmentController1/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.GetFromJsonAsync<ShipmentDTO>($"{_endpoint}/{id}");
            return View(response);
        }

        // POST: ShipmentController1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _client.DeleteAsync($"{_endpoint}/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }

    }
}
