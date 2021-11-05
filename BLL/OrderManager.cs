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
            //Déclaration variables
            List<Staff> listStaff = new List<Staff>();
            List<DistrictStaff> listOfStaffDistrict = DistrictStaffDb.GetDistrictStaffs();
            List<Order> listOrders = OrderDb.GetOrders();
            Order order = OrderDb.GetOrder(idOrder);

            //Variable pour déterminer la plage horaire de 30 minutes
            DateTime dateTimeOrder = order.DELIVERTIME;
            DateTime dateTimeOrderBefore = dateTimeOrder.Subtract(new TimeSpan(0, 15, 0));
            DateTime dateTimeOrderAfter = dateTimeOrder.Add(new TimeSpan(0, 15, 0));


            //Rechercher les staffs qui travaillent dans la même région que l'order
            foreach (var ds in listOfStaffDistrict)
            {
                if (order.IDDISTRICT == ds.IDDISTRICT)
                {
                    listStaff.Add(StaffDb.GetStaff(ds.IDSTAFF));
                }
            }

            var cpt = 0;

            //Rechercher dans les staffs sélectionners lesquels ont moins de 5 ordres
            //et dans la tranche horaire de 30 minutes aux abords du nouvel ordre
            for (int i = 0; i < listStaff.Count; i++)
            {
                cpt = 0;
                foreach (var orderByStaff in listOrders)
                {
                    if (orderByStaff.STATUS.Equals("ongoing") && orderByStaff.IDSTAFF.Equals(listStaff[i].IDSTAFF))
                    {
                        //1 instance later value
                        //-1 instance is earlier
                        Console.WriteLine("compare before: " + orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderBefore));
                        Console.WriteLine("compare after: " + orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderAfter));

                        if (orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderBefore) == 1 && orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderAfter) == -1)
                        {
                            cpt++;
                        }
                    }
                }
                Console.WriteLine("Cpt " + cpt);
                //Si compteur du staff plus que 5, on enlève le staff de la liste
                if (cpt >= 5)
                {
                    listStaff.RemoveAt(i);
                }
            }
            return listStaff;
        }

        public Order InsertOrder(Order order)
        {
            return OrderDb.InsertOrder(order);
        }


    }
}
