using OrderManagementApp.Models;
using OrderManagementApp.Services;
using NUnit.Framework;
using System.Linq;

namespace OrderManagmentApp.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productService = new ProductService();
            _orderService = new OrderService();

            _productService.GetAllProducts().ToList().ForEach(p => _productService.RemoveProduct(p.Id));
            _productService.AddProduct(new Product { Name = "Product A", Price = 100m });
            _productService.AddProduct(new Product { Name = "Product B", Price = 200m });
            _productService.AddProduct(new Product { Name = "Product C", Price = 300m });
            _productService.AddProduct(new Product { Name = "Product D", Price = 500m });
            _productService.AddProduct(new Product { Name = "Product E", Price = 1000m });
        }

        [Test]
        public void AddOrderItem_ShouldAddItem_WhenProductExists()
        {

            var product = _productService.GetAllProducts().First();

            _orderService.AddOrderItem(product.Id, 2);

            var orderItems = _orderService.GetCurrentOrder().OrderItems;
            Assert.AreEqual(1, orderItems.Count);
            Assert.AreEqual(2, orderItems.First().Quantity);
        }

        [Test]
        public void AddOrderItem_ShouldDoNothing_WhenProductDoesNotExist()
        {

            _orderService.AddOrderItem(999, 1);

            var orderItems = _orderService.GetCurrentOrder().OrderItems;
            Assert.AreEqual(0, orderItems.Count);
        }

        [Test]
        public void RemoveOrderItem_ShouldRemoveItem_WhenItemExists()
        {

            var product = _productService.GetAllProducts().First();
            _orderService.AddOrderItem(product.Id, 1);

            _orderService.RemoveOrderItem(product.Id);

            var orderItems = _orderService.GetCurrentOrder().OrderItems;
            Assert.AreEqual(0, orderItems.Count);
        }

        [Test]
        public void RemoveOrderItem_ShouldDoNothing_WhenItemDoesNotExist()
        {

            _orderService.RemoveOrderItem(999);

            var orderItems = _orderService.GetCurrentOrder().OrderItems;
            Assert.AreEqual(0, orderItems.Count);
        }

        [Test]
        public void GetOrderTotal_ShouldCalculateTotalWithoutDiscounts_WhenNoDiscountApplies()
        {
            var product = _productService.GetAllProducts().First(p => p.Price == 100m);
            _orderService.AddOrderItem(product.Id, 1);
            decimal total = _orderService.GetOrderTotal();
            Assert.AreEqual(100m, total);
        }

        [Test]
        public void GetOrderTotal_ShouldApply10PercentDiscount_OnSecondCheaperProduct()
        {
            var products = _productService.GetAllProducts().OrderBy(p => p.Price).ToList();
            _orderService.AddOrderItem(products[0].Id, 1);
            _orderService.AddOrderItem(products[1].Id, 1);

            decimal total = _orderService.GetOrderTotal();

            Assert.AreEqual(290m, total);
        }

        [Test]
        public void GetOrderTotal_ShouldApply20PercentDiscount_OnThreeCheapestProducts()
        {
            var products = _productService.GetAllProducts().OrderBy(p => p.Price).ToList();
            _orderService.AddOrderItem(products[0].Id, 1);
            _orderService.AddOrderItem(products[1].Id, 1);
            _orderService.AddOrderItem(products[2].Id, 1);

            decimal total = _orderService.GetOrderTotal();

            Assert.AreEqual(580m, total);
        }


        [Test]
        public void GetOrderTotal_ShouldApply20PercentDiscount_OnThreeCheapestProducts_And5PercentAdditionalDiscount_WhenTotalExceeds5000()
        {
            var products = _productService.GetAllProducts().OrderBy(p => p.Price).ToList();
            _orderService.AddOrderItem(products[4].Id, 6);
            decimal total = _orderService.GetOrderTotal();
            Assert.AreEqual(5510m, total);
        }

        [Test]
        public void GetOrderTotal_ShouldApply10PercentDiscount_OnSecondCheaperProduct_And5PercentAdditionalDiscount_WhenTotalExceeds5000()
        {
            var products = _productService.GetAllProducts().OrderBy(p => p.Price).ToList();
            _orderService.AddOrderItem(products[0].Id, 1);
            _orderService.AddOrderItem(products[4].Id, 50);
            decimal total = _orderService.GetOrderTotal();
            Assert.AreEqual(47576m, total);
        }

        [Test]
        public void GetOrderTotal_ShouldNotApplyAnyDiscounts_WhenOrderIsEmpty()
        {
            decimal total = _orderService.GetOrderTotal();
            Assert.AreEqual(0m, total);
        }
        [Test]
        public void GetOrderTotal_ShouldApplyCorrectDiscounts_WithMultipleQuantitiesOfSameProduct()
        {
            var products = _productService.GetAllProducts().OrderBy(p => p.Price).ToList();
            _orderService.AddOrderItem(products[0].Id, 2); 
            _orderService.AddOrderItem(products[1].Id, 1); 
            _orderService.AddOrderItem(products[2].Id, 1); 
            decimal total = _orderService.GetOrderTotal();
            Assert.AreEqual(680m, total);
        }
        [Test]
        public void GetOrderTotal_ShouldApplyCorrectDiscounts_WithMultipleQuantitiesOfSameProduct_AndTotalExceeds5000()
        {
            var products = _productService.GetAllProducts().OrderBy(p => p.Price).ToList();
            _orderService.AddOrderItem(products[0].Id, 2);
            _orderService.AddOrderItem(products[1].Id, 1);
            _orderService.AddOrderItem(products[2].Id, 1);
            _orderService.AddOrderItem(products[3].Id, 3);
            _orderService.AddOrderItem(products[4].Id, 4);
            decimal total = _orderService.GetOrderTotal();
            Assert.AreEqual(5871m, total);
        }
    }
}
