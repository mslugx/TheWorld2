using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheWorld.Services
{
    public class DebugMailService : IMailService
    {
        public bool SendMail(string to, string form,string subject, string body)
        {
            Debug.WriteLine($"Sending mail: To : {to}, Subject: {subject}");
            return true;  
        }
    }
}
