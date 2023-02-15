using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
	public class JobRepository : EfEntityRepositoryBase<Job>, IJobRepository
	{
		public JobRepository(DataAccess.Contexts.DepremAppContext context) : base(context)
		{
		}
	}
}
