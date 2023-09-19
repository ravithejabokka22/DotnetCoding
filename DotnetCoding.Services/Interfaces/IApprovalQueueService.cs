using DotnetCoding.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCoding.Services.Interfaces
{
    public interface IApprovalQueueService
    {
        ApprovalDetails GetApprovalDetails(int Id);
        Task<IEnumerable<ApprovalDetails>> GetAllApprovalDetails();
        Task CreateApproval(ApprovalDetails product);
        void UpdateApproval(int id, string status);
        void DeleteApproval(int Id);
    }
}
