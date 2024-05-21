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
    public class ShipmentController : ControllerBase
    {
        private AdminDashboardMVC_DBContext context { get; set; }

        public ShipmentController(AdminDashboardMVC_DBContext context)
        {
            this.context = context;
        }

        // GET: api/<ShipmentController>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<ShipmentDTO> shipments = await context.Shipments.Select(s => new ShipmentDTO {
                 Id = s.Id,
                 Address = s.Address,
                 Cost = s.Cost,
                 Date = s.Date,
                 IsCompleted = s.IsCompleted,
                 moreDetails= s.MoreDetails
                }).ToListAsync();
            if (shipments == null) { return NotFound(" There is no data in shippment"); }
            return Ok(shipments);
        }

        // GET api/<ShipmentController>/5
        [HttpGet("{id:int}")]
        public async Task< ActionResult> GetOne(int id)
        {
            var shipment = await context.Shipments.FindAsync(id);
            if (shipment == null)
            { 
                return NotFound("Wrong Id, There is no shippment with this id "); 
            } 
            else
            {
                ShipmentDTO NewShipment = new ShipmentDTO()
                {
                    Id = shipment.Id,
                    Address = shipment.Address,
                    Cost = shipment.Cost,
                    Date = shipment.Date,
                    IsCompleted = shipment.IsCompleted,
                    moreDetails= shipment.MoreDetails
                }
                    ;
                return Ok(NewShipment);
            }
            
        }

        // POST api/<ShipmentController>
        [HttpPost]
        public async Task<ActionResult> Post(ShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
            {

                Shipment shipment = new Shipment()
                {
                    MoreDetails = shipmentDTO.moreDetails,
                    Address = shipmentDTO.Address,
                    Cost = shipmentDTO.Cost,
                    Date = shipmentDTO.Date.Value.AddDays(5),
                    IsCompleted = shipmentDTO.IsCompleted

                };

               context.Shipments.Add(shipment);
                try
                {
                   await context.SaveChangesAsync();
                }catch(Exception ex)
                {
                    return BadRequest("Message : "+ex.Message + " | => InnerException : " + ex.InnerException);
                }

                
                string newUrl = Url.Action(nameof(GetOne), new { id = shipment.Id });

                return Created(newUrl, shipment);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/<ShipmentController>/5
       
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] ShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
            {
                Shipment shipment =await context.Shipments.FindAsync(id);
                if (shipment != null)
                {
                    shipment.Address = shipmentDTO.Address;
                    shipment.Cost = shipmentDTO.Cost;
                    shipment.Date = shipmentDTO.Date;
                    shipment.IsCompleted = shipmentDTO.IsCompleted;
                    shipment.MoreDetails = shipmentDTO.moreDetails;
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
                    }

                }
                return StatusCode(204, new { massage = " successful update", New_Shippment = shipment });

            } else { return BadRequest(); }
        }


        // DELETE api/<ShipmentController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Shipment shipment = await context.Shipments.FindAsync(id);
            if (shipment != null)
            {
                
                try
                {
                    context.Shipments.Remove(shipment);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
                }
                return Ok(new { massage = $" successful delate for shippment : {shipment.Id}", Deleted_Shipment = shipment, Remaining_list = await context.Shipments.ToListAsync() });
            } else { return BadRequest(); }
        }
    }
}
