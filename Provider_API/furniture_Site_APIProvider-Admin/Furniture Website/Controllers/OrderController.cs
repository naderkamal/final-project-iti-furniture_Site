using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private AdminDashboardMVC_DBContext context { get; set; }

        public OrderController(AdminDashboardMVC_DBContext context)
        {
            this.context = context;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<OrderDTO> orders = await context.Orders.Select(s => new OrderDTO
            {
                 Id = s.Id,
                OrderDate=s.OrderDate,
                Statue=s.Statue,
                TotalPrice=s.TotalPrice,
                ShipmentID=s.ShipmentId,
                UserID = s.UserId,
              PhoneNumber=s.PhoneNumber
            }).ToListAsync();
            foreach(var order in orders)
            {
                Order o = await context.Orders.FindAsync(order.Id);
                order.OrderProducts_IDs = o.ProductOrders.Select(x=>x.ProductId).ToList();
            }
           
            if (orders == null) { return NotFound(" There is no data in orders"); }
            return Ok(orders);
        }



        // GET api/<OrdersController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetOne(int id)
        {
            if (!context.Orders.Any(o => o.Id == id))
            {
                return NotFound("Wrong Id, There is no order with this id");
            }
            var order = await context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Wrong Id, There is no order with this id ");
            }
            else
            {
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Statue = order.Statue,
                    TotalPrice = order.TotalPrice,
                    ShipmentID = order.ShipmentId,
                    UserID = order.UserId,
                    PhoneNumber = order.PhoneNumber,
                    OrderProducts_IDs = order.ProductOrders.Select(x => x.ProductId).ToList()
                };

                return Ok(orderDTO);
            }

        }



        // GET api/<OrdersController>/userId/5
        [HttpGet("{user}/{UserId}")]
        public async Task< ActionResult> GetByUserId(string UserId)
        {
            var ordersDTO = await context.Orders.Where(o=>o.UserId==int.Parse(UserId)).Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Statue = order.Statue,
                TotalPrice = order.TotalPrice,
                ShipmentID = order.ShipmentId,
                UserID = order.UserId,
                PhoneNumber = order.PhoneNumber,
                OrderProducts_IDs = order.ProductOrders.Select(x => x.ProductId).ToList()
            }).ToListAsync();
            if (ordersDTO == null)
            { 
                return NotFound("Wrong Id, There is no order with this id "); 
            } 
            
               
                return Ok(ordersDTO);
           
            
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult> Post(OrderDTO orderDTO)
        {
            if (!await context.AspNetUsers.AnyAsync(u => u.Id == orderDTO.UserID))
                return BadRequest("Wrong User Id !!");
            if (!await context.Shipments.AnyAsync(u => u.Id == orderDTO.ShipmentID))
                return BadRequest("Wrong Shipment Id !! ");

            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    Id = orderDTO.Id,
                    OrderDate = orderDTO.OrderDate,
                    Statue = orderDTO.Statue,
                    TotalPrice = orderDTO.TotalPrice,
                    ShipmentId = orderDTO.ShipmentID,
                    UserId = orderDTO.UserID,
                    PhoneNumber=orderDTO.PhoneNumber
                };
                await context.Orders.AddAsync(order);
                try
                {
                   await context.SaveChangesAsync();
                }catch(Exception ex)
                {
                    return BadRequest("Message : "+ex.Message + " | => InnerException : " + ex.InnerException);
                }

                string newUrl = Url.Action(nameof(GetOne), new { id = order.Id });

                return Created(newUrl, order);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/<OrdersController>/5

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] OrderDTO orderDTO)
        {
            
            if (ModelState.IsValid)
            {
                Order order =await context.Orders.FindAsync(id);
                if (order != null)
                {
                    order.OrderDate = orderDTO.OrderDate;
                    order.Statue = orderDTO.Statue;
                    order.TotalPrice = orderDTO.TotalPrice;
                    order.ShipmentId = orderDTO.ShipmentID;
                    order.UserId = orderDTO.UserID;
                    order.PhoneNumber = orderDTO.PhoneNumber;
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
                    }

                }
                return StatusCode(204, new { massage = " successful update", New_Order = order });

            } else { return BadRequest(); }
        }

        //// PUT api/<OrdersController>/5

        //[HttpPut("{accept:alpha}/{id}")]
        //public async Task<ActionResult> put([FromRoute] int id)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        Order order = await context.Orders.FindAsync(id);
        //        if (order != null)
        //        {
                  
        //            order.Statue = "inProgress";
                   
        //            try
        //            {
        //                await context.SaveChangesAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
        //            }

        //        }
        //        return StatusCode(204, new { massage = " successful update", New_Order = order });

        //    }
        //    else { return BadRequest(); }
        //}


        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
          
            Order order = await context.Orders.FindAsync(id);
            if (order != null)
            {

                try
                {
                    var x = await context.ProductOrders.Where(o => o.OrderId == id).ToListAsync();
                    if (x != null) { 
                     context.ProductOrders.RemoveRange(x);
                        context.Orders.Remove(order);
                    }
                    
                    context.Orders.Remove(order);
                    context.Shipments.Remove(context.Shipments.Find(order.ShipmentId));
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
                }
                return Ok(new { massage = $" successful delate for Order : {order.Id}", Deleted_Order = order, Remaining_list = await context.Orders.ToListAsync() });
            } else { return BadRequest(); }
        }
    }
}
