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
    public class DistrictStaffManager : IDistrictStaffManager
    {
        private IDistrictStaffDB DistrictDb { get; }
        public DistrictStaffManager(IConfiguration conf)
        {
            DistrictDb = new DistrictStaffDB(conf);
        }

        public List<DistrictStaff> GetDistrictStaffs()
        {
            return DistrictDb.GetDistrictStaffs();
        }
    }
}
