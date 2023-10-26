using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.ethereal.email", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("georgianna80@ethereal.email", "83SDX2WdavvCA3Skes");

            client.Send("georgianna80@ethereal.email", email.To, email.Title, email.Body);
        }
    }
}
