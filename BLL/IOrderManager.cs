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

        public List<Order> GetOrders();

        public Order GetOrder(int orderId);

        public void ArchiveDelivery(Order order, string status);

        public void CancelOrder(Customer customer, int orderId, string codeToValidate);

        public int AssignStaff(int idOrder);

        //public int VerifyCurrentOrder(List<Staff> listStaffUpdate);
     
        //public List<Staff> SearchStaffByDistrict(Order order);

        //public List<Staff> SearchStaffByTime(List<Staff> listStaff, DateTime deliverTime);

        public Order InsertOrder(Order order);

        //public long ConvertHoursToMiliseconds(int hours);
        public void AddTime(Order order);
      



    }

}

   

