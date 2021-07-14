using System.Collections.Generic;
using ShoppingCart.Abstractions;
using ShoppingCart.Domain;

namespace ShoppingCart.Business
{
    public class DiscountContext: IDiscountContext
    {
        private static IEnumerable<Discount> _discounts;

        private IEnumerable<Discount> Discounts
            => _discounts ?? (_discounts = _dataSource.LoadDiscounts(_config.GetDiscountsDataSourcePath()));

        private readonly IDataSource _dataSource;

        private readonly IConfig _config;

        public DiscountContext(IDataSource dataSource, IConfig config)
        {
            _dataSource = dataSource;
            _config = config;
        }

        public IEnumerable<Discount> GetDiscounts()
        {
            return this.Discounts;
        }
    }
}
