using DAL;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OrderManager 
    {
        private IOrderDB OrderDb { get; }
        public OrderManager(IConfiguration conf)
        {
            OrderDb = new OrderDB(conf);
        }

        public List<Order> GetOrders()
        {
            return OrderDb.GetOrders();
        }

        public Order GetOrder(int orderId)
        {
            return OrderDb.GetOrder(orderId);
        }

        public void ArchiveDelivery(Order order, string status)
        {
            Order order1 = OrderDb.GetOrder(order.IDORDER);
            OrderDb.ArchiveDelivery(order1, status);
        }

    }
}
