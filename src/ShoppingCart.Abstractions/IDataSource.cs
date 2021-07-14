using System.Collections.Generic;
using ShoppingCart.Domain;

namespace ShoppingCart.Abstractions
{
    public interface IDataSource
    {
        IEnumerable<Product> LoadProducts(string pathProducts);
        IEnumerable<Discount> LoadDiscounts(string pathDiscounts);
    }
}
