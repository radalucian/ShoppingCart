using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ShoppingCart.Abstractions;
using ShoppingCart.Domain;

namespace ShoppingCart.Service.Controllers
{
    public class ShoppingBasketController : ApiController
    {
        private readonly IShoppingBasketContext _shoppingBasketContext;

        public ShoppingBasketController(IShoppingBasketContext shoppingBasketContext)
        {
            _shoppingBasketContext = shoppingBasketContext;
        }

        [Route("api/ShoppingBasket/{cartname}")]
        [HttpGet]
        public IEnumerable<Product> Get(string cartName)
        {
            // poate sa fie si null - ar trebui tratat sa returnam tot un obiect de eroare in cazul asta - to Do - for the future
            var shoppingBasket = _shoppingBasketContext.GetShoppingCart(cartName);
            return shoppingBasket;
        }

        [Route("api/ShoppingBasket/{cartname}/Checkout")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult CheckOut(string cartName)
        {
            var checkoutResult = _shoppingBasketContext.Checkout(cartName);
          
            // to Do - obiectul de eroare returnat sa fie completat automat in metoda de nivelul inferior
            ApiCustomError errorObj = new ApiCustomError();
            errorObj.isError = !string.IsNullOrEmpty(checkoutResult);
            errorObj.errorDescription = checkoutResult;

            // to Do - in functie de tipul de eroare, HTTPStatusCode sa fie altul decat ok - 200
            return Ok(errorObj);
        }

        [Route("api/ShoppingBasket/{cartname}/Add/{productId}/{quantity}")]
        [HttpGet]
        [HttpPut]
        public IHttpActionResult AddProduct(string cartName, int productId, int quantity)
        {
            var addingResult = _shoppingBasketContext.AddProduct(cartName, productId, quantity);
          
            // to Do - obiectul de eroare returnat sa fie completat automat in metoda de nivelul inferior
            ApiCustomError errorObj = new ApiCustomError();
            errorObj.isError = !string.IsNullOrEmpty(addingResult);
            errorObj.errorDescription = addingResult;

            // to Do - in functie de tipul de eroare, HTTPStatusCode sa fie altul decat ok - 200
            return Ok(errorObj);
        }

        [Route("api/ShoppingBasket/{cartname}/Add/{productId}/{quantity}/{discountId}")]
        [HttpGet]
        [HttpPut]
        public IHttpActionResult AddProductWithDiscount(string cartName, int productId, int quantity, int discountId)
        {
            var addingWithDiscountResult = _shoppingBasketContext.AddProductWithDiscount(cartName, productId, quantity, discountId);

            // to Do - obiectul de eroare returnat sa fie completat automat in metoda de nivelul inferior
            ApiCustomError errorObj = new ApiCustomError();
            errorObj.isError = !string.IsNullOrEmpty(addingWithDiscountResult);
            errorObj.errorDescription = addingWithDiscountResult;

            // to Do - in functie de tipul de eroare, HTTPStatusCode sa fie altul decat ok - 200
            return Ok(errorObj);
        }
        
        [Route("api/ShoppingBasket/{cartname}/Delete/{productId}/{quantity}")]
        [HttpGet]
        [HttpDelete]
        public IHttpActionResult DeleteProductFromShoppingCart(string cartName, int productId, int quantity)
        {
            var deleteProductResult = _shoppingBasketContext.DeleteProductFromShoppingCart(cartName, productId, quantity);

            // to Do - obiectul de eroare returnat sa fie completat automat in metoda de nivelul inferior
            ApiCustomError errorObj = new ApiCustomError();
            errorObj.isError = !string.IsNullOrEmpty(deleteProductResult);
            errorObj.errorDescription = deleteProductResult;

            // to Do - in functie de tipul de eroare, HTTPStatusCode sa fie altul decat ok - 200
            return Ok(errorObj);
        }

    }
}
