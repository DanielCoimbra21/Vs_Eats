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

        public List<Staff> GetStaffs()
        {
            return StaffDb.GetStaffs(); 
        }

        public void UpdateStaff(StaffManager staff) {}

        public DTO.Staff GetStaff(string usernameStaff, string passwordStaff)
        {
            return StaffDb.GetStaff(usernameStaff, passwordStaff);
        }

        public void ArchiveDelivery(Order order)
        {

        }

        public Staff GetStaff(int idStaff)
        {
            return StaffDb.GetStaff(idStaff);
        }

    }
}
