using A108222027_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace A108222027_finalProject.Controllers
{
    public class HomeController : Controller
    {

        MacdonaldEntities mac = new MacdonaldEntities();
        // GET: Home
        public ActionResult Index()
        {
            Account.allAccount = mac.tAccount.ToList();
            return View();
        }
    }
}