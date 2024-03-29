﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IStaffManager
    {
        void UpdateStaff(Staff staff);
        DTO.Staff GetStaff(string mailStaff, string passwordStaff);
        Staff GetStaff(int idStaff);
        Staff GetStaff(string mailStaff);
        void UpdatePassword(Staff staff);

        public string SetPassword(string password);

        //public string GetPassword(string mail);


        public Boolean VerifyPassword(string passwordLogin, string passwordStored);
    }


}
