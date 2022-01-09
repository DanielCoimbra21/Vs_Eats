using BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebAppVSEAT.Controllers
{
    public class MailController : Controller
    {

        private string pwd = "eatvalais13";
        private string from = "eat.valais@gmail.com";
        private ICustomerManager CustomerManager { get; }
        private IStaffManager StaffManager { get; }

        public MailController()
        {
            
        }

        public MailController(ICustomerManager customerManager, IStaffManager staffManager)
        {
            CustomerManager = customerManager;
            StaffManager = staffManager;
        }

        /// <summary>
        /// Method to create a new mail when a new customer is registered
        /// </summary>
        /// <param name="customerMail"></param>
        /// <param name="firstname"></param>
        public void SendRegisterMail(string customerMail, string firstname)
        {          
            //Create the mail
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(customerMail);
            mail.From = new MailAddress(from);
            mail.Subject = "Thank you !";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Dear " + firstname + "," + "\n Welcome to VS EAT, we hope you'll enjoy ordering from us. \n" + " The VS EAT Team !";

            //Method to send the mail
            SendMail(mail);
        }

        /// <summary>
        /// Method to create a new email when someone ordered dishes
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="idStaff"></param>
        /// <param name="idOrder"></param>
        /// <param name="deliverytime"></param>
        /// <param name="totalPrice"></param>
        public void SendOrderConfirmationMail(int idCustomer, int idStaff,int idOrder,DateTime deliverytime,double totalPrice)
        {
            var customer = CustomerManager.GetCustomerID(idCustomer);
            var staff = StaffManager.GetStaff(idStaff);

            //Create the mail
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(customer.MAIL);
            mail.From = new MailAddress(from);
            mail.Subject = "Your order number " + idOrder;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "You will find here your order details,\n"
                + "OrderId: " + idOrder + "\n"
                + "DeliveryTime: " + deliverytime + "\n"
                + "Delivery Man: " + staff.NAMESTAFF + " " + staff.SURNAMESTAFF + "\n"
                + "Phone number: " + staff.PHONENUMBERSTAFF + "\n"
                + "Total price: " + totalPrice + " CHF \n"
                + "Enjoy your future meal \n The VS EAT Team !";

            //Method to send the mail
            SendMail(mail);
        }

        /// <summary>
        /// Method to send the mail when you want to become a staff 
        /// </summary>
        public void SendBecomeStaffMail(string mailFrom, string body, string subject)
        {
            //Create the mail
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(from);
            mail.From = new MailAddress(mailFrom);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            
            //Method to send the mail
            SendMail(mail);
        }

        /// <summary>
        /// Method to create a new email when someone cancel an order
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="idStaff"></param>
        /// <param name="idOrder"></param>
        /// <param name="deliverytime"></param>
        /// <param name="totalPrice"></param>
        public void SendCancelOrderMail(string mailCustomer, int idOrder)
        {
            //Create the mail
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(mailCustomer);
            mail.From = new MailAddress(from);
            mail.Subject = "Cancel order n°" + idOrder;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Your order has been canceled\n"
               
                + "\n We hope to see you back soon ! \n The VS EAT Team !";

            //Method to send the mail
            SendMail(mail);
        }

        /// <summary>
        /// Method that actually send the mail to the customer
        /// </summary>
        /// <param name="mail"></param>
        private void SendMail(System.Net.Mail.MailMessage mail)
        {
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();

            //Use your own email id and password
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pwd);

            // Gmail works on this port
            client.Port = 587;
            client.Host = "smtp.gmail.com";

            //Gmail works on Server Secured Layer
            client.EnableSsl = true;


            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
            }
        }

    }
}
