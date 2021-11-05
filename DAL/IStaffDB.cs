using DTO;
using System.Collections.Generic;
namespace DAL
{
    public interface IStaffDB
    {
        public List<Staff> GetStaffs();

        public Staff GetStaff(string usernameStaff, string passwordStaff);
        public void UpdateStaff(Staff staff);

        public Staff GetStaff(int idStaff);


    }
}
