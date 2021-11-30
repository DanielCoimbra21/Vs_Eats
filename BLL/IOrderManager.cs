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
        List<Order> GetOrders();

        Order GetOrder(int orderId);

        void ArchiveDelivery(Order order, string status);

        void CancelOrder(Customer customer, int orderId, string codeToValidate);

        int AssignStaff(int idOrder);

        int VerifyCurrentOrder(List<Staff> listStaffUpdate);

        List<Staff> SearchStaffByDistrict(Order order);

        List<Staff> SearchStaffByTime(List<Staff> listStaff, DateTime deliverTime);

        Order InsertOrder(Order order);

        long ConvertHoursToMiliseconds(int hours);

        void AddTime(Order order);

    }

   
}
