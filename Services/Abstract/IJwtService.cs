using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
	public interface IJwtService
	{
		string GenerateToken(User user);
		ClaimsPrincipal GetPrincipalFromToken(string token);
	}
}
