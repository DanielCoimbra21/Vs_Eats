using DTO;
using System.Collections.Generic;
namespace DAL
{
    public interface IStaffDB
    {
        List<Staff> GetStaffs();

        DTO.Staff GetStaff(string mailStaff, string passwordStaff);
        void UpdateStaff(Staff staff);

        Staff GetStaff(int idStaff);


    }
}
