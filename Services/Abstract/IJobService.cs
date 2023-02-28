using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
	public interface IJobService
	{
		Task<int> Add(Job job);
		Task<int> Update(Job job);
		Task<int> Delete(int id);
		Task<Job> GetById(int id);
		Task<Job> GetByWorkId(int id);
		Task<Job> GetByWorkIdWorkType(int userId,int workId,int workType);
		Task<List<Job>> GetAll();
		Task<List<Job>> GetUserJobs(int userId, DateTime startDate, DateTime endDate);
	}
}
