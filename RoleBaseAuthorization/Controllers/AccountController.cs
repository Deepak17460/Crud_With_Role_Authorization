using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using RoleBaseAuthorization.Models;
using RoleBaseAuthorization.Models.Authentication;

namespace RoleBaseAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        [HttpPost]

        public async Task<IActionResult> Register([FromBody] UserRegister register,string role)
        {
            //Check User if exist already
            var userExit= await _userManager.FindByEmailAsync(register.Email);
            if(userExit != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User Already exist" });
            }
            // Add new user to the database
            IdentityUser user = new()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.UserName
            };
           // var result=await _userManager.CreateAsync(user,register.Password);
           if(await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, register.Password);
                if(!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                   new Response { Status = "Error", Message = "Failed to Created user into the Database" });

                }
                // Add role to the user
                await _userManager.AddToRoleAsync(user,role);
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Success", Message = "Created New User Successfully!" });

            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "This Role Does not exit" });

            }
           
         

           
        }
    }
}
