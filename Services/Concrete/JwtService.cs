using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
	public class JwtService : IJwtService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly UserManager<User> _userManager;
		private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;
		private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

		public JwtService(IOptions<JwtSettings> options, UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
		{
			_jwtSettings = options.Value;
			_userManager = userManager;
			_claimsPrincipalFactory = claimsPrincipalFactory;
			_jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
		}
		public string GenerateToken(User user)
		{
			var principal = _claimsPrincipalFactory.CreateAsync(user).Result;
			var token = _jwtSecurityTokenHandler.CreateToken(new SecurityTokenDescriptor
			{
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256),
				Subject = principal.Identity as ClaimsIdentity
			});
			return _jwtSecurityTokenHandler.WriteToken(token);
		}

		public ClaimsPrincipal GetPrincipalFromToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true
			};
			var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
			return principal;
		}
	}
}
