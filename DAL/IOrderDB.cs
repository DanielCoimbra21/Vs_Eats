using DTO;
using System.Collections.Generic;


namespace DAL
{
    public interface IOrderDB
    {

        List<Order> GetOrders();
        void ArchiveDelivery(Order order, string status);
        Order GetOrder(int orderId);

        Order InsertOrder(Order order, int idStaff);

        List<Order> GetOrders(int idStaff);

        List<Order> GetOrders(int idStaff, string status);

        List<Order> GetCustomerOrders(int idCustomer);

        void UpdateOrder(Order order);
    }
}
