using OrderManagementApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp.Controllers
{
    public class OrderController
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public void AddProductToOrder(int productId, int quantity)
        {
            _orderService.AddOrderItem(productId, quantity);
        }

        public void RemoveProductFromOrder(int productId)
        {
            _orderService.RemoveOrderItem(productId);
        }

        public decimal GetOrderTotal()
        {
            return _orderService.GetOrderTotal();
        }

        public List<(string ProductName, int Quantity, decimal Price)> GetOrderItems()
        {
            var orderItems = _orderService.GetCurrentOrder().OrderItems;

            var result = orderItems.Select(oi => (
                ProductName: oi.Product.Name,
                Quantity: oi.Quantity,
                Price: oi.Product.Price
            )).ToList();

            return result;
        }
    }
}

