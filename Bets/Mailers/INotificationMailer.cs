using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;

namespace Bets.Mailers
{ 
    public interface INotificationMailer
    {
				
		MailMessage Match();
		
		
	}
}