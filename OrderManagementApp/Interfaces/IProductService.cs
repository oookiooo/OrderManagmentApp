﻿using OrderManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp.Interfaces
{
    public interface IProductService
    {
        bool AddProduct(Product product);
        void RemoveProduct(int productId);
        Product GetProduct(int productId);
        IEnumerable<Product> GetAllProducts();
    }

}
