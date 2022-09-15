using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A108222027_finalProject.Models
{
    public class Account
    {
        public static List<tAccount> allAccount { get; set; }
        public static bool login = false;
        public static tAccount nowAccount = null;
    }
}