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
    public class OrderManager : IOrderManager
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

        public List<Order> GetOrders(int idStaff)
        {
            return OrderDb.GetOrders(idStaff);
        }

        public List<Order> GetOrders(int idStaff, string status)
        {
            return OrderDb.GetOrders(idStaff,status);
        }

        public List<Order> GetCustomerOrders(int idCustomer)
        {
            return OrderDb.GetCustomerOrders(idCustomer);
        }

        public Order GetOrder(int orderId)
        {
            return OrderDb.GetOrder(orderId);
        }

        /// <summary>
        /// Method to archive the delivery from staff
        /// </summary>
        /// <param name="order"></param>
        /// <param name="status"></param>
        public void ArchiveDelivery(Order order, string status)
        {
            //Get the order we want to modify
            Order order1 = OrderDb.GetOrder(order.IDORDER);
           
            OrderDb.ArchiveDelivery(order1, status);
            
            /*
             * When we archive an order that is delivered
             * add one to the currentTotal
             * It is helpful when we assign the staff
             */
            StaffDb.UpdateCurrentTotal(order1.IDSTAFF);
        }


        /// <summary>
        /// Method to cancel the order
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="codeToValidate"></param>
        /// <param name="orderId"></param>
        public bool CancelOrder(Customer customer, string codeToValidate, int orderId)
        {
            Order order = OrderDb.GetOrder(orderId);

            /*
             * To cancel, we have to verify the name and surname that we entered
             * with the information from the database
            */
            var codeToCancel = String.Concat(orderId, customer.NAME, customer.SURNAME);
                if (codeToCancel.Equals(codeToValidate))
                {
                    OrderDb.ArchiveDelivery(order, "canceled");
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Method to choose which staff will delivered the order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int AssignStaff(Order order)
        {
            List<Staff> listStaff = new List<Staff>();


            //Get the staff that are in the same district as the restaurant
            listStaff = SearchStaffByDistrict(order);


            //Get a new list of staff with staff that have less than 5 orders
            List<Staff> listStaffUpdate = SearchStaffByTime(listStaff, order.DELIVERTIME);


            /*
             * Check if the list is empty, then return -1
            */
            if (listStaffUpdate.Count == 0)
            {
                return -1;
            }


            //Last verification to choose which one has the less order
            int idStaff = VerifyCurrentOrder(listStaffUpdate);


            return idStaff;
        }

        /// <summary>
        /// Method to control the number of order by each staff
        /// </summary>
        /// <param name="listStaffUpdate"></param>
        /// <returns></returns>
        private int VerifyCurrentOrder(List<Staff> listStaffUpdate)
        {
            var minOrderTotal = listStaffUpdate[0].ORDERCURRENTTOTAL;

            List<Staff> listStaffLastOfThem = new List<Staff>();

            for (int i = 0; i < listStaffUpdate.Count; i++)
            {
                if (listStaffUpdate[i].ORDERCURRENTTOTAL <= minOrderTotal)
                {
                    listStaffLastOfThem.Add(listStaffUpdate[i]);
                    minOrderTotal = listStaffUpdate[i].ORDERCURRENTTOTAL;
                }
            }

            /*
             * Returns the last value of the array as it will be the smallest
             * the smallest value is always added at the end
             */
            return listStaffLastOfThem[listStaffLastOfThem.Count-1].IDSTAFF;
        }

        /// <summary>
        /// Method to select the staff who work in the same disctict as the order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private List<Staff> SearchStaffByDistrict(Order order)
        {
            List<DistrictStaff> listOfStaffDistrict = DistrictStaffDb.GetDistrictStaffs();
            List<Staff> listStaff = new List<Staff>();

            //Search the staff that are working in the same district as the order 
            foreach (var staff in listOfStaffDistrict)
            {
                if (order.IDDISTRICT == staff.IDDISTRICT)
                {
                    listStaff.Add(StaffDb.GetStaff(staff.IDSTAFF));
                }
            }
            return listStaff;
        }

        /// <summary>
        /// Method to select the staff with less than 5 commands during 30 minutes
        /// </summary>
        /// <param name="listStaff"></param>
        /// <param name="deliverTime"></param>
        /// <returns></returns>
        private List<Staff> SearchStaffByTime(List<Staff> listStaff,DateTime dateTimeOrder)
        {
            //var cpt = 0;
            //int max = 5;
            //List<Order> listOrders = OrderDb.GetOrders();
            //List<Staff> results = new List<Staff>();
            //DateTime dateTimeOrderBefore = dateTimeOrder.Subtract(new TimeSpan(0, 15, 0));
            //DateTime dateTimeOrderAfter = dateTimeOrder.Add(new TimeSpan(0, 15, 0));

            ///*
            // * Search in the listStaff the one with less than 5 orders in the scale of 30 minutes
            //*/
            //for (int i = 0; i < listStaff.Count; i++)
            //{
            //    //Cpt back to zero everytime for each staff in the list
            //    cpt = 0;
            //    foreach (var orderByStaff in listOrders)
            //    {
            //        /*
            //         * 2 conditions : 
            //         * 1. Statut has to be ongoing and idStaff searched = idStaff in the list
            //         * Control the hour of each command between -15min et +15min and the deliverytime entered
            //        */
            //    if (orderByStaff.STATUS.Equals("ongoing") && orderByStaff.IDSTAFF.Equals(listStaff[i].IDSTAFF))
            //        {
            //            if(orderByStaff.DELIVERTIME == dateTimeOrder || orderByStaff.DELIVERTIME == dateTimeOrderAfter || orderByStaff.DELIVERTIME == dateTimeOrderBefore){
            //                cpt++;
            //            }
            //        }
            //    }


            //    // Verify the cpt et update the variable max, max value will not be more thant 5
            //    if(cpt < max)
            //    {
            //        max = cpt;
            //        results.Add(listStaff[i]);
            //    }

            var cpt = 0;
            int max = 5;
            List<Order> listOrders = OrderDb.GetOrders();
            List<Staff> results = new List<Staff>();
            DateTime dateTimeOrderAfter2 = dateTimeOrder.Add(new TimeSpan(0, 30, 0));
            DateTime dateTimeOrderAfter = dateTimeOrder.Add(new TimeSpan(0, 15, 0));

            /*
            * Search in the listStaff the one with less than 5 orders in the scale of 30 minutes
            */
            for (int i = 0; i < listStaff.Count; i++)
                {
                    //Cpt back to zero everytime for each staff in the list
                    cpt = 0;
                    foreach (var orderByStaff in listOrders)
                    {
                        /*
                         * 2 conditions : 
                         * 1. Statut has to be ongoing and idStaff searched = idStaff in the list
                         * Control the hour of each command between -15min et +15min and the deliverytime entered
                        */
                        if (orderByStaff.STATUS.Equals("ongoing") && orderByStaff.IDSTAFF.Equals(listStaff[i].IDSTAFF))
                        {
                            if (orderByStaff.DELIVERTIME == dateTimeOrder || orderByStaff.DELIVERTIME == dateTimeOrderAfter || orderByStaff.DELIVERTIME == dateTimeOrderAfter2)
                            {
                                cpt++;
                            }
                        }
                    }


                    // Verify the cpt et update the variable max, max value will not be more thant 5
                    if (cpt < max)
                    {
                        max = cpt;
                        results.Add(listStaff[i]);
                    }
                }
            return results;
        }

        public void UpdateOrder(Order order)
        {
            OrderDb.UpdateOrder(order);
        }

        public Order InsertOrder(Order order, int idStaff)
        {
            return OrderDb.InsertOrder(order, idStaff);
        }

        public long ConvertHoursToMiliseconds(int hours)
        {
            long milliseconds;
            return milliseconds = hours * 60 * 60 * 1000;
        }
    }
}
