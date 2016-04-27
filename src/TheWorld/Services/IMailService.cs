using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public interface IMailService
    {
        bool SendMail(string to, string form, string subject, string body);
    }
}
