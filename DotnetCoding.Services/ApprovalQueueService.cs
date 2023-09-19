using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCoding.Services
{
    public class ApprovalQueueService : IApprovalQueueService
    {
        public IUnitOfWork _unitOfWork;
        public ApprovalQueueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public async Task CreateApproval(ApprovalDetails approval)
        {
            await _unitOfWork.Approvals.Create(approval);
            _unitOfWork.Save();
        }

        public void DeleteApproval(int Id)
        {
            _unitOfWork.Approvals.Delete(GetApprovalDetails(Id));
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<ApprovalDetails>> GetAllApprovalDetails()
        {
            //7. User should be able to see all the products in approval queue
            var approvalResult = await _unitOfWork.Approvals.GetAll();

            return approvalResult.OrderByDescending(d => d.CreatedDate).ToList();
        }

        public ApprovalDetails GetApprovalDetails(int Id)
        {
            return _unitOfWork.Approvals.GetAll().Result.First(item => item.Id == Id);
        }

        public async void UpdateApproval(int id, string approvestatus)
        {
            //8. Product will be updated with new state (Create, Update, Delete) which should be reflected
            ApprovalDetails approvaldetails = GetApprovalDetails(id);
            approvaldetails.ApproveStatus = approvestatus;
            
             var productDetails = new ProductDetails()
                {
                    //Id= approvaldetails.Id,
                    ProductPrice = approvaldetails.ProductPrice,
                    ProductName = approvaldetails.ProductName,
                    ProductStatus = approvaldetails.ProductStatus,
                    ProductDescription = approvaldetails.ProductDescription,
                    CreatedDate = DateTime.Now                 
                };
            
            
             
            if(approvestatus == "Approved")
            { 

                 
                if(approvaldetails.State=="Create")
                {
                    _unitOfWork.Products.Create(productDetails);
                }
                else if(approvaldetails.State=="Update")
                {
                    _unitOfWork.Products.Update(productDetails);
                }else if (approvaldetails.State == "Delete")
                {
                    _unitOfWork.Products.Delete(productDetails);
                }
                _unitOfWork.Approvals.Update(approvaldetails);
                _unitOfWork.Save();

                DeleteApproval(id);
            }
            else if(approvestatus == "Rejected")
            {
                _unitOfWork.Approvals.Update(approvaldetails);
                _unitOfWork.Save();
            }
            
            
        }
    }
}
