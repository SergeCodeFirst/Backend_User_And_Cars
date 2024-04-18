using System;
using backend.Dtos.User;
using backend.Services.ServiceResponse;
using backend.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
	{
		private readonly IUserService _userServise;

		public UserController( IUserService userServise)
		{
			_userServise = userServise;
		}

        [HttpGet("api/getallusers", Name = "GetAllUser"), Authorize(Roles = "User")]
        public ActionResult<string> getUsers()
        {
            return Ok(_userServise.getAllUsers());
        }

        // REGISTRATION PROCESS
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("api/resgister", Name ="AddNewUser")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> addNewUser([FromBody]AddUserDto newUser)
        {
            // Check the formate of name, email, and Pwd
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sent Info to user Service
            ServiceResponse<GetUserDto>  res = await _userServise.addUser(newUser);

            // Check if User already existe
            if (res.success == false)
            {
                Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
                errors["email"] = new List<string>{"Email already existe"};
               
                Dictionary<string, Dictionary<string, List<string>>> myres = new Dictionary<string, Dictionary<string, List<string>>>();
                myres["errors"] = errors;

                return BadRequest(myres);
            }

            // Adding my token into a cookies
            HttpContext.Response.Cookies.Append("Token", res.auth!,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

            res.auth = "";

            return Ok(res);
        }

        // LOGIN PROCESS
        [HttpPost("api/login", Name = "LoginRoute")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>>  Login([FromBody]LoginUserDto logUser)
        {
            // Check User email and Passsword
            var res = await _userServise.LoginProcess(logUser);
            if (!res.success)
            {
                return BadRequest(res.message);
            }

            // Adding my token into a cookies
            HttpContext.Response.Cookies.Append("Token", res.auth!,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

            res.auth = "";

            return Ok(res);
        }

        // GET ONE USER
        [HttpGet("api/getuserwithid", Name = "getOneUserWithId")]
        public ActionResult<string> getOneUserWithId()
        {
            string token = HttpContext.Request.Cookies["Token"];
            //Console.WriteLine(cookieValue);

            if (token == null)
            {
                token = "Cookie Not Found!";
                return BadRequest("User Must Login");
            }

            var res = _userServise.getUserWithId(token);

            return Ok(res);
        }

        // LOGOUT
        [HttpPost("api/logout", Name = "LogoutUser")]
        public ActionResult<string> logout()
        {
            Response.Cookies.Delete("Token");
            return Ok("User Logout successfuly!");
        }
    }
}

