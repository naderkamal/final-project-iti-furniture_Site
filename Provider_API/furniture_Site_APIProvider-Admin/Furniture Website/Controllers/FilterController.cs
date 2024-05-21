using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly AdminDashboardMVC_DBContext _context;

        public FilterController(AdminDashboardMVC_DBContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> filterproductbyid(int id)
        {
            Category category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            List<ProductDTO> productDTOs = category.Products.Select(e => new ProductDTO
                {
                Id = e.Id,
                Weight = e.Weight,
                Stock = e.Stock,
                Price = e.Price,
                Description = e.Description,
                Image = e.Image,
                Name = e.Name,
                CategoryID = e.CategoryId,
            }).ToList();

            return productDTOs;
        }

        [HttpGet("{stratprice:int},{endprice:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> filterproductbyprice(int stratprice,int endprice)
        {
            List<ProductDTO> products = await _context.Products.Where(s=>s.Price> stratprice && s.Price< endprice).Select(e => new ProductDTO
            {
                Id = e.Id,
                Weight = e.Weight,
                Stock = e.Stock,
                Price = e.Price,
                Description = e.Description,
                Image = e.Image,
                Name = e.Name,
                CategoryID = e.CategoryId,

            }).ToListAsync();
            return products;
        }
    }
}
