using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewDetaildController : ControllerBase
    {
        private AdminDashboardMVC_DBContext context { get; set; }

        public ReviewDetaildController(AdminDashboardMVC_DBContext context)
        {
            this.context = context;
        }
        // GET: 
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<ReviewDetailsDTO> Reviews = await context.Reviews.Select(s => new ReviewDetailsDTO
            {
                Id = s.Id,
                UserID = s.UserId,
                Comment = s.Comment,
               // Date = string.Format("{0:dd/MM/yyyy}", s.Date)

                Date = s.Date           


            }).ToListAsync();
            foreach (var item in Reviews)
            {
                AspNetUser u = await context.AspNetUsers.FindAsync(item.UserID);
                if (u != null)
                {
                    item.FirstName = u.FirstName;
                    item.LastName = u.LastName;
                    item.Email = u.Email;

                }
                else
                {
                    item.FirstName = "Removed user";
                    item.LastName = "";
                    item.Email = "";

                }
            }
            if (Reviews == null) { return NotFound(" There is no data "); }
            return Ok(Reviews);
        }

        // GET
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetOne(int id)
        {
            var review = await context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound("Wrong Id, There is no review with this id ");
            }
            else
            {
                ReviewDetailsDTO reviewDTO = new ReviewDetailsDTO()
                {
                    Id = review.Id,
                    UserID = review.UserId,
                    Comment = review.Comment,
                    Date=review.Date
                  //  Date = string.Format("{0:dd/MM/yyyy}", review.Date)s

                  

                };
                AspNetUser u = await context.AspNetUsers.FindAsync(reviewDTO.UserID);
                if (u != null)
                {
                    reviewDTO.FirstName = u.FirstName;
                    reviewDTO.LastName = u.LastName;
                    reviewDTO.Email = u.Email;

                }
                else
                {
                    reviewDTO.FirstName = "Removed user";
                    reviewDTO.LastName = "";
                    reviewDTO.Email = "";

                }

                return Ok(reviewDTO);
            }

        }
       /// POST
       [HttpPost]
        public async Task<ActionResult> Post(ReviewDTO reviewDTO)
        {
            if (!await context.AspNetUsers.AnyAsync(u => u.Id == reviewDTO.UserID))
                return BadRequest("Wrong User Id ");


            if (ModelState.IsValid)
            {
                Review review = new Review()
                {
                    Id = reviewDTO.Id,

                    UserId = reviewDTO.UserID,
                    Date = reviewDTO.Date,

                    Comment = reviewDTO.Comment,

                };
                await context.Reviews.AddAsync(review);
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
                }

                string newUrl = Url.Action(nameof(GetOne), new { id = review.Id });

                return Created(newUrl, review);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Review review = await context.Reviews.FindAsync(id);
            if (review != null)
            {

                try
                {
                    context.Reviews.Remove(review);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest("Message : " + ex.Message + " | => InnerException : " + ex.InnerException);
                }
                return Ok(new { massage = $" successful delate for Order : {review.Id}", Deleted_review = review, Remaining_list = await context.Reviews.ToListAsync() });
            }
            else { return BadRequest(); }
        }
    
}
}

