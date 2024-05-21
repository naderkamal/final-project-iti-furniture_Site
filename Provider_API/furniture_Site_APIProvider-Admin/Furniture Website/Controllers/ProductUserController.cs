using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductUserController : ControllerBase
    {
        private readonly AdminDashboardMVC_DBContext _context;

        public ProductUserController(AdminDashboardMVC_DBContext context)
        {
            _context = context;
        }

        // GET: api/ProductUsers
        [HttpGet("{count:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetTopRatedProducts()
        {
            var topRatedProducts = await _context.ProductUsers
                .GroupBy(pu => pu.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    AverageRating = g.Average(pu => pu.RatingStar) // Calculate average rating
                })
                .OrderByDescending(p => p.AverageRating) // Order by average rating
                .Take(4) 
                .ToListAsync();

            if (topRatedProducts.Count < 4)
            {
                topRatedProducts.AddRange(_context.Products.OrderByDescending(p=>p.Price).Take(4- topRatedProducts.Count).Select(p=> new
                {
                    ProductId = p.Id,
                    AverageRating = p.ProductUsers.Average(pu => pu.RatingStar) // Calculate average rating
                }).ToList());

            }

            var products = new List<ProductDTO>();

            foreach (var productRating in topRatedProducts)
            {
                var product = await _context.Products.FindAsync(productRating.ProductId);
                if (product != null)
                {
                    var productDTO = new ProductDTO()
                    {
                        Id = product.Id,
                        Weight = product.Weight,
                        Stock = product.Stock,
                        Price = product.Price,
                        Description = product.Description,
                        Image = product.Image,
                        Name = product.Name,
                        CategoryID = product.CategoryId,
                        AverageRating = productRating.AverageRating??0
                    };
                    products.Add(productDTO);
                }
            }

            return Ok(products);
        }


        // GET: api/ProductUser/5/10
        [HttpGet("{userId}/{productId}")]
        public async Task<IActionResult> Get(int userId, int productId)
        {
            try
            {
                var productUser = await _context.ProductUsers.FindAsync(userId, productId);

                if (productUser == null)
                {
                    return NotFound();
                }

                return Ok(productUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/ProductUser
        [HttpPost]
        public async Task<IActionResult> Post(ProductUserDTO productUserDto)
        {
            try
            {
                var productUser = new ProductUser
                {
                    UserId = productUserDto.UserId,
                    ProductId = productUserDto.ProductId,
                    RatingStar = productUserDto.RatingStar
                };

                _context.ProductUsers.Add(productUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new { userId = productUser.UserId, productId = productUser.ProductId }, productUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/ProductUser/5/10
        [HttpPut("{userId}/{productId}")]
        public async Task<IActionResult> Put(int userId, int productId, ProductUserDTO productUserDto)
        {
            try
            {
                var productUser = await _context.ProductUsers.FindAsync(userId, productId);

                if (productUser == null)
                {
                    return NotFound();
                }

                productUser.RatingStar = productUserDto.RatingStar;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/ProductUser/5/10
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> Delete(int userId, int productId)
        {
            try
            {
                var productUser = await _context.ProductUsers.FindAsync(userId, productId);

                if (productUser == null)
                {
                    return NotFound();
                }

                _context.ProductUsers.Remove(productUser);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
