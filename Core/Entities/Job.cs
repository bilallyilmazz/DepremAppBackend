using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Job
	{
		public int Id { get; set; }
		public int WorkId { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }
		public string Note { get; set; }
		public DateTime Date { get; set; }
		public int WorkType { get; set; }
		public int UserId { get; set; }
	}
}
