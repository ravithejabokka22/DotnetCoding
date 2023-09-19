using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using Microsoft.VisualBasic;

namespace DotnetCoding.Services
{
    public class ProductService : IProductService
    {
        public IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateProduct(ProductDetails product)
        {
            if (product.ProductPrice > 5000) // 4. Any product should be pushed to approval queue if its price is more than 5000 dollars at the time of creation and update.
            {
                await MoveToApproval(product, "price is more than 5000 dollars","Create");
                
            }
            else
            {
                await _unitOfWork.Products.Create(product);
                _unitOfWork.Save();
            }
        }

        public async void DeleteProduct(int Id)
        {
            //6. Product should be pushed to approval queue in case delete.
            ProductDetails productDetails = GetProduct(Id);
            if (productDetails != null)
            {
                await MoveToApproval(productDetails, "Approval for Product Delete","Delete");
            }
            //_unitOfWork.Products.Delete(GetProduct(Id));
            //_unitOfWork.Save();
        }

        public async Task<IEnumerable<ProductDetails>> GetAllProducts()
        {
            var productResult= await _unitOfWork.Products.GetAll();

            return productResult.OrderByDescending(d => d.Id).Where(c => c.ProductStatus == "1").ToList();
        }

        public ProductDetails GetProduct(int Id)
        {
            return _unitOfWork.Products.GetAll().Result.First(item => item.Id == Id);
        }

        public async Task<IEnumerable<ProductDetails>> SearchProducts(Dictionary<string, string> searchItems)
        {
            //2. User can search using Product name, Price range and posted date range.
            StringBuilder errorMsg = new StringBuilder();
            string FinalErrorMsg = "";
            List<string> properties = new List<string>
            {
                "ProductName",
                "postedFromdate",
                "postedTodate",
                "PriceFromrange",
                "PriceTorange"
            };

            //User field validation
            foreach (var item in searchItems)
            {
                if (!properties.Contains(item.Key))
                {
                    errorMsg.Append("Invalid Field Name " + item.Key + " ");
                }
            }
            if (!string.IsNullOrEmpty(errorMsg.ToString()))
            {
                FinalErrorMsg = "-8/" + errorMsg;                
                throw new InvalidOperationException(FinalErrorMsg.ToString());
            }

            var productResult = await _unitOfWork.Products.GetAll();

            if (productResult != null && searchItems.ContainsKey("ProductName"))
                productResult = productResult.Where(i => i.ProductName == searchItems["ProductName"]);
            if (productResult != null && searchItems.ContainsKey("postedFromdate") && searchItems.ContainsKey("postedTodate"))
                productResult = productResult.Where(i => i.CreatedDate> DateTime.Parse(searchItems["postedFromdate"]) && i.CreatedDate< DateTime.Parse(searchItems["postedTodate"]));
            if (productResult != null && searchItems.ContainsKey("PriceFromrange") && searchItems.ContainsKey("PriceTorange"))
                productResult = productResult.Where(i => i.ProductPrice > Int32.Parse(searchItems["PriceFromrange"]) && i.ProductPrice< Int32.Parse(searchItems["PriceTorange"]));

            
            return productResult.OrderByDescending(d => d.Id).ToList();

           
        }

        public async void UpdateProduct(ProductDetails product)
        {
            ProductDetails productDetails= GetProduct(product.Id);
            //5. Any product should be pushed to approval queue if its price is more than 50% of its previous price.
            if (productDetails!=null && (product.ProductPrice > (productDetails.ProductPrice+ productDetails.ProductPrice/2) ))
            {
                await MoveToApproval(product, "price is more than 50 percent","Update");

            }
            _unitOfWork.Products.Update(product);
            _unitOfWork.Save();
        }

        private async Task MoveToApproval(ProductDetails product,string reason,string state)
        {
            var data = new ApprovalDetails()
            {
                ProductPrice = product.ProductPrice,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductStatus = product.ProductStatus,
                ApproveStatus = "Pending",
                CreatedDate = DateTime.Now,
                RequestReason = reason,
                State= state
                 
            };
            await _unitOfWork.Approvals.Create(data);
            _unitOfWork.Save();
        }

    }
}
