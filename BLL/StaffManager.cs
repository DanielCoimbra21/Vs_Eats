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
    public class StaffManager : IStaffManager
    {
        private IStaffDB StaffDb { get; }
        public StaffManager(IConfiguration conf)
        {
            StaffDb = new StaffDB(conf);
        }

        public void UpdatePassword(Staff staff)
        {
            StaffDb.UpdatePassword(staff);
        }

        public void UpdateStaff(Staff staff) {
            StaffDb.UpdateStaff(staff);
        }

        public DTO.Staff GetStaff(string mailStaff, string passwordStaff)
        {
            return StaffDb.GetStaff(mailStaff, passwordStaff);
        }

        public Staff GetStaff(int idStaff)
        {
            return StaffDb.GetStaff(idStaff);
        }

    }
}
