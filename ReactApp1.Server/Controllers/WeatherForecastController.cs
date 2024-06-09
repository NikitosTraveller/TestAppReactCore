using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System;
using System.Security.Claims;
using TestApp.Server.Data;
using TestApp.Server.Models;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(SignInManager<User> sm, UserManager<User> um, 
        AppDBContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        private readonly SignInManager<User> _signInManager = sm;
        private readonly UserManager<User> _userManager = um;
        private readonly AppDBContext _appDBContext = context;
        private readonly IWebHostEnvironment _hostEnvironment = webHostEnvironment;

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
                var addedUser = await _userManager.FindByEmailAsync(registerRequest.Email);
                await _userManager.AddToRoleAsync(addedUser, AppUserRole.Regular.ToString());

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
                User? _user = _appDBContext.Users
                    .Where(u => u.Email == loginRequest.Email)
                    .FirstOrDefault(); 

                if (_user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(_user, loginRequest.Password, loginRequest.RememberMe, false);

                    if (!result.Succeeded)
                    {
                        return Unauthorized(new { message = "Check your login credentials and try again" });
                    }

                    _user.LastLoginDate = DateTime.Now;
                    _user.LoginCount++;

                    var updateResult = await _userManager.UpdateAsync(_user);
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

        [HttpPut("changerole"), Authorize]
        public async Task<ActionResult> ChangeUserRole(ChangeRoleRequest changeRoleRequest)
        {
            var data = await _appDBContext.Users
                .Where(u => u.Id == changeRoleRequest.Id)
                .Include(x => x.UserRoles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync();

            if (data == null) {
                return BadRequest();
            }

            await _userManager.RemoveFromRolesAsync(data, [AppUserRole.Admin.ToString(), AppUserRole.Regular.ToString()]);
            await _userManager.AddToRoleAsync(data, changeRoleRequest.Role);

            var result = _mapper.Map<UserResponse>(data);
            result.RoleName = changeRoleRequest.Role;

            return Ok(new { updatedUser = result });
        }

        [HttpGet("users"), Authorize]
        public async Task<ActionResult> GetAllUsers()
        {
            var data = await _appDBContext.Users
                .Include(x => x.UserRoles)
                .ThenInclude(r => r.Role)
                .ToListAsync();
            var result = data.Select(u => _mapper.Map<UserResponse>(u)).ToList();

            return Ok(new { users =  result});
        }

        [HttpGet("home/{email}"), Authorize]
        public async Task<ActionResult> GetCurrentUser(string email)
        {
            User? userInfo = await _appDBContext.Users
                .Where(u => u.Email == email)
                .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync();

            if (userInfo == null)
            {
                return BadRequest(new { message = "Something went wrong, please try again." });
            }
            else
            {
                var resultUser = _mapper.Map<UserResponse>(userInfo);
                var u = String.Format("{0}://{1}{2}/Avatars/{3}", Request.Scheme, Request.Host, Request.PathBase, "89d01b6b-d2ee-4ef0-9cb9-a3fcb903a7c7_CV_img.jpg");
                resultUser.Avatar = u; // Path.Combine(_hostEnvironment.ContentRootPath, "Avatars", "89d01b6b-d2ee-4ef0-9cb9-a3fcb903a7c7_CV_img.jpg"); ;
                return Ok(new { userInfo = resultUser });
            }
        }

        [HttpGet("validate")]
        public async Task<ActionResult> CheckUser()
        {
            User currentuser = new();

            try
            {
                var _user = HttpContext.User;
                var principals = new ClaimsPrincipal(_user);
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

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadFileRequest uploadFileRequest)
        {
            if (uploadFileRequest.FormFile == null || uploadFileRequest.FormFile.Length == 0)
                return BadRequest("No file uploaded.");

            //uploadFileRequest.FormFile.

            var uploadsFolder = Path.Combine("wwwroot", "Avatars");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = uploadFileRequest.UserId + "_" + uploadFileRequest.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFileRequest.FormFile.CopyToAsync(fileStream);
            }

            var avatarUrl = Url.Content($"~/Avatars/{uniqueFileName}");

            return Ok(new { avatarUrl });
        }
    }
}
