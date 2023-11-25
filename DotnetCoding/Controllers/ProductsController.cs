using Microsoft.AspNetCore.Mvc;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using DotnetCoding.Core.DtoModels;

namespace DotnetCoding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get the list of product
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductList()
        {
            var productDetailsList = await _productService.GetAllProducts();
            //
            if(productDetailsList == null)
            {
                return NotFound();
            }
            return Ok(productDetailsList);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductModelDto data)
        {
            if(data.ProductPrice <= 10000)
            {
                await _productService.CreateProduct(new ProductDetails()
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    ProductPrice = data.ProductPrice,
                    ProductStatus =data.ProductStatus,
                     CreatedDate= DateTime.Now
                });
                return Ok();
            }
            else
            {
                return BadRequest("Product price should not be more than 10000");
            }            
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductModelDto data)
        {
             _productService.UpdateProduct(new ProductDetails() 
                                            { 
                                                
                                                ProductName = data.ProductName, 
                                                ProductDescription = data.ProductDescription, 
                                                ProductPrice = data.ProductPrice, 
                                                ProductStatus = data.ProductStatus
                                            });
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            _productService.DeleteProduct(Id);
            return Ok();
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchProduct([FromBody] Dictionary<string,string> seachItems)
        {
            
            var result= await _productService.SearchProducts(seachItems);
            if (result == null)
            {
                return NotFound();
            }else
            return Ok(result);
        }
    }
}
