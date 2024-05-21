using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IPasswordHasher<AspNetUser> _passwordHasher;


        private readonly AdminDashboardMVC_DBContext _context;

        public UserController(AdminDashboardMVC_DBContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<AspNetUser>();
        }

        [HttpGet("uniqueEmail")]
        public bool IsEmailUnique(string email)
        {
            // Check if the email exists in the database
            var existingEmailUser = _context.AspNetUsers.FirstOrDefault(u => u.Email == email);

            // If both email and phone number are null, they are unique; otherwise, they already exist
            return existingEmailUser == null;
        }
        [HttpGet("uniquePhone")]
        public bool IsPhoneUnique(string phoneNumber)
        {
            // Check if the phone number exists in the database
            var existingPhoneUser = _context.AspNetUsers.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            // If both email and phone number are null, they are unique; otherwise, they already exist
            return existingPhoneUser == null;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {

            var allUsers = _context.AspNetUsers
                .Include(u => u.Orders)
                .Include(u => u.Reviews)
                .Include(ur => ur.Roles)
                .Include(u => u.ProductUsers)
                .Select(u => new userdetails
                {
                    Id = u.Id,
                    PhoneNumber = u.PhoneNumber,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    Email = u.Email,
                    Street = u.Street,
                    SSN = u.Ssn,
                    Age = u.Age,
                    City = u.City,
                    Governorate = u.Governorate,
                    Password = u.Password,
                    Role = u.Roles.Select(ur => ur.Name).FirstOrDefault(),
                    OrderIds = u.Orders.Select(o => o.Id).ToList(),
                    ReviewIds = u.Reviews.Select(r => r.Id).ToList(),
                    ProductIds = u.ProductUsers.Select(pu => pu.ProductId).ToList()
                })
                .ToList();

            return Ok(allUsers);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneUser(int id)
        {
            var User = _context.AspNetUsers.Include(u => u.Orders).Include(u => u.Reviews).Include(u => u.ProductUsers).Include(u => u.Roles)
                .FirstOrDefault(x => x.Id == id);
            userdetails u = new userdetails();
            if (User == null) { return NotFound(); }
            else
            {
                u.Id = User.Id; u.PhoneNumber = User.PhoneNumber; u.LastName = User.LastName; u.FirstName = User.FirstName; u.Email = User.Email;
                u.Street = User.Street; u.SSN = User.Ssn; u.Age = User.Age; u.City = User.City; u.Governorate = User.Governorate; u.Password = User.Password;
                u.Role = User.Roles.Select(r => r.Name).FirstOrDefault();
                u.OrderIds = User.Orders.Select(o => o.Id).ToList();
                u.ReviewIds = User.Reviews.Select(r => r.Id).ToList();
                u.ProductIds = User.ProductUsers.Select(pu => pu.ProductId).ToList();
            }
            return Ok(u);
        }

        [HttpPost]
        public IActionResult AddOneUser(userdetails u)
        {
            bool unique1 = IsEmailUnique(u.Email);
            if (!unique1)
            {
                return Ok(new { Error = " Email is taken before ! " });
            }

            bool unique2 = IsPhoneUnique(u.PhoneNumber);
            if (!unique2)
            {
                return Ok(new { Error = " Phone number is taken before ! " });
            }

            AspNetUser newUser = new AspNetUser();
            if (u == null) { return BadRequest("User details cannot be null."); }

            else
            {
                /*newUser.Id = u.Id;*/
                newUser.PhoneNumber = u.PhoneNumber; newUser.LastName = u.LastName; newUser.FirstName = u.FirstName; newUser.Email = u.Email;
                newUser.Street = u.Street; newUser.Ssn = u.SSN; newUser.Age = u.Age; newUser.City = u.City; newUser.Governorate = u.Governorate; newUser.Password = u.Password;

                //// Hash the password
                //var hashedPassword = _userManager.PasswordHasher.HashPassword(newUser, u.Password);
                //newUser.Password = hashedPassword;

                _context.Add(newUser);
                _context.SaveChanges();

                //return Ok(u);
                return CreatedAtAction("GetOneUser", new { id = u.Id }, newUser);
            }
        }


        // POST: api/Users
        [HttpPost("{roleId:int}")]
        public async Task<ActionResult<AspNetUser>> PostUser(userdetails u, int roleId)
        {
            AspNetUser newUser = new AspNetUser();

            /*newUser.Id = u.Id;*/
            newUser.PhoneNumber = u.PhoneNumber; newUser.LastName = u.LastName; newUser.FirstName = u.FirstName; newUser.Email = u.Email;
            newUser.Street = u.Street; newUser.Ssn = u.SSN; newUser.Age = u.Age; newUser.City = u.City; newUser.Governorate = u.Governorate; newUser.Password = u.Password;
            newUser.UserName = u.Email;
            newUser.NormalizedUserName = u.Email;
            newUser.NormalizedEmail = u.Email;


            _context.Add(newUser);
            _context.SaveChanges();

            // Hash the password
            var hashedPassword = _passwordHasher.HashPassword(newUser, newUser.Password);
            newUser.SecurityStamp = Guid.NewGuid().ToString();
            newUser.PasswordHash = hashedPassword;


            // Retrieve the role based on the roleId
            var role = await _context.AspNetRoles.FindAsync(roleId);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            // Assign the role to the user
            newUser.Roles = new List<AspNetRole> { role };
            await _context.SaveChangesAsync();

            // Prepare the UserDetails object for response
            var userDetails = new userdetails()
            {
                Id = newUser.Id,
                PhoneNumber = newUser.PhoneNumber,
                LastName = newUser.LastName,
                FirstName = newUser.FirstName,
                Email = newUser.Email,
                Street = newUser.Street,
                SSN = newUser.Ssn,
                Age = newUser.Age,
                City = newUser.City,
                Governorate = newUser.Governorate,
                Password = newUser.Password,
                Role = newUser.Roles.Select(r => r.Name).FirstOrDefault(),
                OrderIds = newUser.Orders.Select(o => o.Id).ToList(),
                ReviewIds = newUser.Reviews.Select(r => r.Id).ToList(),
                ProductIds = newUser.ProductUsers.Select(pu => pu.ProductId).ToList()
            };

            // Return the created user along with details
            return Ok(userDetails);
        }


        [HttpPut("{id:int}")]
        public IActionResult UpdateOneUser(userdetails u, int id)
        {
            AspNetUser OldUser = _context.AspNetUsers.Find(id);
            bool unique1 = IsEmailUnique(u.Email);
            bool unique2 = IsPhoneUnique(u.PhoneNumber);
            if (OldUser.Email != u.Email)
            {
                if (!unique1)
                {
                    return Ok(new { Error = " Email is taken before ! " });
                }
            }
            if (OldUser.PhoneNumber != u.PhoneNumber)
            { 
                if (!unique2)
                {
                    return Ok(new { Error = " Phone number is taken before ! " });
                }
            }





            if (OldUser != null)
            {
                //OldUser.Id = u.Id;
                OldUser.PhoneNumber = u.PhoneNumber ?? OldUser.PhoneNumber; OldUser.LastName = u.LastName ?? OldUser.PhoneNumber; OldUser.FirstName = u.FirstName ?? OldUser.FirstName; OldUser.Email = u.Email ?? OldUser.Email;
                OldUser.Street = u.Street ?? OldUser.Street; OldUser.Ssn = u.SSN ?? OldUser.Ssn; OldUser.Age = u.Age ?? OldUser.Age; OldUser.City = u.City ?? OldUser.City; OldUser.Governorate = u.Governorate ?? OldUser.Governorate; OldUser.Password = u.Password ?? OldUser.Password;

                _context.SaveChanges();
                  


                return Ok(OldUser);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPut("{id:int}/{roleId:int}")]
        public async Task<ActionResult<AspNetUser>> UpdateUser(userdetails u, int id, int roleId)
        {
            // Retrieve the existing user from the database based on userId
            AspNetUser OldUser = _context.AspNetUsers.Include(u => u.Roles).FirstOrDefault(x => x.Id == id);
            //var oldRoleId= OldUser.Roles.FirstOrDefault();
            if (OldUser == null)
            {
                return NotFound("User not found");
            }

            // Update the user details
            OldUser.PhoneNumber = u.PhoneNumber;
            OldUser.LastName = u.LastName;
            OldUser.FirstName = u.FirstName;
            OldUser.Email = u.Email;
            OldUser.Street = u.Street;
            OldUser.Ssn = u.SSN;
            OldUser.Age = u.Age;
            OldUser.City = u.City;
            OldUser.Governorate = u.Governorate;
            OldUser.Password = u.Password;
            OldUser.UserName = u.Email;
            OldUser.NormalizedUserName = u.Email;
            OldUser.NormalizedEmail = u.Email;
            // Hash the password
            var hashedPassword = _passwordHasher.HashPassword(OldUser, OldUser.Password);
            OldUser.SecurityStamp = Guid.NewGuid().ToString();
            OldUser.PasswordHash = hashedPassword;

            // Retrieve the role based on the roleId
            var role = await _context.AspNetRoles.FindAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            // Update the user's role
            OldUser.Roles = new List<AspNetRole> { role };
            await _context.SaveChangesAsync();

            // Prepare the UserDetails object for response
            var userDetails = new userdetails()
            {
                Id = OldUser.Id,
                PhoneNumber = OldUser.PhoneNumber,
                LastName = OldUser.LastName,
                FirstName = OldUser.FirstName,
                Email = OldUser.Email,
                Street = OldUser.Street,
                SSN = OldUser.Ssn,
                Age = OldUser.Age,
                City = OldUser.City,
                Governorate = OldUser.Governorate,
                Password = OldUser.Password,
                Role = OldUser.Roles.Select(r => r.Name).FirstOrDefault(),
                OrderIds = OldUser.Orders.Select(o => o.Id).ToList(),
                ReviewIds = OldUser.Reviews.Select(r => r.Id).ToList(),
                ProductIds = OldUser.ProductUsers.Select(pu => pu.ProductId).ToList()
            };

            // Return the updated user details
            return Ok(userDetails);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {

            AspNetUser DeletedUser = _context.AspNetUsers.Find(id);
            if (DeletedUser != null)
            {
                // delete all user reviews
                //var reviews = _context.Reviews.Where(r => r.UserId == id).ToList();
                //if (reviews != null || reviews.Count == 0)
                //{
                //    _context.RemoveRange(reviews);
                //    _context.SaveChanges();

                //}



                _context.AspNetUsers.Remove(DeletedUser);
                _context.SaveChanges();
                //to add Default admin if there is no admin in users
                if (!_context.AspNetUsers.Any(u => u.Roles.FirstOrDefault().Id == 1))
                {
                    AspNetUser newUser = new AspNetUser();

                    /*newUser.Id = u.Id;*/
                    newUser.PhoneNumber = "011111111111"; newUser.LastName = "main admin"; newUser.FirstName = "main admin"; newUser.Email = "mainAdmain@Admin";
                    newUser.Password = "Asd123*-*";
                    newUser.UserName = "mainAdmain@Admin";
                    newUser.NormalizedUserName = "mainAdmain@Admin";
                    newUser.NormalizedEmail = "mainAdmain@Admin";


                    _context.Add(newUser);
                    _context.SaveChanges();

                    // Hash the password
                    var hashedPassword = _passwordHasher.HashPassword(newUser, newUser.Password);
                    newUser.SecurityStamp = Guid.NewGuid().ToString();
                    newUser.PasswordHash = hashedPassword;


                    // Retrieve the role based on the roleId
                    var role = _context.AspNetRoles.Find(1);

                    if (role == null)
                    {
                        return NotFound("Role not found");
                    }

                    // Assign the role to the user
                    newUser.Roles = new List<AspNetRole> { role };
                    _context.SaveChanges();
                }
                return Ok(DeletedUser);
                //return Ok(new
                //{
                //    message = $"emplyee with id={DeletedUser.Id} is deleted",
                //    user = DeletedUser,
                //    Remaining = _context.Users.ToList()
                //});
            }
            else
                return NotFound();
        }
    }
}
