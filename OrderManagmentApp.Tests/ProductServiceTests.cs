using NUnit.Framework;
using OrderManagementApp.Models;
using OrderManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OrderManagmentApp.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
           
            _productService = new ProductService();
            _productService.GetAllProducts().ToList().ForEach(p => _productService.RemoveProduct(p.Id));
        }

        [Test]
        public void AddProduct_ShouldAddProduct_WhenProductIsValid()
        {
            
            var product = new Product { Name = "Test Product", Price = 100m };

            bool result = _productService.AddProduct(product);

            Assert.IsTrue(result);
            var products = _productService.GetAllProducts();
            Assert.AreEqual(1, products.Count());
            Assert.AreEqual("Test Product", products.First().Name);
        }

        [Test]
        public void AddProduct_ShouldNotAddProduct_WhenProductNameIsDuplicate()
        {
           
            var product1 = new Product { Name = "Test Product", Price = 100m };
            var product2 = new Product { Name = "Test Product", Price = 200m };

            _productService.AddProduct(product1);
            bool result = _productService.AddProduct(product2);
            Assert.IsFalse(result);
            var products = _productService.GetAllProducts();
            Assert.AreEqual(1, products.Count());
        }

        [Test]
        public void RemoveProduct_ShouldRemoveProduct_WhenProductExists()
        {
            
            var product = new Product { Name = "Test Product", Price = 100m };
            _productService.AddProduct(product);
            var addedProduct = _productService.GetAllProducts().First();
            _productService.RemoveProduct(addedProduct.Id);
            var products = _productService.GetAllProducts();
            Assert.AreEqual(0, products.Count());
        }

        [Test]
        public void RemoveProduct_ShouldDoNothing_WhenProductDoesNotExist()
        {
            _productService.RemoveProduct(999);
            var products = _productService.GetAllProducts();
            Assert.AreEqual(0, products.Count());
        }

        [Test]
        public void GetProduct_ShouldReturnProduct_WhenProductExists()
        {
           
            var product = new Product { Name = "Test Product", Price = 100m };
            _productService.AddProduct(product);
            var addedProduct = _productService.GetAllProducts().First();
            var fetchedProduct = _productService.GetProduct(addedProduct.Id);
            Assert.IsNotNull(fetchedProduct);
            Assert.AreEqual("Test Product", fetchedProduct.Name);
        }

        [Test]
        public void GetProduct_ShouldReturnNull_WhenProductDoesNotExist()
        {
           
            var product = _productService.GetProduct(999);
            Assert.IsNull(product);
        }
    }
}
