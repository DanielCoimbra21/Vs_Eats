using DTO;
using System.Collections.Generic;


namespace DAL
{
    public interface IOrderDB
    {

        List<Order> GetOrders();
        void ArchiveDelivery(Order order, string status);
        Order GetOrder(int orderId);

        Order InsertOrder(Order order);

        void AddTime(Order order);

        List<Order> GetOrders(int idStaff);
    }
}
