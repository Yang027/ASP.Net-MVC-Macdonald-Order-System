using A108222027_finalProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace A108222027_finalProject.Controllers
{
    //會員管理頁
    public class MemberController : Controller
    {
        MacdonaldEntities mc = new MacdonaldEntities();
        Category cc = new Category();

        List<List<Transaction>> alltransrec;//訂單詳情
        List<int> TotalMoney;//這是給每筆訂單設的總價 交易記錄用
        List<string> Datetime;//交易記錄用 每筆訂單的交易時間 儅標題
        List<string> Acc;
        Transaction tc;
        public void refresh()
        {
            cc = new Category();
            ViewBag.allitem = cc.Item;
            ViewBag.cate = cc.categort;
        }
        void updateTrans()
        {
            tc = new Transaction();
            MacdonaldEntities mc = new MacdonaldEntities();
            //  var ttac = mc.tAccount.Where(x => x.aAcount == Account.nowAccount.aAcount).ElementAt(0);         
            var nowac = mc.tAccount.Where(x => x.aAcount == Account.nowAccount.aAcount).FirstOrDefault();
            if (nowac.Transaction != null)
                tc.str2Transaction(nowac);

        }
        // GET: Member
        public ActionResult Index()
        {
            ViewBag.nowAccount = Account.nowAccount.aAcount;
            if (Account.nowAccount.aLevel == 2)//一般顧客
            {
                //更新
                updateTrans();
                return View();
            }
            else
            {
                refresh();
                return View("AdminIndex");
            }
        }

        void adminTrans()
        {
            tc = new Transaction();
            mc = new MacdonaldEntities();
            Datetime = new List<string>();
            var ac = mc.tAccount.ToList();
            Acc = new List<string>();
            ini();
            for (int i = 0; i < mc.tAccount.Count(); i++)
            {
                var nowac = ac[i];
                tc.str2Transaction(nowac);
                // Acc.Add(nowac.aAcount);//買的那個使用者
                var dd = Transaction.Datetime;
                var tm = Transaction.TotalMoney;
                var trans = Transaction.alltransrec;
                if (dd != null)
                {
                    for (int x = 0; x < dd.Count; x++)
                    {
                        Acc.Add(nowac.aAcount);//買的那個使用者
                        Datetime.Add(dd[x]);
                    }//時間
                    for (int y = 0; y < tm.Count; y++) { TotalMoney.Add(tm[y]); }//錢
                    for (int z = 0; z < trans.Count; z++) { alltransrec.Add(trans[z]); }//賬單
                }
            }
            ViewBag.tdate = Datetime;
            ViewBag.tmoney = TotalMoney;
            ViewBag.tAc = Acc;
        }
        void ini()
        {
            Acc = new List<string>();
            TotalMoney = new List<int>();
            Datetime = new List<string>();
            alltransrec = new List<List<Transaction>>();
        }
        public ActionResult Signout()//登出
        {
            Account.login = false;
            Transaction.Datetime = null;
            Transaction.alltransrec = null;
            Transaction.TotalMoney = null;
            Transaction.transactionList = null;
            Account.nowAccount = null;
            return RedirectToAction("Index", "Home");
        }
        #region 管理員      
        [HttpPost]
        public ActionResult upload(HttpPostedFileBase photo)
        {

            string fileName = "";
            string path = "";
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    fileName = Path.GetFileName(photo.FileName);
                    //  MessageBox.Show("" + fileName);
                    path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    photo.SaveAs(path);
                    if (path != "")
                    {
                        path = "../Images/" + fileName;
                        Item.img = path;
                        ViewBag.photo = path;
                        //   MessageBox.Show(path);
                    }
                }
            }

            ViewBag.select = 'a';
            refresh();
            return View("AdminIndex");
        }
        [HttpPost]
        public ActionResult AddItem(string cate, string item, string price)
        {
            //MessageBox.Show("cate:" + cate + "\n item:" + item + Item.img + "\n" + "photo:" + "\n price:" + price);
            if (item.Trim() != "" && price.Trim() != "")
            {
                mc.tItem.Add(new tItem
                {
                    item = item,
                    price = decimal.Parse(price),
                    photo = Item.img == "" ? "../Images/a1.jpg" : Item.img,
                    category = cate
                });
                Item.img = null;
                mc.SaveChanges();
            }
            else
            {
                MessageBox.Show(item.Trim() == "" ? "商品名稱不能爲空！" : price.Trim() == "" ? "價錢不能爲空" : "");
            }
            refresh();
            ViewBag.select = 'q';//商品管理頁
            return View("AdminIndex");
        }
        [HttpPost]
        public ActionResult addDiscount(string discount, string money, string dd, string discribe, string limit)
        {
            //dd->是限制是否為第一次購買才能用的折價券
            //   MessageBox.Show($"{discount},{money},{dd},{discribe},{limit}");
            discount = discount.Trim();
            money = money.Trim();
            dd = dd.Trim();
            discribe = discribe.Trim();
            limit = limit.Trim();

            if (discount != "" && money != "" && dd != "" && discribe != "" && limit != "")
            {
                decimal mm = 0;
                int ll = 0;
                if (decimal.TryParse(money, out mm) && int.TryParse(limit, out ll) && (dd == "1" || dd == "2"))
                {
                    mc = new MacdonaldEntities();
                    tdiscount dis = new tdiscount();
                    dis.Id = mc.tdiscount.Count();
                    dis.Discount = discount;
                    dis.discribe = discribe;
                    dis.money = mm;
                    dis.limit = int.Parse(limit);
                    dis.noob = int.Parse(dd);
                    //noob(也就是dd)==2就是for everybody
                    //==1就是for noob
                    mc.tdiscount.Add(dis);
                    mc.SaveChanges();
                    MessageBox.Show("加入成功！");
                }
                else
                {
                    MessageBox.Show("請正確填寫消費券欄位！");
                }
            }
            else
            {
                MessageBox.Show("所有欄位不得爲空！");
            }

            ViewBag.select = 'e';
            //save to db
            Discountsetting();//<-this will Re-read DB ,and pass value to viewBag
            return View("AdminIndex");
        }
        public ActionResult Del_Discount(string c)
        {
            mc = new MacdonaldEntities();
            var dis = mc.tdiscount.ToList();
            var deldis = dis[int.Parse(c)];
            mc.tdiscount.Remove(deldis);
            mc.SaveChanges();

            ViewBag.select = 'e';
            Discountsetting();
            return View("AdminIndex");
        }
        public ActionResult AdminIndex()
        {
            refresh();
            return View();
        }
        int pageSize = 6;
        public ActionResult _switch4admin(char a, int page = 1)
        {
            refresh();
            switch (a)
            {
                case 'q':
                    ViewBag.select = 'q';
                    return View("AdminIndex");
                //case 'w':
                //    ViewBag.select = 'w';
                //    return View("AdminIndex");
                case 'e'://折價券
                    ViewBag.select = 'e';
                    Discountsetting();
                    return View("AdminIndex");
                case 'r'://營業記錄
                    return RedirectToAction("forTrans");
                default:
                    ViewBag.select = 'q';
                    return View("AdminIndex");
            }
        }
        #endregion
        void Discountsetting()
        {
            mc = new MacdonaldEntities();
            var _discount = mc.tdiscount.ToList();
            ViewBag.discount = _discount;
        }
        #region 一般顧客
        public ActionResult Copy(string c)
        {
            c = c.Trim();
            // MessageBox.Show(c);
            mc = new MacdonaldEntities();
            var _discount = mc.tdiscount.ToList();
            //  MessageBox.Show("" + _discount[int.Parse(c)].Discount);
            var xx = _discount[int.Parse(c)].Discount;
            var t = new Thread((ThreadStart)(() =>
              {
                  Clipboard.SetText(xx.ToString());
              }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            ViewBag.select = 'd';
            ViewBag.discount = _discount;
            ViewBag.nowAccount = Account.nowAccount.aAcount;
            return View("Index");
        }
        void refreshNormal()
        {
            ViewBag.nowAccount = Account.nowAccount.aAcount;
            ViewBag.nowPassword = Account.nowAccount.aPassword;
            ViewBag.nowAddress = Account.nowAccount.aAddress;
            ViewBag.nowPhone = Account.nowAccount.aPhone;
        }
        public ActionResult _switch(char a)
        {
            refreshNormal();
            switch (a)
            {
                case 'a': ViewBag.select = 'a'; return View("Index");
                case 'b':
                    return RedirectToAction("forTransNormal");
                case 'd':
                    ViewBag.select = 'd';
                    Discountsetting();
                    return View("Index");
                default: ViewBag.select = 'a'; return View("Index");
            }

        }

        /// <summary>
        /// save是用來保存一般顧客修改的個人資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult save()//string aAcount, string aPassword, string aPhone, string aAddress)
        {
            // MessageBox.Show("yang shujun is handsome");
            return View();
        }
        public ActionResult save(string aAcount, string aPassword, int aPhone, string aAddress)
        {
            //MessageBox.Show($"acount:{aAcount},ps:{aPassword},phone:{aPhone},address:{aAddress}");
            try
            {
                mc = new MacdonaldEntities();
                var xx = mc.tAccount.Where(x => x.aAcount == Account.nowAccount.aAcount).FirstOrDefault();
                xx.aAcount = aAcount;
                xx.aPassword = aPassword;
                xx.aPhone = aPhone;
                xx.aAddress = aAddress;

                
                mc.SaveChanges();
                Account.allAccount = mc.tAccount.ToList();
                Account.nowAccount = mc.tAccount.Where(x => x.aAcount == xx.aAcount).FirstOrDefault();
                ViewBag.nowAccount = Account.nowAccount.aAcount;
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }

            //use sequence
            return View("Index");
        }
        public ActionResult forTrans(int page = 1)
        {
            ViewBag.select = 'r';
            adminTrans();
            int currPage = page < 1 ? 1 : page;
            ViewBag.page = currPage;
            IPagedList<string> rr = null;

            rr = Datetime.ToPagedList(currPage, pageSize);
            //display all Transaction Record
            return View("AdminIndex", rr);
        }
        public ActionResult forTransNormal(int page = 1)
        {
            refreshNormal();
            ViewBag.select = 'b';
            passTransaction();
            var dd = Transaction.Datetime;
            int currPage = page < 1 ? 1 : page;
            ViewBag.page = currPage;
            IPagedList<string> rr = null;
            if (dd == null)
            { dd = new List<string>(); }
            rr = dd.ToPagedList(currPage, pageSize);
            return View("Index", rr);

            //display all Transaction Record

        }
        public ActionResult detail(string _index, int page = 1)
        {
            ViewBag.select = 'b';
            // MessageBox.Show(_index);
            int ii = int.Parse(_index);
            passTransaction();
            var dd = Transaction.Datetime;
            ViewBag.detail = Transaction.alltransrec[ii];
            ViewBag.nowAccount = Account.nowAccount.aAcount;
            int currPage = page < 1 ? 1 : page;
            ViewBag.page = currPage;
            var rr = dd.ToPagedList(currPage, pageSize);
            return View("Index", rr);
        }
        public ActionResult ddetail(string _index, int page = 1)
        {
            //ViewBag.select = 'r';
            int ii = int.Parse(_index);
            adminTrans();
            ViewBag.detail = alltransrec[ii];
            //return View("AdminIndex");
            ViewBag.select = 'r';
            // adminTrans();
            int currPage = page < 1 ? 1 : page;
            ViewBag.page = currPage;
            var rr = Datetime.ToPagedList(currPage, pageSize);
            //display all Transaction Record
            return View("AdminIndex", rr);
        }
        void passTransaction()
        {
            updateTrans();
            //ViewBag.tdate = Transaction.Datetime;
            ViewBag.tmoney = Transaction.TotalMoney;
        }
        // public static List<List<Transaction>> alltransrec { get; set; }//訂單詳情
        // public static List<int> TotalMoney { get; set; }//這是給每筆訂單設的總價 交易記錄用
        // public static List<string> Datetime { get; set; }//交易記錄用 每筆訂單的交易時間 儅標題
        #endregion
    }
}