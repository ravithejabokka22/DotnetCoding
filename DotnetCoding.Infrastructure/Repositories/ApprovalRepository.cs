using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCoding.Infrastructure.Repositories
{
    internal class ApprovalRepository : GenericRepository<ApprovalDetails>, IApprovalQueueRepository
    {
        public ApprovalRepository(DbContextClass dbContext) : base(dbContext)
        {

        }
    }
}
