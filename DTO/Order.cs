using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Order
    {
        public int IDORDER { get; set; }
        public int IDDISTRICT { get; set; }
        public int IDRESTAURANT { get; set; }
        public int IDSTAFF { get; set; }
        public int IDCUSTOMER { get; set; }
        public int TOTALPRICE { get; set; }
        public int DELIVERTIME { get; set; }
        public int STATUS { get; set; }

        public override string ToString()
        {
            return "IdOrder : " + IDORDER + "\n" +
            "IdDistrict : " + IDDISTRICT + "\n" +
            "IdRestaurant : " + IDRESTAURANT + "\n" +
            "IdStaff : " + IDSTAFF + "\n" +
            "IdCustomer : " + IDCUSTOMER + "\n" +
            "TotalPrice : " + TOTALPRICE + "\n" +
            "DeliverTime : " + DELIVERTIME + "\n" +
            "Status : " + STATUS + "\n";
        }
    }
}
