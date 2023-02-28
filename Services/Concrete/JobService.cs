using Core.Entities;
using DataAccess.Abstract;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
	public class JobService : IJobService
	{
		private readonly IJobRepository _jobRepository;

		public JobService(IJobRepository jobRepository)
		{
			_jobRepository = jobRepository;
		}

		public async Task<int> Add(Job job)
		{
			return await _jobRepository.Add(job);
		}

		public async Task<int> Delete(int id)
		{
			var entity=await GetById(id);
			return await _jobRepository.Delete(entity);
		}

		public async Task<List<Job>> GetAll()
		{
			return await _jobRepository.GetAll();
		}

		public async Task<Job> GetById(int id)
		{
			return await _jobRepository.Get(x=>x.Id==id);
		}
		public async Task<Job> GetByWorkId(int id)
		{
			return await _jobRepository.Get(x => x.WorkId == id);
		}

		public Task<int> Update(Job job)
		{
			return _jobRepository.Update(job);
   		}

		public async Task<List<Job>> GetUserJobs(int userId,DateTime startDate,DateTime endDate)
		{
			return await _jobRepository.GetAll(x=>x.UserId==userId && x.Date>=startDate && x.Date<=endDate);
		}

		public async Task<Job> GetByWorkIdWorkType(int userId, int workId, int workType)
		{
			return await _jobRepository.Get(x => x.UserId == userId && x.WorkId == workId && x.WorkType == workType);
		}
	}
}
