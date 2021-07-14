using System.Collections.Generic;
using ShoppingCart.Domain;

namespace ShoppingCart.Abstractions
{
    public interface IDiscountContext
    {
        IEnumerable<Discount> GetDiscounts();
    }
}
