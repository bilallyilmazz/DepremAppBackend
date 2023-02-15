namespace Api.Models.ResponseModels
{
	public class LoginResponse
	{
		public int Status { get; set; }
		public Data Data { get; set; }
		public string Message { get; set; }

	}
	public class Data
	{
		public string Token { get; set; }
		public string Role { get; set; }
	}

}
