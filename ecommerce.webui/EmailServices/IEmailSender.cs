using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.webui.EmailServices
{
    public interface IEmailSender
    {
        //smtp server : gmail veya hotmail
        Task SendEmailAsync(string email,string subject,string htmlMessage);
    }
}