using Api.Models.RequestModels;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using System.Data;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		private readonly IJobService _jobService;
		private readonly IJwtService _jwtService;
		private readonly UserManager<User> _userManager;
		public JobController(IJobService jobService, IJwtService jwtService, UserManager<User> userManager)
		{
			_jobService = jobService;
			_jwtService = jwtService;
			_userManager = userManager;
		}

		[Authorize(Roles = "User")]
		[HttpPost("insert")]
		public async Task<IActionResult> Insert(JobRequestModel model)
		{
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			var principal = _jwtService.GetPrincipalFromToken(token);

			var userName = principal.Identity.Name;
			var user=await _userManager.FindByNameAsync(userName);

			Job job = new Job()
			{
				WorkId=model.WorkId,
				Lat=model.Lat,
				Lng=model.Lng,
				Date=model.Date,
				Note=model.Note,
				WorkType=model.WorkType,
				UserId=user.Id
			};

			var result=await _jobService.Add(job);
			if (result>0)
			{
				return Ok(new { Status = true, Data = result, Message = "Başarılı" });
			}
			return BadRequest(new { Status = false, Data = result, Message = "Başarısız" });
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("getuserjobs")]
		public async Task<IActionResult> GetUserJobs(GetUserJobsRequest model)
		{
			var result = await _jobService.GetUserJobs(model.UserId,Convert.ToDateTime(model.StartDate).ToUniversalTime(), Convert.ToDateTime(model.EndDate).ToUniversalTime());
		
			if (result.Count > 0)
			{
				return Ok(new { Status = true, Data = result, Message = "Başarılı" });
			}
			return BadRequest(new { Status = false, Data = result, Message = "Başarısız" });
		}
	}
}
