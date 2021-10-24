using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Customer
    {

        public int IdCustomer { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }

        public string toString()
        {
            return "IdCustomer " + IdCustomer
                + "Name" + Name
                + "Surname" + Surname
                + "Username" + Username
                + "Phone" + Phone
                + "Address" + Address
                + "Mail" + Mail
                + "Password" + Password;
        }
    }
}
