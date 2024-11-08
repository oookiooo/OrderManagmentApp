using OrderManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp.Interfaces
{
    public interface IOrderService
    {
        void AddOrderItem(int productId, int quantity);
        void RemoveOrderItem(int productId);
    }
}
