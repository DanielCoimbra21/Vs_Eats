using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using WebAppVSEAT.Models;

namespace WebAppVSEAT.Controllers
{

    public class StaffController : Controller
    {
        private ICityManager CityManager { get; }
        private IStaffManager StaffManager { get; }

        public StaffController(IStaffManager staffManager, ICityManager cityManager)
        {
            CityManager = cityManager;
            StaffManager = staffManager;
        }

        public IActionResult Profile()
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var staff = StaffManager.GetStaff((int)HttpContext.Session.GetInt32("IdStaff"));
            var staffVM = new StaffVM();

            //Rechercher le nom de la ville
            var city = CityManager.GetCity(staff.IDCITY);


            staffVM.NAMESTAFF = staff.NAMESTAFF;
            staffVM.SURNAMESTAFF = staff.SURNAMESTAFF;
            staffVM.PHONENUMBERSTAFF = staff.PHONENUMBERSTAFF;
            staffVM.CITYNAME = city.CITYNAME;
            staffVM.ADDRESSSTAFF = staff.ADDRESSSTAFF;
            staffVM.MAILSTAFF = staff.MAILSTAFF;
            staffVM.USERNAMESTAFF = staff.USERNAMESTAFF;

            return View(staffVM);
        }

        public IActionResult Edit()
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Récupérer le staff avec id du login dans session
            DTO.Staff staff = StaffManager.GetStaff((int)HttpContext.Session.GetInt32("IdStaff"));
            
            //Trouver l'idCity en fonction de la ville
            var city = CityManager.GetCity(staff.IDCITY);

            StaffVM staffVM = new StaffVM()
            {
                IDSTAFF = staff.IDSTAFF,
                IDCITY = staff.IDCITY,
                NAMESTAFF = staff.NAMESTAFF,
                SURNAMESTAFF = staff.SURNAMESTAFF,
                PHONENUMBERSTAFF = staff.PHONENUMBERSTAFF,
                ADDRESSSTAFF = staff.ADDRESSSTAFF,
                CITYNAME = city.CITYNAME,
                MAILSTAFF = staff.MAILSTAFF,
                USERNAMESTAFF = staff.USERNAMESTAFF
            };

            return View(staffVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StaffVM staffVM)
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            DTO.Staff staff = StaffManager.GetStaff(staffVM.IDSTAFF);

            //Trouver l'idCity en fonction de la ville
            var cities = CityManager.GetCities();
            var idCity = -1;
            foreach (var city in cities)
            {
                if (city.CITYNAME == staffVM.CITYNAME)
                {
                    idCity = city.IDCITY;
                }
            }

            if (idCity == -1)
            {
                ModelState.AddModelError(string.Empty, "Wrong city entered, write correctly");
                return View(staffVM);
            }

            staff.IDSTAFF = staffVM.IDSTAFF;
            staff.IDCITY = idCity;
            staff.NAMESTAFF = staffVM.NAMESTAFF;
            staff.SURNAMESTAFF = staffVM.SURNAMESTAFF;
            staff.PHONENUMBERSTAFF = staffVM.PHONENUMBERSTAFF;
            staff.ADDRESSSTAFF = staffVM.ADDRESSSTAFF;
            staff.MAILSTAFF = staffVM.MAILSTAFF;
            staff.USERNAMESTAFF = staffVM.USERNAMESTAFF;

            

            if (ModelState.IsValid)
            {
                StaffManager.UpdateStaff(staff);
                return View("~/Views/Staff/Profile.cshtml", staffVM);
            }

            return View(staffVM);
        }
        /// <summary>
        /// Méthode pour le changement de mot de passe dans la page profile
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        /// <summary>
        /// Méthode pour changer le mot de passe
        /// </summary>
        /// <param name="changePasswordVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (ModelState.IsValid)
            {
                int idStaff = (int)HttpContext.Session.GetInt32("IdStaff");
                var staff = StaffManager.GetStaff(idStaff);

                //vérification que le mot de passe entrer est bien le même que celui sauvegarder
                if (StaffManager.VerifyPassword(changePasswordVM.PASSWORDSTAFF, staff.MAILSTAFF))
                {
                    //vérification que le nouveau mot de passe est égal au mot de passe confirmé
                    if (changePasswordVM.NEWPASSWORD == changePasswordVM.CONFIRMPASSWORD)
                    {
                        staff.PASSWORDSTAFF = StaffManager.SetPassword(changePasswordVM.CONFIRMPASSWORD);
                        StaffManager.UpdatePassword(staff);
                        return RedirectToAction("Profile", "Staff");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong password or wrong new password");
                        return View(changePasswordVM);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong password or wrong new password");
                    return View(changePasswordVM);
                }
            }

            return RedirectToAction("Profile", "Staff");
        }
        
    }
        
}
