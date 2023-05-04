using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UsertID { get; set; }
        public DateTime? OrderDate { get; set; }
        public Decimal? OrderCost { get; set; }
        public string? ItemsDescription { get; set; }
        public string? ShippingAddress { get; set; }
        public Order() { }
    }
}
