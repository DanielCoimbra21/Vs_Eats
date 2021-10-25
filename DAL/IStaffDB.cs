using DTO;
using System.Collections.Generic;
namespace DAL
{
    public interface IStaffDB
    {
        public List<Staff> GetStaffs();
    }
}
