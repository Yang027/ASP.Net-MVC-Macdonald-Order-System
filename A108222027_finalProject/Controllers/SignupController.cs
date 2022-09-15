using A108222027_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace A108222027_finalProject.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Submit(string name, string password, int phone, string address)
        {
            try
            {
                tAccount ac = new tAccount
                {
                    aAcount = name,
                    aPassword = password,
                    aAddress = address,
                    aPhone = phone
                    
                };
                ac.aLevel = 2;//normal ac!
                ac.Transaction = null;
                //Account.allAccount.Add(ac);
                MacdonaldEntities mc = new MacdonaldEntities();
                mc.tAccount.Add(ac);
                mc.SaveChanges();
                Account.allAccount = new List<tAccount>();
                Account.allAccount = mc.tAccount.ToList();
                return RedirectToAction("Index", "Login");
            }
            catch { return View(); }
        }
    }
}