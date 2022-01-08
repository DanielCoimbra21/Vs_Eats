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
        public MailController()
        {
            
        }

        public void SendRegisterMail(string customerMail, string firstname)
        {
            //Write the password of your email address
            string pwd = "eatvalais13"; 

            //the mail is sent from the website
            string from = "eat.valais@gmail.com"; 
            
            //Replace this with the Email Address to whom you want to send the mail
            string to = customerMail; 
            
            //Create the mail
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from);
            mail.Subject = "Thank you !";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Dear " + firstname + "," + "\n Welcome to VS EAT, we hope you'll enjoy ordering from us. \n" + " The VS EAT Team !";

            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();

            //Add the Creddentials, use your own email id and password
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
