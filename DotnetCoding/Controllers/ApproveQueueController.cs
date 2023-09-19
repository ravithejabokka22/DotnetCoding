using DotnetCoding.Core.DtoModels;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Intrinsics.X86;

namespace DotnetCoding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveQueueController : ControllerBase
    {
        public readonly IApprovalQueueService _approveService;
        public ApproveQueueController(IApprovalQueueService approveService)
        {
            _approveService = approveService;
        }

        /// <summary>
        /// Get the list of product
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllApprovals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetApprovalList()
        {
            //7.User should be able to see all the products in approval queue along with product name,
            var approveDetailsList = await _approveService.GetAllApprovalDetails();
            if (approveDetailsList == null)
            {
                return NotFound();
            }
            return Ok(approveDetailsList);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] ApprovalModelDto data)
        //{
        //    if (data.ProductPrice <= 10000)
        //    {
        //        await _approveService.CreateApproval(new ApprovalDetails()
        //        {
        //            ProductName = data.ProductName,
        //            ProductDescription = data.ProductDescription,
        //            ProductPrice = data.ProductPrice,
        //            ProductStatus = data.ProductStatus,
        //            CreatedDate = DateTime.Now,
        //            ApproveStatus= data.ApproveStatus,
        //            RequestReason= data.RequestReason
        //        });
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest("Product price should not be more than 10000");
        //    }
        //}

        [HttpPut("ApprovedRejected")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update( int id, string status)
        {
            //9. Product will remain in its original state in case of rejection in the queue
            _approveService.UpdateApproval(id, status);
            
            return Ok();
        }

        //[HttpDelete]
        //public async Task<IActionResult> Delete(int Id)
        //{
        //    _approveService.DeleteApproval(Id);
        //    return Ok();
        //}
    }
}
