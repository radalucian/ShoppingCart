using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ShoppingCart.Abstractions;
using ShoppingCart.Domain;

namespace ShoppingCart.Service.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductContext _productContext;
        private readonly IDiscountContext _discountContext;

        public ProductsController(IProductContext productContext, IDiscountContext discountContext)
        {
            _productContext = productContext;
            _discountContext = discountContext;
        }

        // GET api/products
        public IEnumerable<Product> Get()
        {
            var listOfProducts = _productContext.GetProducts();
            var listOfDiscounts = _discountContext.GetDiscounts();
           
            foreach (Product product in listOfProducts)
            {
                product.AvailableDiscounts = listOfDiscounts.Where(x => x.ProductId == product.ProductId).ToList();
            }


            return listOfProducts;
        }

        // GET api/products/5
        public Product Get(int id)
        {
            var foundProduct =  _productContext.GetProducts().FirstOrDefault(x => x.ProductId == id);
            var listOfDiscounts = _discountContext.GetDiscounts().Where(x => x.ProductId == id).ToList();

            foundProduct.AvailableDiscounts = listOfDiscounts;

            return foundProduct;

        }
        }
}
