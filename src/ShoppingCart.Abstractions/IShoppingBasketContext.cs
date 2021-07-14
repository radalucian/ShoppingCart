using System.Collections.Generic;
using ShoppingCart.Domain;

namespace ShoppingCart.Abstractions
{
    public interface IShoppingBasketContext
    {
        string AddProduct(string cartName, int productId, int quantity);

        string AddProductWithDiscount(string cartName, int productId, int quantity, int discountID);

        string Checkout(string cartName);

        string DeleteProductFromShoppingCart(string cartName, int productId, int quantity);

        List<Product> GetShoppingCart(string cartName);
    }
}
