using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Customer
    {

        public int IDCUSTOMER { get; set; }
        public int IDCITY { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string USERNAME { get; set; }
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        public string MAIL { get; set; }
        public string PASSWORD { get; set; }

        public override string ToString()
        {
            return "IdCustomer: " + IDCUSTOMER +"\n"
                + "IdCity: " + IDCITY + "\n"
                + "Name: " + NAME + "\n"
                + "Surname: " + SURNAME + "\n"
                + "Username: " + USERNAME + "\n"
                + "Phone: " + PHONE + "\n"
                + "Address: " + ADDRESS + "\n"
                + "Mail: " + MAIL + "\n"
                + "Password: " + PASSWORD +"\n";
        }
    }
}
