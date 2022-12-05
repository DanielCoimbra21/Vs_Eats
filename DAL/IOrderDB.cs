using DTO;
using System.Collections.Generic;


namespace DAL
{
    public interface IOrderDB
    {
        List<Order> GetOrders();
        List<Order> GetOrders(int idStaff);
        List<Order> GetOrders(int idStaff, string status);   
        Order GetOrder(int orderId);
        List<Order> GetCustomerOrders(int idCustomer);
        Order InsertOrder(Order order, int idStaff);
        void ArchiveDelivery(Order order, string status);
        void UpdateOrder(Order order);
    }
}
