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
        private IDistrictStaffDB DistrictStaffDb { get; }
        private StaffDB StaffDb { get; }
        public OrderManager(IConfiguration conf)
        {
            OrderDb = new OrderDB(conf);
            DistrictStaffDb = new DistrictStaffDB(conf);
            StaffDb = new StaffDB(conf);
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

        public void CancelOrder(Customer customer, int orderId, string codeToCancel)
        {
            Order order = OrderDb.GetOrder(orderId);

            var output = String.Concat(orderId, customer.NAME, customer.SURNAME);
            if (output.Equals(codeToCancel))
            {
                Console.WriteLine("Canceld");
                OrderDb.ArchiveDelivery(order, "canceled");
            }
        }

        public List<Staff> AssignStaff(int idOrder)
        {
            Order order = OrderDb.GetOrder(idOrder);


            List<DistrictStaff> listDS = DistrictStaffDb.GetDistrictStaffs();
            List<Staff> listStaff = new List<Staff>();
            List<Staff> listStaffs = StaffDb.GetStaffs();

            foreach (var districtstaff in listDS)
            {
                if(order.IDDISTRICT == districtstaff.IDDISTRICT)
                {
                    listStaff.Add(StaffDb.GetStaff(districtstaff.IDSTAFF));
                }

                
            }

            List<Staff> listStaff2 = new List<Staff>();

            for (int i = 0; i < listStaff.Count; i++)
            {
                if(listStaff[i].ORDERCURRENTTOTAL <= 5)
                {
                    listStaff2.Add(listStaff[i]);
                }
            }

            

            return listStaff2;
         

        }

        public Order InsertOrder(Order order)
        {
            return OrderDb.InsertOrder(order);
        }


    }
}
