﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Staff
    {
        public int IDSTAFF { get; set; }
        public int IDCITY { get; set; }
        public string NAMESTAFF { get; set; }
        public string SURNAMESTAFF { get; set; }
        public int PHONENUMBERSTAFF { get; set; }
        public string ADDRESSSTAFF { get; set; }
        public string MAILSTAFF { get; set; }
        public string USERNAMESTAFF { get; set; }
        public string PASSWORDSTAFF { get; set; }
        public int ORDERCURRENTTOTAL { get; set; }

        public override string ToString()
        {
            return "IdStaff : " + IDSTAFF + "\n" +
            "IdCity : " + IDCITY + "\n" +
            "NameStaff : " + NAMESTAFF + "\n" +
            "SurnameStaff : " + SURNAMESTAFF + "\n" +
            "PhoneNumberStaff : " + PHONENUMBERSTAFF + "\n" +
            "AddressStaff : " + ADDRESSSTAFF + "\n" +
            "MailStaff : " + MAILSTAFF + "\n" +
            "UserNameStaff : " + USERNAMESTAFF + "\n" +
            "PasswordStaff : " + PASSWORDSTAFF + "\n" +
            "OrderCurrentTotal : " + ORDERCURRENTTOTAL + "\n";
        }
    }
}
