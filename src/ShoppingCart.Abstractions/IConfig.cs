namespace ShoppingCart.Abstractions
{
    public interface IConfig
    {
        string GetProductsDataSourcePath();
        string GetDiscountsDataSourcePath();
    }
}
