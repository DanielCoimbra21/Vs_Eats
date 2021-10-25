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
    public class StaffManager
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
    }
}
