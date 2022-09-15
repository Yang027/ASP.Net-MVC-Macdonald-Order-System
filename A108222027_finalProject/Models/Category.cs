using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace A108222027_finalProject.Models
{
    public class Category
    {
        MacdonaldEntities mc;
        public Dictionary<string, List<tItem>> Item { get; set; }
        public List<string> categort { get; set; }
        public Category()
        {
            allitem();
        }
        public void allitem()
        {
            mc = new MacdonaldEntities();
            var xx = mc.tItem;
            List<string> cat = new List<string>();
            Dictionary<string, List<tItem>> ans = new Dictionary<string, List<tItem>>();
            foreach (var x in xx)
            {
                if (ans.ContainsKey(x.category))
                {
                    ans[x.category].Add(new tItem
                    {
                        Id = x.Id,
                        item = x.item,
                        price = x.price,
                        category = x.category,
                        photo = x.photo
                    });
                }
                else
                {
                    cat.Add(x.category);
                    ans.Add(x.category, new List<tItem>() {new tItem
                    {
                        Id = x.Id,
                        item = x.item,
                        price = x.price,
                        category = x.category,
                        photo = x.photo
                    } });
                }
            }
            categort = cat;
            Item = ans;
        }
    }
}