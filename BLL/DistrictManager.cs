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
    public class DistrictManager : IDistrictManager
    {
        private IDistrictDB DistrictDb { get; set; }

        public DistrictManager(IConfiguration conf)
        {
            DistrictDb = new DistrictDB(conf);
        }

        public District GetDistrict(int idDistrict)
        {
            return DistrictDb.GetDistrict(idDistrict);
        }

        public List<District> GetDistricts()
        {
            return DistrictDb.GetDistricts();
        }
    }
}
