using A108222027_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
//commit
namespace A108222027_finalProject.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Index()
        {
            //MessageBox.Show(Account.login ? "true" : "false");
            if (!Account.login)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Member");
            }
        }


        [HttpPost]
        public ActionResult Index(string inputmailinline, string inputpasswordinline)
        {
            //MessageBox.Show(inputmailinline + "" + inputpasswordinline);
            inputmailinline = inputmailinline.Trim();
            inputpasswordinline = inputpasswordinline.Trim();
            try
            {
                //MessageBox.Show("allCOunt" + Account.allAccount.Count());
                var temp = Account.allAccount.Where(x => x.aAcount.Trim() == inputmailinline);
                if (temp.Count() == 0)
                {
                    ViewBag.Err = "賬號錯誤！";
                    return View();
                }
                else
                {
                    // MessageBox.Show(temp.FirstOrDefault().aPassword.Trim() + "and" + inputpasswordinline);
                    if (temp.FirstOrDefault().aPassword.Trim() == inputpasswordinline)
                    {
                        Account.nowAccount = temp.FirstOrDefault();
                        Account.login = true;
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        ViewBag.Err = "密碼錯誤！";
                        return View();
                    }
                }
            }
            catch (Exception ee)
            {
                ViewBag.Err = "賬密錯誤！";
                //MessageBox.Show("" + ee.Message);
                return View();
            }
        }
        //need actionResult to judge whether have account or not

        //redirectAction to signup the account;
    }
}
