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
            //Déclaration des variables, objets
            Order order1 = OrderDb.GetOrder(order.IDORDER);
           
            OrderDb.ArchiveDelivery(order1, status);
            
            /*
             * Lorsqu'on archive une commande qui est livré
             * on ajoute 1 au ordercurrentotal du staff qui l'a livré
             */
            StaffDb.UpdateCurrentTotal(order1.IDSTAFF);
        }

        public void CancelOrder(Customer customer, int orderId, string codeToValidate)
        {
            //Order sera utilisé plus tard pour l'archivage
            Order order = OrderDb.GetOrder(orderId);
            DateTime timeNow = DateTime.Now;
            long msTN = timeNow.Millisecond;
            long msOD = order.DELIVERTIME.Millisecond;
            long threeHours = ConvertHoursToMiliseconds(3);

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

        public int AssignStaff(int idOrder)
        {
            //Déclaration variables
            Order order = OrderDb.GetOrder(idOrder);
            List<Staff> listStaff = new List<Staff>();

            /*
             * Appel méthode recherche staff par district
             */
            listStaff = SearchStaffByDistrict(order);


            /*
             * Appel méthode mise à jour du staff avec condition < 5
             * ainsi que vérification si il y a des staff qui on un compteur plus bas
             * que 5 -> pour mieux répartir les livreurs selon le nombre de commande
            */
            List<Staff> listStaffUpdate = SearchStaffByTime(listStaff, order.DELIVERTIME);


            /*
             * Vérification si la liste est vide
             * si oui retour de la valeur -1
            */
            if (listStaffUpdate.Count == 0)
            {
                Console.WriteLine("Aucun livreur n'est disponible pour cette heure-ci");
                return -1;
            }


            /*
             * Dernière vérification dans la list staff, obtenir les staffs qui ont le moins livré
             * dans selon le ordeur current total
             * seul livreur disponible sinon on vérifie la valeur ORDERCURRENTOTAL
            */
            int idStaff = VerifyCurrentOrder(listStaffUpdate);

            return idStaff;

        }

        private int VerifyCurrentOrder(List<Staff> listStaffUpdate)
        {
            int idStaff;
            var minOrderTotal = listStaffUpdate[0].ORDERCURRENTTOTAL;
            List<Staff> listStaffLastOfThem = new List<Staff>();

            for (int j = 0; j < listStaffUpdate.Count; j++)
            {
                if (listStaffUpdate[j].ORDERCURRENTTOTAL <= minOrderTotal)
                {
                    listStaffLastOfThem.Add(listStaffUpdate[j]);
                    minOrderTotal = listStaffUpdate[j].ORDERCURRENTTOTAL;
                }
            }
            //Retourne la dernière valeur du tableau car elle sera la plus petite selon la condition
            return idStaff = listStaffLastOfThem[listStaffLastOfThem.Count-1].IDSTAFF;
        }


        private List<Staff> SearchStaffByDistrict(Order order)
        {
            //Déclaration des variables
            List<DistrictStaff> listOfStaffDistrict = DistrictStaffDb.GetDistrictStaffs();
            List<Staff> listStaff = new List<Staff>();

            //Rechercher les staffs qui travaillent dans la même région que l'order 
            foreach (var ds in listOfStaffDistrict)
            {
                if (order.IDDISTRICT == ds.IDDISTRICT)
                {
                    listStaff.Add(StaffDb.GetStaff(ds.IDSTAFF));
                }
            }

            return listStaff;
        }

        private List<Staff> SearchStaffByTime(List<Staff> listStaff,DateTime deliverTime)
        {
            //Déclaration variable
            var cpt = 0;
            int max = 5;
            List<Order> listOrders = OrderDb.GetOrders();
            List<Staff> results = new List<Staff>();
            DateTime dateTimeOrder = deliverTime;
            DateTime dateTimeOrderBefore = dateTimeOrder.Subtract(new TimeSpan(0, 15, 0));
            DateTime dateTimeOrderAfter = dateTimeOrder.Add(new TimeSpan(0, 15, 0));


            //Rechercher dans les staffs sélectionnés lesquels ont moins de 5 ordres
            //et dans la tranche horaire de 30 minutes
            for (int i = 0; i < listStaff.Count; i++)
            {
                //Réinitialisation du compteur à zéro pour chaque staff de la liste
                cpt = 0;
                foreach (var orderByStaff in listOrders)
                {
                    /*2 conditions
                     * 1. Vérifier le statut qu'ils soient "ongoing" et idStaff recherché = idStaff dans la list
                     * 2. Vérifier les heures des commandes entre -15min et +15min
                    */
                    if (orderByStaff.STATUS.Equals("ongoing") && orderByStaff.IDSTAFF.Equals(listStaff[i].IDSTAFF))
                    {
                        if (orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderBefore) == 1 && orderByStaff.DELIVERTIME.CompareTo(dateTimeOrderAfter) == -1)
                        {
                            cpt++;
                        }
                    }
                }

                if(cpt < max)
                {
                    max = cpt;
                    results.Add(listStaff[i]);
                }
            }
            return results;
        }



        public Order InsertOrder(Order order)
        {
            return OrderDb.InsertOrder(order);
        }

        private long ConvertHoursToMiliseconds(int hours)
        {
            long milliseconds;
            return milliseconds = hours * 60 * 60 * 1000;
        }

        public void AddTime(Order order)
        {
            List<Order> orderList = OrderDb.GetOrders();
            
            foreach(var o in orderList)
            {
                //checker si c'est la bonne ORDER grace à l'ID
                if(o.IDORDER.Equals(order.IDORDER))
                {
                    //on va rajouter 15 minutes à la commande à chaque fois qu'elle clique sur AddTime
                    order.DELIVERTIME = order.DELIVERTIME.Add(new TimeSpan(0, 15, 0));
                    Console.WriteLine("15 minutes Added successfuly");
                }
            }

            //retourner l'ordre mise à jour
            //return order;
        }



    }
}
