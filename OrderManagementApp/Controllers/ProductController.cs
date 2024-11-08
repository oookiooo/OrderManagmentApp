using OrderManagementApp.Interfaces;
using OrderManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp.Controllers
{
    public class ProductController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public bool AddProduct(string name, decimal price)
        {
            var product = new Product { Name = name, Price = price };
            return _productService.AddProduct(product);
        }

        public void RemoveProduct(int productId)
        {
            _productService.RemoveProduct(productId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productService.GetAllProducts();
        }
    }
}
