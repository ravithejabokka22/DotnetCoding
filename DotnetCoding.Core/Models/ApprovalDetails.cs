using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCoding.Core.Models
{
    public class ApprovalDetails
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public string ProductStatus { get; set; }

        public DateTime CreatedDate { get; set; }
        public string ApproveStatus { get; set; }
        public string RequestReason { get; set; }

        public string State { get; set; }

    }
}
