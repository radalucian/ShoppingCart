using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingCart.Abstractions;
using ShoppingCart.Domain;
using AutoMapper;

namespace ShoppingCart.Business
{
    public class ShoppingBasketContext : IShoppingBasketContext
    {
        private static Dictionary<string, List<Product>> _baskets;

        private Dictionary<string, List<Product>> Baskets
            => _baskets ?? (_baskets = new Dictionary<string, List<Product>>());

        private readonly IProductContext _productContext;
        private readonly IDiscountContext _discountContext;

        public ShoppingBasketContext(IProductContext productContext, IDiscountContext discountContext)
        {
            _productContext = productContext;
            _discountContext = discountContext;
        }

        public string AddProduct(string cartName, int productId, int quantity)
        {
            // cauta produsul in lista de produse
            var availableProduct = this._productContext.GetProducts().FirstOrDefault(x => x.ProductId == productId);
 
            // verifica stocul
            if (quantity > availableProduct?.Stock)
                return String.Format("Error - Insufficient stock for product: {2}! Available stock for product is {0} and required stock is {1}", availableProduct?.Stock, quantity, availableProduct?.Name);

            if (this.Baskets.ContainsKey(cartName))
            {
                var existingProduct = this.Baskets[cartName].FirstOrDefault(x => x.ProductId == productId);

                if (existingProduct != null)
                {
                    this.Baskets[cartName].Remove(existingProduct);
                    quantity += existingProduct.Stock;
                    if (quantity > availableProduct?.Stock)
                        return String.Format("Error - Insufficient stock! By adding the required quantity,{0} , available stock is exceded by {1} number of products", quantity - existingProduct.Stock, quantity - availableProduct?.Stock);
                }
                if (availableProduct == null) return String.Format("Error - No product was found based on id {0}", productId);
                Product cartProduct = CreateCartProduct(availableProduct, quantity);
                this.Baskets[cartName].Add(cartProduct);
            }
            else
            {
                Product cartProduct = CreateCartProduct(availableProduct, quantity);
                this.Baskets.Add(cartName, new List<Product> { cartProduct });
            }

            return "";
        }

        public string AddProductWithDiscount(string cartName, int productId, int quantity, int discountID)
        {
            var availableProduct = this._productContext.GetProducts().FirstOrDefault(x => x.ProductId == productId);
            var availableProductDiscounts = this._discountContext.GetDiscounts().Where(x => x.DiscountId == discountID && x.ProductId == productId).ToList();

            // daca nu se gaseste pentru acel produs, discountul care a fost selectat/ales, vom da eroare
            if (availableProductDiscounts.Count() == 0)
                return "Error - Discount not available for product!";

            if (quantity > availableProduct?.Stock)        
                return String.Format("Error - Insufficient stock for product: {2}! Available stock for product is {0} and required stock is {1}", availableProduct?.Stock, quantity, availableProduct?.Name);
          

            availableProduct.AvailableDiscounts = availableProductDiscounts;

            if (this.Baskets.ContainsKey(cartName))
            {
                var existingProduct = this.Baskets[cartName].FirstOrDefault(x => x.ProductId == productId);

                if (existingProduct != null)
                {
                    this.Baskets[cartName].Remove(existingProduct);
                    quantity += existingProduct.Stock;
                    if (quantity > availableProduct?.Stock)
                        return String.Format("Error - Insufficient stock! By adding the required quantity,{0} , available stock is exceded by {1} number of products", quantity - existingProduct.Stock, quantity - availableProduct?.Stock);
                }
                if (availableProduct == null) return String.Format("Error - No product was found based on id {0}", productId);

                Product cartProduct = CreateCartProduct(availableProduct, quantity, true);
                this.Baskets[cartName].Add(cartProduct);
            }
            else
            {
                Product cartProduct = CreateCartProduct(availableProduct, quantity, true);
                this.Baskets.Add(cartName, new List<Product> { cartProduct });
            }

            return "";
        }

        public string Checkout(string cartName)
        {
            if (!this.Baskets.ContainsKey(cartName))
                return "Shoping cart was not found!";

            var shoppingBasket = this.Baskets[cartName];

            if (!CanCheckout(shoppingBasket))
                // aici ar trebui tratat custom fiecare caz: ori "s-a sters" (sau cineva face brute force) produsul din baza de date ori cantitatea disponibila e mai mica (s-au mai onorat si alte comenzi)
                return "One or more products does not exist or required quantity exceeds available stock quantity!";

            UpdateQuantities(shoppingBasket);

            ClearShoppingCart(cartName);

            // la Checkout ar trebui luat in considerare discount-ul pe fiecare produs in parte. Momentan, nu scriem un istoric al cumparaturilor, deci nu este cazul.

            return "";
        }

        // metoda verifica daca informatiile din cosul de cumparaturi sunt valide, corecte si cantitatile produselor sunt disponibile
        private bool CanCheckout(List<Product> shoppingBasket)
        {
            foreach (var cartProduct in shoppingBasket)
            {
                var realProduct = _productContext.GetProducts().FirstOrDefault(x => x.ProductId == cartProduct.ProductId);
                if (realProduct == null)
                    return false;

                if (realProduct.Stock < cartProduct.Stock)
                    return false;
            }
            return true;
        }

        // actualizeaza stocul pentru produsele care se afla in cosul de cumparaturi
        private void UpdateQuantities(List<Product> shoppingBasket)
        {
            foreach (var cartProduct in shoppingBasket)
            {
                var realProduct =
                    _productContext.GetProducts().FirstOrDefault(x => x.ProductId == cartProduct.ProductId);
                if (realProduct != null) realProduct.Stock -= cartProduct.Stock;
            }
        }

        // sterge cosul de cumparaturi
        private void ClearShoppingCart(string cartName)
        {
            this.Baskets.Remove(cartName);
        }

        // creaza obiectul pentru produsul din cosul de cumparaturi
        private Product CreateCartProduct(Product product, int quantity, bool discountIncluded = false)
        {
            //Initialize the mapper
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Product, Product>()
                );

            var mapper = new Mapper(config);
            var newCartProduct = mapper.Map<Product>(product);
            
            newCartProduct.Stock = quantity;

            // actualizam pretul final -> se tine cont de discount, daca e cazul
            SetProductFinalPrice(newCartProduct, discountIncluded);

            return newCartProduct;
        }

        public List<Product> GetShoppingCart(string cartName)
        {
            return !this.Baskets.ContainsKey(cartName) ? null : this.Baskets[cartName];
        }

        public string DeleteProductFromShoppingCart(string cartName, int productId, int quantity)
        {
            // cauta produsul in lista de produse
            var availableProduct = this._productContext.GetProducts().FirstOrDefault(x => x.ProductId == productId);

            if (availableProduct == null)
                return String.Format("Error - Product was not found based on id {0}", productId);

            if (this.Baskets.ContainsKey(cartName))
            {
                var existingProduct = this.Baskets[cartName].FirstOrDefault(x => x.ProductId == productId);

                if (existingProduct != null)
                {
                    if (quantity >= existingProduct.Stock)
                    {
                        this.Baskets[cartName].Remove(existingProduct);
                        
                        //in case the deleted item was the only product within the current shopping cart, shopping cart will be removed
                        if (this.Baskets[cartName].Count() == 0)
                        {
                            this.Baskets.Remove(cartName);
                        }
                    }
                    else
                    {
                        existingProduct.Stock -= quantity;
                    }
                }
                else
                {
                    return String.Format("Error - Product {0} was found in shopping cart {1}", availableProduct.Name, cartName);
                }
            }
            else
            {
                return String.Format("Error! Shopping cart was not found based on name {0}", cartName);
            }

            return "";
        }
        
        private void SetProductFinalPrice(Product product, bool discountIncluded)
        {
            product.FinalPrice = discountIncluded ? (product.Price - product.AvailableDiscounts[0].DiscountValue) * product.Stock : product.Price * product.Stock;
        }
    }
}
