using A108222027_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace A108222027_finalProject.Controllers
{
    public class ShoppingCartController : Controller
    {
        MacdonaldEntities mc = new MacdonaldEntities();
        List<tItem> cart = Shoppingcart.shoppingcart == null ? new List<tItem>() : Shoppingcart.shoppingcart.Keys.ToList();
        List<int> num = new List<int>();

        // GET: ShoppingCart

        [HttpPost]
        public ActionResult discount(string _discount)
        {
            //check if its noob

            _discount = _discount.Trim();
            //MessageBox.Show(_discount.Trim());
            mc = new MacdonaldEntities();
            tAccount now = null;
            if (Account.nowAccount != null)
            {
                now = mc.tAccount.Where(x => x.aAcount == Account.nowAccount.aAcount).FirstOrDefault();
            }
            else
            {
                MessageBox.Show("please Log in first!");
                return RedirectToAction("Index", "Login");
            }
            var dd = mc.tdiscount.ToList();
            var tmp = dd.Where(x => x.Discount == _discount).FirstOrDefault();
            tdiscount usediscount = null;
            if (tmp.noob == 1)
            {
                if (Account.nowAccount != null)
                {
                    if (now.Transaction == null)
                    {
                        usediscount = tmp;
                    }
                }
                else
                {
                    MessageBox.Show("please Log in first!");
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            { usediscount = tmp; }
            if (usediscount != null)
            {
                var price = Shoppingcart.shoppingcart.Keys.Select(x => x.price);
                var count = Shoppingcart.shoppingcart.Values.Select(x => x);

                List<int> total = new List<int>();
                for (int i = 0; i < price.Count(); i++)
                {
                    var subtotal = price.ElementAt(i) * count.ElementAt(i);
                    total.Add((int)subtotal);
                }
                var sum = total.Sum();
                if (sum < (int)usediscount.limit) { ViewBag.Err = "X不符合此優惠券！"; }
                else
                {
                    var afterdiscount = sum - usediscount.money;
                    ViewBag.discount = (int)afterdiscount;//傳discount的價錢到view
                    Shoppingcart.ddmoney = (int)usediscount.money;
                    ViewBag.dd = usediscount.money;
                    //判斷金額有沒有到
                    //有到就打折               
                }
            }
            else
            {
                ViewBag.Err = "X不符合此優惠券！";
            }
            refresh();
            return View("Index");
        }
        public ActionResult Index()
        {
            refresh();
            return View();
        }
        void refresh()
        {
            try
            {
                if (Shoppingcart.shoppingcart != null) { 
                int count = Shoppingcart.shoppingcart.Keys.Count;
                if (cart == null) { ViewBag.cartcount = 0; }
                else { ViewBag.cartcount = count; }
                ViewBag.cart = cart;
                num = Shoppingcart.shoppingcart.Values.ToList();
                ViewBag.count = num;}
            }
            catch
            {
                Shoppingcart.shoppingcart = new Dictionary<tItem, int>();
            }
            //compute
            List<double> subtoal = new List<double>();
            for (int i = 0; i < num.Count; i++)
            {
                subtoal.Add(num[i] * (double)cart[i].price);
            }
            ViewBag.subtotal = subtoal;
            ViewBag.total = subtoal.Sum();
        }
        public ActionResult ChangeCount(string ii, string num)
        {
            MessageBox.Show("ii:" + ii + "" +
                "\n" + "num" + num);
            return View();
        }
        public ActionResult Delete(string index)
        {
            index = index.Trim();
            var allitem = Shoppingcart.shoppingcart.Keys.ToList();
            var nowitem = allitem.Where(x => x.item == index).FirstOrDefault();
            if (Shoppingcart.shoppingcart[nowitem] >= 1)
                Shoppingcart.shoppingcart[nowitem] -= 1;
            if (Shoppingcart.shoppingcart[nowitem] <= 0)
                Shoppingcart.shoppingcart.Remove(nowitem);
            cart = Shoppingcart.shoppingcart.Keys.ToList();
            refresh();
            return View("Index");
        }

        public ActionResult Buy()
        {
            //MessageBox.Show("buy");
            if (Account.nowAccount == null)
            {
                MessageBox.Show("please Log in first!");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                MessageBox.Show("感謝您的購買");
                var buy = Transaction.transactionList;
                Transaction tc = new Transaction();
                //清空購物車

                //save to database
                MacdonaldEntities mc = new MacdonaldEntities();
                var _now = mc.tAccount.First(x => x.aAcount == Account.nowAccount.aAcount);

                _now.Transaction += tc._toString(Shoppingcart.ddmoney);
                Shoppingcart.ddmoney = 0;
                mc.SaveChanges();
                Transaction.transactionList = null;
                Shoppingcart.shoppingcart = null;
                return RedirectToAction("Index", "Home");
            }
        }

    }
}