using System.Collections.Generic;

namespace ShoppingCart.Domain
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        
        public string Image { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        
        public decimal FinalPrice { get; set; }

        public int Stock { get; set; }
       
        public List<Discount> AvailableDiscounts { get; set; }

}



}
