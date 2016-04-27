using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using TheWorld.Models;

namespace TheWorld.Controllers.Web
{
    public class AppController:Controller
    {
        private IMailService _mailService;
        private WorldContext _context;

        public AppController(IMailService Service, WorldContext context)
        {
            _mailService = Service;
            _context = context;

        }
        public IActionResult Index()
        {
            var trips = _context.Trips.OrderBy(t=>t.Name).ToList();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            { 

                var email = Startup.configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("","Could not send email, configuration problem.");
                }
               if ( _mailService.SendMail(email, email,$"Contact Page from {model.Name} ({model.Email})",model.Message))
               {
                    ModelState.Clear();

                    ViewBag.Message = "Mail Sent.Thanks!";
               }
                
            }
            return View();
        }
    }
}
