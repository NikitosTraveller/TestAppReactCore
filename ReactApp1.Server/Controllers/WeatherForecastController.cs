using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System;
using System.Security.Claims;
using TestApp.Server.Models;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(SignInManager<User> sm, UserManager<User> um, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        private readonly SignInManager<User> _signInManager = sm;
        private readonly UserManager<User> _userManager = um;

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterRequest registerRequest)
        {

            IdentityResult result = new();

            try
            {
                User _user = new User()
                {
                    Email = registerRequest.Email,
                    UserName = registerRequest.UserName,
                };

                result = await _userManager.CreateAsync(_user, registerRequest.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }

                var addedUser = await _userManager.FindByEmailAsync(registerRequest.Email);
                await _userManager.AddToRoleAsync(addedUser, Role.Regular.ToString());

            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, please try again. " + ex.Message);
            }

            return Ok(new { message = "Registered Successfully.", result = result });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(LoginRequest loginRequest)
        {

            try
            {
                User user_ = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user_ != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user_, loginRequest.Password, loginRequest.RememberMe, false);

                    if (!result.Succeeded)
                    {
                        return Unauthorized(new { message = "Check your login credentials and try again" });
                    }

                    user_.LastLoginDate = DateTime.Now;
                    user_.LoginCount++;

                    var updateResult = await _userManager.UpdateAsync(user_);
                }
                else
                {
                    return BadRequest(new { message = "Please check your credentials and try again. " });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong, please try again. " + ex.Message });
            }

            return Ok(new { message = "Login Successful." });
        }

        [HttpGet("logout"), Authorize]
        public async Task<ActionResult> LogoutUser()
        {

            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Someting went wrong, please try again. " + ex.Message });
            }

            return Ok(new { message = "You are free to go!" });
        }

        [HttpDelete("delete/{id}"), Authorize]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                User userInfo = await _userManager.FindByIdAsync(id);

                if (userInfo == null) {
                    return BadRequest(new {message = "User doesn't exist!"});
                }

                var result = await _userManager.DeleteAsync(userInfo);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Someting went wrong, please try again. " + ex.Message });
            }
        }

        [HttpGet("admin"), Authorize]
        public async Task<ActionResult> AdminPage()
        {
            var result = await _userManager.Users.Select(user => _mapper.Map<UserResponse>(user)).ToListAsync();
            return Ok(new { users = result });
        }

        [HttpGet("home/{email}"), Authorize]
        public async Task<ActionResult> HomePage(string email)
        {
            User userInfo = await _userManager.FindByEmailAsync(email);
            if (userInfo == null)
            {
                return BadRequest(new { message = "Something went wrong, please try again." });
            }

            return Ok(new { userInfo = _mapper.Map<UserResponse>(userInfo) });
        }

        [HttpGet("xhtlekd")]
        public async Task<ActionResult> CheckUser()
        {
            User currentuser = new();

            try
            {
                var user_ = HttpContext.User;
                var principals = new ClaimsPrincipal(user_);
                var result = _signInManager.IsSignedIn(principals);
                if (result)
                {
                    currentuser = await _signInManager.UserManager.GetUserAsync(principals);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong please try again. " + ex.Message });
            }

            return Ok(new { message = "Logged in", user = currentuser });
        }
    }
}
