using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using WebAppVSEAT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppVSEAT.Controllers
{
    public class LoginController
    {

        private ICustomerManager CustomerManager { get; }

        public LoginController(ICustomerManager customerManager)
        {
            CustomerManager = customerManager;
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public iactionresult index(loginvm loginvm)
        //{
        //    if (modelstate.isvalid)
        //    {
        //        var member = membermanager.getmember(loginvm.email, loginvm.password);

        //        if (member != null)
        //        {
        //            httpcontext.session.setint32("idmember", member.idmember);
        //            return redirecttoaction("index", "members");
        //        }

        //        modelstate.addmodelerror(string.empty, "invalid email or password");
        //    }
        //    return view(loginvm);
        //}

    }
}
