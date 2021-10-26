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
    public class DistrictManager
    {
        private IDistrictDB DistrictDb { get; set; }

        public DistrictManager(IConfiguration conf)
        {
            DistrictDb = new DistrictDB(conf);
        }

        public District GetDistrict(string districtName)
        {
            return DistrictDb.GetDistrict(districtName);
        }

        public List<District> GetDistricts()
        {
            return DistrictDb.GetDistricts();
        }
    }
}
