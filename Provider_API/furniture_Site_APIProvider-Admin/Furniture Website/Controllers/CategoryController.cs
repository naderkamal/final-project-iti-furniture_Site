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
    public class CategoryController : ControllerBase
    {
        private readonly AdminDashboardMVC_DBContext _context;

        public CategoryController(AdminDashboardMVC_DBContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            List<CategoryDTO> categories = await _context.Categories.Select(e => new CategoryDTO
            {
                Id = e.Id,
                Image = e.Image,
                Description = e.Description,
                Name = e.Name,

            }).ToListAsync();
            return categories;
        }



        // GET: api/Categories/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            Category category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            CategoryDTO categoryDTO = new CategoryDTO()
            {
                Id = category.Id,
                Image = category.Image,
                Description = category.Description,
                Name = category.Name,
            };
            return categoryDTO;
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryByName(string name)
        {
            Category category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Name == name);

            if (category == null)
            {
                return NotFound();
            }

            List<ProductDTO> productDTOs = category.Products
                .Take(4) // Limit to four products
                .Select(product => new ProductDTO
                {
                    Id = product.Id,
                    Weight = product.Weight,
                    Stock = product.Stock,
                    Price = product.Price,
                    Description = product.Description,
                    Image = product.Image,
                    Name = product.Name,
                    CategoryName = product.Category.Name,
                    CategoryID = product.CategoryId,
                }).ToList();

            CategoryDTO categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Image = category.Image,
                Description = category.Description,
                Name = category.Name,
                Products = productDTOs
            };

            return categoryDTO;
        }


        // Post: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category()
                {
                    Image = categoryDTO.Image,
                    Description = categoryDTO.Description,
                    Name = categoryDTO.Name,
                };
                _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // Put: api/Categories/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutCategory(CategoryDTO categoryDTO, int id)
        {
            if (ModelState.IsValid)
            {
                Category category= await _context.Categories.FindAsync(id);
                if (category == null) 
                { 
                    return NotFound();
                }
                else
                {
                    category.Image = categoryDTO.Image;
                    category.Description = categoryDTO.Description;
                    category.Name = categoryDTO.Name;
                    await _context.SaveChangesAsync();
                    return StatusCode(204, new { massage = "updated", _category = categoryDTO });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Delete: api/Categories/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            Category category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                /*List<Product> deleteProducts = await _context.Products.Where(e => e.CategoryId == id).ToListAsync();
                if (deleteProducts!=null)
                {
                _context.Products.RemoveRange(deleteProducts);
                    await _context.SaveChangesAsync();
                }*/

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message=$"category with id = {category.Id} is deleted",
                    _category = category,
                    categorylist= await _context.Categories.ToListAsync()
                });
            }
        }
    }
}
