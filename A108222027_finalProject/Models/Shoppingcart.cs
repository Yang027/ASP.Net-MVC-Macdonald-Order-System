using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A108222027_finalProject.Models
{
    public class Shoppingcart
    {
        public static Dictionary<tItem, int> shoppingcart { get; set; }//item count
        public static int ddmoney { get; set; }
    }
}