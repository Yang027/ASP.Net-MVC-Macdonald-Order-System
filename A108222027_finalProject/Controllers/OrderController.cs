using A108222027_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace A108222027_finalProject.Controllers
{
    public class OrderController : Controller
    {
        MacdonaldEntities mc = new MacdonaldEntities();
        Category cc = new Category();
        // GET: Order
        public ActionResult Index()
        {
           
            var xx = cc.Item.Select(x => x.Key);
            ViewBag.cIndex = 0;
            ViewBag.firstcat = cc.Item[cc.Item.First().Key];//category
            passVal();
            return View();
        }
        public ActionResult selectCate(string cateIndex)
        {
          //  cc = new Category();
            //MessageBox.Show("cate:" + cateIndex);
            int cateindex = int.Parse(cateIndex.Trim());
            passVal();
            ViewBag.cIndex = cateIndex;
            ViewBag.firstcat = cc.Item[cc.Item.ElementAt(cateindex).Key];//category
                                                                         // MessageBox.Show(cc.Item[cc.Item.ElementAt(cateindex).Key][0].photo);//~/Images/breakfast/Image_Break_Topic/@i .png
            return View("Index");
        }
        void passVal(bool flag = false)
        {
            ViewBag.allitem = cc.Item;
            ViewBag.allcate = cc.Item.Select(x => x.Key);// cc.Item.Keys;
            if (flag)
            {
                TempData["buy"] = "已加入購物車";
            }
        }
        public ActionResult buy(string cateIndex, string itemIndex)
        {
            int cIndex = int.Parse(cateIndex);
            int rIndex = int.Parse(itemIndex);


            if (Shoppingcart.shoppingcart == null)
            {
                Shoppingcart.shoppingcart = new Dictionary<tItem, int>();
            }

            var check = Shoppingcart.shoppingcart.Keys.Where(x => x.item == cc.Item.ElementAt(cIndex).Value[rIndex].item);//看看現在點選的物品有沒有重複的

            IEnumerable<Transaction> tt = null;
            if (Transaction.transactionList != null)
            {
                tt = Transaction.transactionList.Where(x => x._item == cc.Item.ElementAt(cIndex).Value[rIndex].item);
                if (tt.Count() > 0)
                {
                    var tmpItem = tt.ElementAt(0);
                    tmpItem._count += 1;
                    tmpItem._subtotal += (double)tmpItem._price;
                }
                else
                {
                    var _item = cc.Item.ElementAt(cIndex).Value[rIndex];
                    Transaction ts = new Transaction(_item.Id, _item.item, (decimal)_item.price, 1, 1 * (double)_item.price);//Transaction(int id,  string item, decimal price, int count, double subtotal, double total)
                    Transaction.transactionList.Add(ts);
                }
            }
            else
            {
                Transaction.transactionList = new List<Transaction>();
                var _item = cc.Item.ElementAt(cIndex).Value[rIndex];
                Transaction ts = new Transaction(_item.Id, _item.item, (decimal)_item.price, 1, 1 * (double)_item.price);//Transaction(int id,  string item, decimal price, int count, double subtotal, double total)
                Transaction.transactionList.Add(ts);
            }


            if (check.Count() > 0)//有重複
            {
                //不確定val會不會傳入 list
                Shoppingcart.shoppingcart[check.ElementAt(0)] += 1;
            }
            else//沒有重複
            {
                Shoppingcart.shoppingcart.Add(cc.Item.ElementAt(cIndex).Value[rIndex], 1);
            }

            passVal(true);
            ViewBag.cIndex = cateIndex;
            ViewBag.firstcat = cc.Item[cc.Item.ElementAt(cIndex).Key];//category

            return View("Index");//to shoppcart
        }
    }
}