namespace Api.Models.RequestModels
{
	public class JobRequestModel
	{
		public int WorkId { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }
		public string Note { get; set; }
		public DateTime Date { get; set; }

		public int WorkType { get; set; }
	}
}
