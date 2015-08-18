using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meridian59.Data.Models.AdminData
{
    public class AddTrackedAdminObjectEventHandlerArgs
    {
        public AdminObject AdminObject;
        public object Sender;

        public AddTrackedAdminObjectEventHandlerArgs(object sender, AdminObject adminObject)
        {
            AdminObject = adminObject;
            Sender = sender;
        }
    }
    
    public class LogAdminMessageEventHandlerArgs
    {
        public string AdminMessage { get; set; }

        public LogAdminMessageEventHandlerArgs(string message)
        {
            AdminMessage = message;
        }
    }
}
