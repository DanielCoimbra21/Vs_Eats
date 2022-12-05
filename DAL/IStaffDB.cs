using DTO;
using System.Collections.Generic;
namespace DAL
{
    public interface IStaffDB
    {
        DTO.Staff GetStaff(string mailStaff, string passwordStaff);
        void UpdateStaff(Staff staff);
        Staff GetStaff(int idStaff);
        void UpdatePassword(Staff staff);

        public string GetPassword(string email);
        public Staff GetStaff(string email);
    }
}
