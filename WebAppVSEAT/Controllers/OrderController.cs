using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppVSEAT.Models;

namespace WebAppVSEAT.Controllers
{
    public class OrderController : Controller
    {
        private IOrderManager OrderManager { get; }

        public OrderController(IOrderManager orderManager)
        {
            OrderManager = orderManager;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var order = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"));
            var orderVM = new List<OrderVM>();


            foreach (var r in order)
            {
                var vm = new OrderVM();
                vm.IDORDER = r.IDORDER;
                vm.IDDISTRICT = r.IDDISTRICT;
                vm.IDRESTAURANT = r.IDRESTAURANT;
                vm.IDSTAFF = r.IDSTAFF;
                vm.IDCUSTOMER = r.IDCUSTOMER;
                vm.TOTALPRICE = r.TOTALPRICE;
                vm.STATUS = r.STATUS;

                orderVM.Add(vm);
            }

            return View(orderVM);
        }
    }
}
