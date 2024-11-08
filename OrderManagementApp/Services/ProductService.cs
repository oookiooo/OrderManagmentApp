using OrderManagementApp.Interfaces;
using OrderManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService()
        {
            _context = new ApplicationDbContext();
            _context.Database.EnsureCreated();
        }

        public bool AddProduct(Product product)
        {
            bool productExists = _context.Products
                .Any(p => p.Name.ToLower() == product.Name.ToLower());

            if (productExists)
            {
                return false;
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return true; 
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProduct(int productId)
        {
            return _context.Products.Find(productId);
        }

        public void RemoveProduct(int productId)
        {
            var product = GetProduct(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }

}
