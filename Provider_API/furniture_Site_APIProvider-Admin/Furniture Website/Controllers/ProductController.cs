using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AdminDashboardMVC_DBContext _context;

        public ProductController(AdminDashboardMVC_DBContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct()
        {
            List<ProductDTO> products = await _context.Products.Select(e => new ProductDTO
            {
                Id = e.Id,
                Weight=e.Weight,
                Stock=e.Stock,
                Price=e.Price,
                Description = e.Description,
                Image = e.Image,
                Name = e.Name,
                CategoryID = e.CategoryId,
                AverageRating = e.ProductUsers.Average(pu => pu.RatingStar) ?? 0

            }).ToListAsync();
            return products;
        }

        //public class custom {
        //  public int ProductId;
        //   public double AverageRating;
        //}
        //[HttpGet("{count}")]
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetFourProductsByCategory(string count)
        //{
        //    List<custom> RelatedProducts = await _context.ProductUsers.Where(p=>p.Product.CategoryID==int.Parse(count))
        //        .GroupBy(pu => pu.ProductId)
        //        .Select(g => new custom
        //        {
        //            ProductId = g.Key,
        //            AverageRating = g.Average(pu => pu.RatingStar).Value // Calculate average rating
        //        })
        //        .OrderByDescending(p => p.AverageRating) // Order by average rating
        //        .Take(4)
        //        .ToListAsync();
        //    if (RelatedProducts.Count < 4) {
        //        RelatedProducts.AddRange(await _context.Products.Where(p => p.CategoryID == int.Parse(count))
        //        .Select(g => new custom
        //        {
        //            ProductId = g.Id,
        //            AverageRating =0
        //        })
        //        .OrderByDescending(p => p.AverageRating) // Order by average rating
        //        .Take(4-RelatedProducts.Count)
        //        .ToListAsync());
        //    }
        //    var products = new List<ProductDTO>();
            

        //    foreach (var productRating in RelatedProducts)
        //    {
        //        var product = await _context.Products.FindAsync(productRating.ProductId);
        //        if (product != null)
        //        {
        //            var productDTO = new ProductDTO()
        //            {
        //                Id = product.Id,
        //                Weight = product.Weight,
        //                Stock = product.Stock,
        //                Price = product.Price,
        //                Description = product.Description,
        //                Image = product.Image,
        //                Name = product.Name,
        //                CategoryID = product.CategoryID,
        //                AverageRating = productRating.AverageRating
        //            };
        //            products.Add(productDTO);
        //        }
        //    }

        //    return Ok(products);
        //}

        // GET: api/Product/5

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            Product product = await _context.Products.Include("Category").FirstOrDefaultAsync(p=>p.Id==id);

            if (product == null)
            {
                return NotFound();
            }
            ProductDTO productDTO = new ProductDTO()
            {
                Id = product.Id,
                Weight = product.Weight,
                Stock = product.Stock,
                Price = product.Price,
                Description = product.Description,
                Image = product.Image,
                Name = product.Name,
                CategoryName = product.Category.Name,
                CategoryID=product.CategoryId,
                AverageRating= product.ProductUsers.Average(pu => pu.RatingStar)?? 0
            };
            return productDTO;
        }

        // Post: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDTO)
        {
            if (!_context.Categories.Any(e => e.Id == productDTO.CategoryID))
            {
                return BadRequest("category does not exist.");
            }
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Weight = productDTO.Weight,
                    Stock = productDTO.Stock,
                    Price = productDTO.Price,
                    Description = productDTO.Description,
                    Image = productDTO.Image,
                    Name = productDTO.Name,
                    CategoryId = productDTO.CategoryID,
                };
                _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // Put: api/Product/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutProduct(ProductDTO productDTO, int id)
        {
            if (!_context.Categories.Any(e => e.Id == productDTO.CategoryID))
            {
                return BadRequest("category does not exist.");
            }
            if (ModelState.IsValid)
            {
                Product product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    product.Weight = productDTO.Weight;
                    product.Stock = productDTO.Stock;
                    product.Price = productDTO.Price;
                    product.Description = productDTO.Description;
                    product.Image = productDTO.Image;
                    product.Name = productDTO.Name;
                    product.CategoryId = productDTO.CategoryID;
                    await _context.SaveChangesAsync();
                    return StatusCode(204, new { massage = "updated", _product = productDTO });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Delete: api/Product/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> PutProduct(int id)
        {
            Product product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                /*List<ProductUser> deleteProductUser = await _context.ProductUsers.Where(e => e.ProductId == id).ToListAsync();
                List<ProductOrder> deleteProductOrder = await _context.ProductOrders.Where(e => e.ProductId == id).ToListAsync();
                if (deleteProductUser != null && deleteProductOrder != null)
                {
                    _context.ProductUsers.RemoveRange(deleteProductUser);
                    await _context.SaveChangesAsync();
                    _context.ProductOrders.RemoveRange(deleteProductOrder);
                    await _context.SaveChangesAsync();
                }
                else if(deleteProductUser != null && deleteProductOrder==null)
                {
                    _context.ProductUsers.RemoveRange(deleteProductUser);
                    await _context.SaveChangesAsync();
                }
                else if(deleteProductUser == null && deleteProductOrder != null)
                {
                    _context.ProductOrders.RemoveRange(deleteProductOrder);
                    await _context.SaveChangesAsync();
                }*/

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"product with id = {product.Id} is deleted",
                    _product = product,
                    productlist = _context.Products.ToList()
                });
            }
        }
    }
}
