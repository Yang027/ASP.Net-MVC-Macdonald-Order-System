using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace A108222027_finalProject.Models
{
    public class Transaction
    {
        public int _id;
        public string _item;
        public decimal _price;
        public int _count;
        public double _subtotal;
        // private double _total;
        public Transaction() { }
        public Transaction(int id, string item, decimal price, int count, double subtotal)
        {

            _id = id;
            _item = item;
            _price = price;
            _count = count;
            _subtotal = subtotal;
        }
        public static List<Transaction> transactionList { get; set; }
        public static List<Transaction> tt { get; set; }
        public static List<List<Transaction>> alltransrec { get; set; }//訂單詳情
        public static List<int> TotalMoney { get; set; }//這是給每筆訂單設的總價 交易記錄用
        public static List<string> Datetime { get; set; }//交易記錄用 每筆訂單的交易時間 儅標題
        public string _toString(int discount = 0)//結賬
        {
            string ss = "=";
            ss += "\n";
            ss += DateTime.Now.ToString("f") + "\n";
            var pp = transactionList.Select(x => x._price);
            var cc = transactionList.Select(x => x._count);
            List<int> tmp = new List<int>();
            for (int j = 0; j < pp.Count(); j++)
            {
                tmp.Add((int)(pp.ElementAt(j) * cc.ElementAt(j)));
            }
            var sum = (tmp.Sum() - discount) < 0 ? 0 : (tmp.Sum() - discount);
            ss += sum.ToString() + "\n";//total money

            for (int i = 0; i < transactionList.Count; i++)
            {
                ss += $"{transactionList[i]._id},{transactionList[i]._item},{transactionList[i]._price},{transactionList[i]._count},{transactionList[i]._subtotal}" + "\t";
            }
            return ss;
        }
        public void str2Transaction(tAccount ac)
        {
            tt = null;
            alltransrec = null;
            TotalMoney = null;
            Datetime = null;
            try
            {
                if (ac.Transaction != null)
                {
                    string alltrans = ac.Transaction;
                    var tmpeachTrans = alltrans.Split('=');
                    for (int i = 1; i < tmpeachTrans.Length; i++)
                    {
                        var eachTrans = tmpeachTrans[i].Split('\n');
                        #region 每筆訂單的交易時間
                        if (Datetime == null)
                        {
                            Datetime = new List<string>() { eachTrans[1] };
                        }
                        else { Datetime.Add(eachTrans[1]); }
                        #endregion
                        #region 每筆訂單的總價
                        if (TotalMoney == null)
                        {
                            TotalMoney = new List<int>() { int.Parse(eachTrans[2]) };
                        }
                        else { TotalMoney.Add(int.Parse(eachTrans[2])); }
                        #endregion

                        //用\t 先分成 各個餐點 再用逗號分
                        var kind = eachTrans[3].Split('\t');
                        for (int j = 0; j < kind.Length; j++)
                        {
                            var tmp = kind[j].Split(',');
                            if (tmp.Length > 1)
                            {
                                if (tt == null)
                                {
                                    tt = new List<Transaction>();
                                    tt.Add(new Transaction(this._id = int.Parse(tmp[0]), this._item = tmp[1],
                                        this._price = decimal.Parse(tmp[2]), this._count = int.Parse(tmp[3]), this._subtotal = int.Parse(tmp[4])));
                                }
                                else
                                {
                                    tt.Add(new Transaction(this._id = int.Parse(tmp[0]), this._item = tmp[1],
                                             this._price = decimal.Parse(tmp[2]), this._count = int.Parse(tmp[3]), this._subtotal = int.Parse(tmp[4])));
                                }
                            }
                        }
                        if (alltransrec == null) alltransrec = new List<List<Transaction>>();
                        alltransrec.Add(tt);
                        tt = null;
                    }//加完
                }
            }
            catch { }
        }
    }
}