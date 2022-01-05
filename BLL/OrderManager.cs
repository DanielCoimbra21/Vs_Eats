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
        /// Méthode pour l'archivage des livraisons du staff
        /// </summary>
        /// <param name="order"></param>
        /// <param name="status"></param>
        public void ArchiveDelivery(Order order, string status)
        {
            //Récupération de l'ordre que l'on souhaite archiver
            Order order1 = OrderDb.GetOrder(order.IDORDER);
           
            OrderDb.ArchiveDelivery(order1, status);
            
            /*
             * Lorsqu'on archive une commande qui est livré
             * on ajoute 1 au ordercurrentotal du staff qui l'a livré
             * Cela est utile pour la répartition des livraisons
             */
            StaffDb.UpdateCurrentTotal(order1.IDSTAFF);
        }


        /// <summary>
        /// Méthode pour l'annulation d'un ordre
        /// Objet customer correspond au customer de l'ordre que l'on veut annuler
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="codeToValidate"></param>
        /// <param name="orderId"></param>
        public bool CancelOrder(Customer customer, string codeToValidate, int orderId)
        {
            Order order = OrderDb.GetOrder(orderId);

            /*
             * Pour l'annulation, vérifier que nom, prénom entrer dans la page
             * est équivalent à l'ordre récupéré
            */
            var codeToCancel = String.Concat(orderId, customer.NAME, customer.SURNAME);
                if (codeToCancel.Equals(codeToValidate))
                {
                    OrderDb.ArchiveDelivery(order, "canceled");
                    return true;
                }

            return false;
        }

        public int AssignStaff(Order order)
        {
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
             * Vérification si de la liste
             * Si la liste est vide -> retour de la valeur -1
            */
            if (listStaffUpdate.Count == 0)
            {
                return -1;
            }


            /*
             * Dernière vérification dans la liste staff, obtenir les staffs qui ont le moins livré
             * dans selon le OrderCurrentTotal
             * seul livreur disponible sinon on vérifie la valeur ORDERCURRENTOTAL
            */
            int idStaff = VerifyCurrentOrder(listStaffUpdate);


            return idStaff;
        }

        /// <summary>
        /// Méthode pour contrôler le nombre d'ordre total de chaque livreur
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
             * Retourne la dernière valeur du tableau car elle sera la plus petite
             * la plus petite valeur est toujours ajouté à la fin
             * si égalité la dernière valeure est renvoyé
             */
            return listStaffLastOfThem[listStaffLastOfThem.Count-1].IDSTAFF;
        }


        /// <summary>
        /// Méthode pour sélectionner le staff qui est dans la région des restaurants
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private List<Staff> SearchStaffByDistrict(Order order)
        {
            List<DistrictStaff> listOfStaffDistrict = DistrictStaffDb.GetDistrictStaffs();
            List<Staff> listStaff = new List<Staff>();

            //Rechercher les staffs qui travaillent dans la même région que la nouvelle commande 
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
        /// Méthode pour sélectionner les staff qui ont moins de 5 commandes en une demi-heure
        /// </summary>
        /// <param name="listStaff"></param>
        /// <param name="deliverTime"></param>
        /// <returns></returns>
        private List<Staff> SearchStaffByTime(List<Staff> listStaff,DateTime dateTimeOrder)
        {
            var cpt = 0;
            int max = 5;
            List<Order> listOrders = OrderDb.GetOrders();
            List<Staff> results = new List<Staff>();
            DateTime dateTimeOrderBefore = dateTimeOrder.Subtract(new TimeSpan(0, 15, 0));
            DateTime dateTimeOrderAfter = dateTimeOrder.Add(new TimeSpan(0, 15, 0));

            /*
             * Rechercher dans les staffs sélectionnés lesquels ont moins de 5 ordres
             * et dans la tranche horaire de 30 minutes
            */
            for (int i = 0; i < listStaff.Count; i++)
            {
                //Réinitialisation du compteur à zéro pour chaque staff de la liste
                cpt = 0;
                foreach (var orderByStaff in listOrders)
                {
                    /*Deux conditions
                     * 1. Vérifier le statut qu'ils soient "ongoing" et idStaff recherché = idStaff dans la liste
                     * 2. Vérifier les heures des commandes entre -15min et +15min et l'heure elle-même
                    */
                if (orderByStaff.STATUS.Equals("ongoing") && orderByStaff.IDSTAFF.Equals(listStaff[i].IDSTAFF))
                    {
                        if(orderByStaff.DELIVERTIME == dateTimeOrder || orderByStaff.DELIVERTIME == dateTimeOrderAfter || orderByStaff.DELIVERTIME == dateTimeOrderBefore){
                            cpt++;
                        }
                    }
                }


                /*
                 * Vérifier le compteur et mise à jour du max, le max ne dépassera pas le 5
                 */
                if(cpt < max)
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
