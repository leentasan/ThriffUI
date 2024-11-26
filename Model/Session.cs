using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThriffSignUp.Model
{
    public static class Session
    {
        public static int LoggedInSellerId { get; set; }
        public static string SellerUsername { get; set; }
        public static int BuyerId { get; set; }
        public static string BuyerAddress { get; set; }
        public static string Expedition { get; set; }
        public static int Weight { get; set; }
        public static int SellerId { get; set; }
        public static int ProductId { get; set; }
        public static double TotalAmount { get; set; }
        public static List<int> ProductIds { get; set; } = new List<int>();

    }
}
