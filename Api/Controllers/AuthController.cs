using Api.Models;
using Api.Models.ResponseModels;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IJwtService _jwtService;

		public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwtService, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtService = jwtService;
			_roleManager = roleManager;
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("register")]
		public async Task<IActionResult> Register(LoginRequest model)
		{
			var user = new User { UserName = model.UserName};
			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok();
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("addrole")]
		public async Task<IActionResult> AddRole(string name)
		{
			
			var result = await _roleManager.CreateAsync(new Role() { Name= name });
			
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok();
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("adduserrole")]
		public async Task<IActionResult> AddUserRole(string userName,string roleName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			var role = await _roleManager.FindByNameAsync(roleName);
			
			var result = await _userManager.AddToRoleAsync(user,role.Name);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginRequest model)
		{
			LoginResponse loginResponse = new LoginResponse();
			var user = await _userManager.FindByNameAsync(model.UserName);
			if (user == null)
			{
				return Unauthorized();
			}
			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (!result.Succeeded)
			{
			  loginResponse = new LoginResponse()
				{
					Status = 0,
					Data = new Data(),
					Message = "Başarısız"
				};

				return Unauthorized();
			}

			// Kullanıcı giriş yaptıktan sonra JWT üretin ve geri döndürün
			var token = _jwtService.GenerateToken(user);
			var roles = await _userManager.GetRolesAsync(user);
		  loginResponse = new LoginResponse()
			{
				Status=1,
				Data=new Data() { Token=token,Role=roles.FirstOrDefault()},
				Message="Başarılı"
			};

			return Ok(loginResponse);
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("getusers")]
		public async Task<IActionResult> GetUsers()
		{
			List<GetUsersResponse> users =  _userManager.GetUsersInRoleAsync("User").Result.Select(x => new GetUsersResponse() { UserName = x.UserName, UserId = x.Id }).ToList();
			

			if (users.Count>0)
			{
				return Ok(new { Status = true, Data = users, Message = "Başarılı" });

			}
			return BadRequest(new { Status = false, Data = users, Message = "Başarısız" });

		}
		[Authorize(Roles = "Admin")]
		[HttpPost("getuserbyid")]
		public async Task<IActionResult> GetUserById(int id)
		{
			User responseUser = await _userManager.FindByIdAsync(id.ToString());


			if (responseUser != null)
			{
				return Ok(new { Status = true, Data = responseUser, Message = "Başarılı" });

			}
			return BadRequest(new { Status = false, Data = responseUser, Message = "Başarısız" });

		}

	}
}
