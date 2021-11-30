using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IStaffManager
    {
        public void UpdateStaff(StaffManager staff);

        List<Staff> GetStaffs();

        Staff GetStaff(string usernameStaff, string passwordStaff);

        void ArchiveDelivery(Order order);

        Staff GetStaff(int idStaff);

    }


}
