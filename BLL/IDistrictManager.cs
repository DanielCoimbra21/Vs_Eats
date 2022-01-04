using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IDistrictManager
    {
        District GetDistrict(int idDistrict );
        List<District> GetDistricts();
    }
}
