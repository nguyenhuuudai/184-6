using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBizApplication.model;

namespace MyBizApplication.service
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
    }
}