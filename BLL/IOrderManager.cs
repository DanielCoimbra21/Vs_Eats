using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IOrderManager
    {
        public List<Order> GetOrders(int idStaff);

        public List<Order> GetOrders(int idStaff, string status);

        public List<Order> GetCustomerOrders(int idCustomer);

        public long ConvertHoursToMiliseconds(int hours);

        public Order GetOrder(int orderId);

        public void ArchiveDelivery(Order order, string status);

        public bool CancelOrder(Customer customer, string codeToValidate, int orderId);

        public int AssignStaff(Order order);

        public Order InsertOrder(Order order, int idStaff);
      
    }

}

   

