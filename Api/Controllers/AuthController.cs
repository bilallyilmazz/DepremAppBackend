using Api.Models;
using Api.Models.ResponseModels;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract;
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

		[HttpPost("register")]
		public async Task<IActionResult> Register(LoginRequest model)
		{
			var user = new User { UserName = model.UserName};
			var result = await _userManager.CreateAsync(user, model.Password);
			//await _userManager.AddToRoleAsync(user, "Admin");
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok();
		}

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
			var reToken=_jwtService.GetPrincipalFromToken(token);
		  loginResponse = new LoginResponse()
			{
				Status=1,
				Data=new Data() { Token=token,Role="test"},
				Message="Başarılı"
			};

			return Ok(loginResponse);
		}

		[HttpPost("getusers")]
		public async Task<IActionResult> GetUsers()
		{
			List<GetUsersResponse> users = _userManager.Users.Select(x => new GetUsersResponse() { UserName = x.UserName, UserId = x.Id }).ToList();


			if (users.Count>0)
			{
				return Ok(new { Status = true, Data = users, Message = "Başarılı" });

			}
			return BadRequest(new { Status = false, Data = users, Message = "Başarısız" });

		}

	}
}
