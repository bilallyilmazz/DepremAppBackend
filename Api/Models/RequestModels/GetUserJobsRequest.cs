namespace Api.Models.RequestModels
{
	public class GetUserJobsRequest
	{
		public int UserId { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
	}
}
