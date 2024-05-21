using Furniture_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOrdersController : ControllerBase
    {
        private readonly AdminDashboardMVC_DBContext _context;

        public ProductOrdersController(AdminDashboardMVC_DBContext context)
        {
            _context = context;
        }

        // GET: api/ProductOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductOrderDTO>>> GetProductOrders()
        {
            var productOrders = await _context.ProductOrders
                .Select(po => new ProductOrderDTO
                {
                    OrderId = po.OrderId,
                    ProductId = po.ProductId,
                    ItemPrice = po.ItemPrice,
                    Count = po.Count
                })
                .ToListAsync();

            return productOrders;
        }
        // GET: api/ProductOrder/5
        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<ProductOrderDTO>> GetProductOrder(int orderId)
        {
            var productOrderDto = await  _context.ProductOrders.Where(o=> o.OrderId==orderId).Select(o=> new ProductOrderDTO
            {
                OrderId=o.OrderId,
                ProductId=o.ProductId,
                ItemPrice=o.ItemPrice,
                Count=o.Count

            }).ToListAsync();

            if (productOrderDto == null)
            {
                return NotFound();
            }


            return Ok(productOrderDto);
        }

        // GET: api/ProductOrder/5/10
        [HttpGet("{orderId:int}/{productId:int}")]
        public async Task<ActionResult<ProductOrderDTO>> GetProductOrder(int orderId, int productId)
        {
            var productOrder = await _context.ProductOrders.FindAsync(orderId, productId);

            if (productOrder == null)
            {
                return NotFound();
            }

            var productOrderDto = new ProductOrderDTO
            {
                OrderId = productOrder.OrderId,
                ProductId = productOrder.ProductId,
                ItemPrice = productOrder.ItemPrice,
                Count = productOrder.Count
            };

            return Ok(productOrderDto);
        }
        // POST: api/ProductOrder
        [HttpPost]
        public async Task<ActionResult<ProductOrderDTO>> PostProductOrder(ProductOrderDTO productOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productOrder = new ProductOrder
            {
                OrderId = productOrderDto.OrderId,
                ProductId = productOrderDto.ProductId,
                ItemPrice = productOrderDto.ItemPrice,
                Count = productOrderDto.Count
            };

            _context.ProductOrders.Add(productOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductOrder), new { orderId = productOrder.OrderId, productId = productOrder.ProductId }, productOrderDto);
        }



        // PUT: api/ProductOrder/5/10
        [HttpPut("{orderId}/{productId}")]
        public async Task<IActionResult> PutProductOrder(int orderId, int productId, ProductOrderDTO productOrderDto)
        {
            if (orderId != productOrderDto.OrderId || productId != productOrderDto.ProductId)
            {
                return BadRequest();
            }

            var productOrder = await _context.ProductOrders.FindAsync(orderId, productId);

            if (productOrder == null)
            {
                return NotFound();
            }

            productOrder.ItemPrice = productOrderDto.ItemPrice;
            productOrder.Count = productOrderDto.Count;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductOrderExists(orderId, productId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/ProductOrder/5/10
        [HttpDelete("{orderId:int}/{productId:int}")]
        public async Task<IActionResult> DeleteProductOrder(int orderId, int productId)
        {
            var productOrder = await _context.ProductOrders.FindAsync(orderId, productId);
            if (productOrder == null)
            {
                return NotFound();
            }

            _context.ProductOrders.Remove(productOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ProductOrder/5/10 // to delete all row with product id 5
        [HttpDelete("{product}/{productId:int}")]
        public async Task<IActionResult> DeleteOrderProducts(string product, int productId)
        {
            var productOrder = await _context.ProductOrders.Where(o => o.ProductId == productId).ToListAsync();
            if (productOrder == null)
            {
                return NotFound();
            }

            _context.ProductOrders.RemoveRange(productOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ProductOrder/5 // to delete all row with order id 5
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteProductOrder(int orderId)
        {
            var productOrder = await _context.ProductOrders.Where(o => o.OrderId == orderId).ToListAsync();
            if (productOrder == null)
            {
                return NotFound();
            }

            _context.ProductOrders.RemoveRange(productOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductOrderExists(int orderId, int productId)
        {
            return _context.ProductOrders.Any(po => po.OrderId == orderId && po.ProductId == productId);
        }
    }
}
