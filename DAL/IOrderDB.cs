using DTO;
using System.Collections.Generic;


namespace DAL
{
    public interface IOrderDB
    {

        public List<Order> GetOrders();
        public void ArchiveDelivery(Order order);

    }
}
