using OrderManagementApp.Interfaces;
using OrderManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly Order _currentOrder;

        public OrderService()
        {
            _context = new ApplicationDbContext();
            _currentOrder = new Order { OrderItems = new System.Collections.Generic.List<OrderItem>() };
            _context.Orders.Add(_currentOrder);
            _context.SaveChanges();
        }

        public void AddOrderItem(int productId, int quantity)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return;

            var orderItem = _currentOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if (orderItem == null)
            {
                orderItem = new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    OrderId = _currentOrder.Id
                };
                _currentOrder.OrderItems.Add(orderItem);
            }
            else
            {
                orderItem.Quantity += quantity;
            }

            _context.SaveChanges();
        }

        public void RemoveOrderItem(int productId)
        {
            var orderItem = _currentOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if (orderItem != null)
            {
                _currentOrder.OrderItems.Remove(orderItem);
                _context.SaveChanges();
            }
        }

        public decimal GetOrderTotal()
        {
            var items = _currentOrder.OrderItems
            .SelectMany(oi => Enumerable.Repeat(oi, oi.Quantity))
            .OrderBy(oi => oi.Product.Price)
            .ToList();

            decimal totalWithoutDiscounts = items.Sum(oi => oi.Product.Price);

            decimal discount = 0m;

            if (items.Count == 2)
            {
                var secondCheapestPrice = items[0].Product.Price;
                discount = secondCheapestPrice * 0.10m;
            }
            else if (items.Count == 3)
            {
                var thirdCheapestPrice = items[0].Product.Price;
                discount = thirdCheapestPrice * 0.20m;
            }
            else if (items.Count > 3)
            {
                var cheapestPrice = items[0].Product.Price;
                discount = cheapestPrice * 0.20m;
            }

            decimal totalAfterProductDiscount = totalWithoutDiscounts - discount;

            if (totalAfterProductDiscount > 5000m)
            {
                totalAfterProductDiscount *= 0.95m;
            }

            return totalAfterProductDiscount;
        }

        public Order GetCurrentOrder()
        {
            return _currentOrder;
        }
    }
}