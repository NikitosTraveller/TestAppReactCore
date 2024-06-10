using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Helpers;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;
using ReactApp1.Server.Validators;
using System.Security.Claims;
using TestApp.Server.Data;
using TestApp.Server.Models;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(SignInManager<User> sm, UserManager<User> um, 
        AppDBContext context, IUserService userService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        private readonly IUserService _userService = userService;
        private readonly SignInManager<User> _signInManager = sm;
        private readonly UserManager<User> _userManager = um;
        private readonly AppDBContext _appDBContext = context;

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

                result = await _userService.CreateUserAsync(_user, registerRequest.Password);

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
                User? _user = await _userService.GetUserByEmailAsync(loginRequest.Email);

                if (_user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(_user, loginRequest.Password, loginRequest.RememberMe, false);

                    if (!result.Succeeded)
                    {
                        return Unauthorized(new { message = "Check your login credentials and try again" });
                    }

                    var updateResult = await _userService.LoginUserAsync(_user); 
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
                User? userToDelete = await _userService.GetUserByIdAsync(id);

                if (userToDelete == null) {
                    return BadRequest(new {message = "User doesn't exist!"});
                }

                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                var currentUserRole = await _userService.GetUserRoleAsync(currentUser);
                var userToDeleteRole = await _userService.GetUserRoleAsync(userToDelete); 
                var r1 = RoleHelper.GetRoleByName(currentUserRole);
                var r2 = RoleHelper.GetRoleByName(userToDeleteRole);

                bool isDeletePermitted = UserOperationsValidator.IsDeletePermitted(currentUser.Id, userToDelete.Id, r2, r1);

                if (!isDeletePermitted)
                {
                    return Forbid("Delete forbidden");
                }

                var result = await _userService.DeleteUserAsync(userToDelete);

                if (result.Succeeded)
                {
                    return Ok("Deleted");
                }

                return BadRequest("Someting went wrong, please try again.");

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Someting went wrong, please try again. " + ex.Message });
            }
        }

        [HttpPut("changerole"), Authorize(Roles ="Admin,SuperAdmin")]
        public async Task<ActionResult> ChangeUserRole(ChangeRoleRequest changeRoleRequest)
        {
            var userToChange = await _userService.GetUserByIdAsync(changeRoleRequest.Id);

            if (userToChange == null) {
                return BadRequest();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var currentUserRole = await _userService.GetUserRoleAsync(currentUser);
            var userToChangeRole = await _userService.GetUserRoleAsync(userToChange);
            var r1 = RoleHelper.GetRoleByName(currentUserRole);
            var r2 = RoleHelper.GetRoleByName(userToChangeRole);
            bool isChangeRolePermitted = UserOperationsValidator.IsChangeRolePermitted(currentUser.Id, userToChange.Id, r2, r1);

            if (!isChangeRolePermitted)
            {
                return Forbid("Change Role forbidden");
            }

            await _userService.ChangeUserRoleAsync(userToChange, changeRoleRequest.Role);

            var result = _mapper.Map<UserResponse>(userToChange);
            result.RoleName = changeRoleRequest.Role;

            return Ok(new { updatedUser = result });
        }

        [HttpGet("users"), Authorize]
        public async Task<ActionResult> GetAllUsers()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                return Forbid();
            }

            var data = await _userService.GetAllUsersAsync(currentUser.Id);
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

            var role = await _userService.GetUserRoleAsync(currentuser);

            return Ok(new { message = "Logged in", user = new UserResponse() { Email = currentuser.Email, RoleName = role } });
        }

        [HttpPost("avatar"), Authorize]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadFileRequest uploadFileRequest)
        {
            if(!ModelState.IsValid || uploadFileRequest.FormFile.Length > 20*1024)
            {
                return BadRequest("Error while uploading.");
            }

            var _user = await _userService.GetUserByIdAsync(uploadFileRequest.UserId);

            if (_user == null)
            {
                return BadRequest("");
            }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var currentUserRole = await _userService.GetUserRoleAsync(currentUser);
            var roleToSetAvatar = await _userService.GetUserRoleAsync(_user);
            var r1 = RoleHelper.GetRoleByName(currentUserRole);
            var r2 = RoleHelper.GetRoleByName(roleToSetAvatar);

            bool isSetAvatarPermitted = UserOperationsValidator.IsSetAvatarPermitted(currentUser.Id, _user.Id, r2, r1);

            if (!isSetAvatarPermitted)
            {
                return Forbid("Set Avatar forbidden");
            }

            string uniqueFileName = await FileHelper.SaveAvatarImageAsync(uploadFileRequest, _user.Avatar);

            _user.Avatar = $"/Avatars/{uniqueFileName}";

            await _userService.UpdateUserAsync(_user);

            return Ok(new { user = _mapper.Map<UserResponse>(_user) });
        }
    }
}
