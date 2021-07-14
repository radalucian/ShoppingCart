using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain
{
    public class Discount
    {
        public int DiscountId { get; set; }

        public int ProductId { get; set; }

        public decimal DiscountValue { get; set; }
    }
}
