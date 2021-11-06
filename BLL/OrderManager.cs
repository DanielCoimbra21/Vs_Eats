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

        public void CancelOrder(Customer customer, int orderId, string codeToValidate)
        {
            //Order sera utilisé plus tard pour l'archivage
            Order order = OrderDb.GetOrder(orderId);
            DateTime timeNow = DateTime.Now;
            long msTN = timeNow.Millisecond;
            long msOD = order.DELIVERTIME.Millisecond;
            long threeHours = convertHoursToMiliseconds(3);

            //Condition vérification si l'heure de l'ordre et la l'heure d'aujourd'hui
            //est plus grande que 3 heures
            if(msTN- msOD > threeHours)
            {
                var codeToCancel = String.Concat(orderId, customer.NAME, customer.SURNAME);
                if (codeToCancel.Equals(codeToValidate))
                {
                    Console.WriteLine("Canceled");
                    OrderDb.ArchiveDelivery(order, "canceled");
                }
            }
        }

        public List<Staff> AssignStaff(int idOrder)
        {
            //Déclaration variables
            List<Staff> listStaff = new List<Staff>();
            List<Staff> listStaffUpdate = new List<Staff>();
            List<DistrictStaff> listOfStaffDistrict = DistrictStaffDb.GetDistrictStaffs();
            List<Order> listOrders = OrderDb.GetOrders();
            List<int> listCpt = new List<int>();
            Order order = OrderDb.GetOrder(idOrder);
            var cpt = 0;
            int min = 4;


            //Variable pour déterminer la plage horaire de 30 minutes
            DateTime dateTimeOrder = order.DELIVERTIME;
            DateTime dateTimeOrderBefore = dateTimeOrder.Subtract(new TimeSpan(0, 15, 0));
            DateTime dateTimeOrderAfter = dateTimeOrder.Add(new TimeSpan(0, 15, 0));


            //Rechercher les staffs qui travaillent dans la même région que l'order
            //listStaff = searchStaffByDistrict();
            foreach (var ds in listOfStaffDistrict)
            {
                if (order.IDDISTRICT == ds.IDDISTRICT)
                {
                    listStaff.Add(StaffDb.GetStaff(ds.IDSTAFF));
                }
            }



            //Rechercher dans les staffs sélectionnés lesquels ont moins de 5 ordres
            //et dans la tranche horaire de 30 minutes
            for (int i = 0; i < listStaff.Count; i++)
            {
                Console.WriteLine("Cpt de " + listStaff[i].NAMESTAFF);
                cpt = 0;
                foreach (var orderByStaff in listOrders)
                {
                    if (orderByStaff.STATUS.Equals("ongoing") && orderByStaff.IDSTAFF.Equals(listStaff[i].IDSTAFF))
                    {
                        if (orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderBefore) == 1 && orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderAfter) == -1)
                        {
                            cpt++;
                        }
                    }
                }


                //Si compteur du staff plus que 5, on enlève le staff de la liste
                if (cpt >= 5)
                {
                    Console.WriteLine("Cpt de " + listStaff[i].NAMESTAFF + " " + cpt + " enlevé");
                    //listCpt.Add(i);
                    //listStaff.RemoveAt(i);
                }
                else
                {
                    if (cpt <= min)
                    {
                        min = cpt;
                        Console.WriteLine("Minium:" + min);
                        listStaffUpdate.Add(listStaff[i]);
                    }
                    else
                    {
                        //listCpt.Add(i);
                        //listStaff.RemoveAt(i);
                    }
                }
            }
           

            //Vérification si la liste est vide
            //il faudra dire que la livraison n'est pas possible à cette heure-ci
            Console.WriteLine("Longueur tableau " + listStaff.Count);
            if (listStaffUpdate.Count == 0)
            {
                Console.WriteLine("Aucun livreur n'est disponible pour cette heure-ci");
                return null;
            }
            //Vérifier la longueur de la liste Staff si 1, cela veut dire que l'on a qu'un
            //seul livreur disponible sinon on vérifie la valeur ORDERCURRENTOTAL
            var minOrderTotal = listStaffUpdate[0].ORDERCURRENTTOTAL;
            List<int> listToRemove = new List<int>();
            List<Staff> listStaffLastOfThem = new List<Staff>();

            for (int j = 0; j < listStaff.Count; j++)
            {
                if (listStaffUpdate[j].ORDERCURRENTTOTAL > minOrderTotal)
                {
                    listStaffLastOfThem.Add(listStaff[j]);
                    //listStaff.RemoveAt(j);
                }
            }

            return listStaffLastOfThem;

        }



        public Order InsertOrder(Order order)
        {

            return OrderDb.InsertOrder(order);
        }

        public long convertHoursToMiliseconds(int hours)
        {
            long milliseconds;
            return milliseconds = hours * 60 * 60 * 1000;
        }


    }
}
