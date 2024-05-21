using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private AdminDashboardMVC_DBContext context { get; set; }

        public RoleController(AdminDashboardMVC_DBContext context)
        {
            this.context = context;
        }

        // GET: 
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<RoleDTO> AspNetRoles = await context.AspNetRoles.Select(s => new RoleDTO
            {
                Id = s.Id,
                Name = s.Name,
                NormalizedName = s.NormalizedName,
                ConcurrencyStamp = s.ConcurrencyStamp

            }).ToListAsync();
            if (AspNetRoles == null) { return NotFound(" There is no data "); }
            return Ok(AspNetRoles);
        }



        // GET 
        // GET 
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetOne(int id)
        {
            var role = await context.AspNetRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound("Wrong Id, There is no Role with this id ");
            }
            else
            {
                RoleDTO Newrole = new RoleDTO()
                {
                    Id = role.Id,
                    Name = role.Name,
                    NormalizedName = role.NormalizedName,
                    ConcurrencyStamp = role.ConcurrencyStamp,
                    Users_IDs = role.Users.Select(x => x.Id).ToList()
                }
                    ;
                return Ok(Newrole);
            }

        }

            // POST 
            //[HttpPost]
            //public async Task<ActionResult> Post(RoleDTO roleDTO)
            //{
            //    if (ModelState.IsValid)
            //    {

            //        AspNetRole role = new AspNetRole()
            //        {
            //            Id = roleDTO.Id,
            //            Name = roleDTO.Title,

            //        };
            //        await context.AspNetRoles.AddAsync(role);
            //        try
            //        {
            //            await context.SaveChangesAsync();
            //        }
            //        catch (Exception ex)
            //        {
            //            return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
            //        }


            //        string newUrl = Url.Action(nameof(GetOne), new { id = role.Id });

            //        return Created(newUrl, role);

            //    }
            //    else
            //    {
            //        return BadRequest("error1");
            //    }
            //}

            // PUT 

            [HttpPut("{id}")]
            public async Task<ActionResult> Put([FromRoute] int id, [FromBody] RoleDTO roleDTO)
            {
                if (ModelState.IsValid)
                {
                    AspNetRole role = await context.AspNetRoles.FindAsync(id);
                    if (role != null)
                    {
                        role.Name = roleDTO.Name;
                        role.NormalizedName = roleDTO.NormalizedName;
                        role.ConcurrencyStamp = roleDTO.ConcurrencyStamp;

                        try
                        {
                            await context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
                        }

                    }
                    return StatusCode(204, new { massage = " successful update", New_Role = role });

                }
                else { return BadRequest(); }
            }


            // DELETE 
            //[HttpDelete("{id}")]
            //public async Task<ActionResult> Delete(int id)
            //{
            //    AspNetRole role = await context.AspNetRoles.FindAsync(id);
            //    if (role != null)
            //    {

            //        try
            //        {
            //            context.AspNetRoles.Remove(role);
            //            await context.SaveChangesAsync();
            //        }
            //        catch (Exception ex)
            //        {
            //            return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
            //        }
            //        return Ok(new { massage = $" successful delate for Role : {role.Id}", Deleted_Role = role, Remaining_list = await context.Roles.ToListAsync() });
            //    }
            //    else { return BadRequest(); }
            //}
 } } 

