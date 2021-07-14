using System.Configuration;
using ShoppingCart.Abstractions;

namespace ShoppingCart.Common
{
    public class ShoppingCartConfig : IConfig
    {

        public string GetDiscountsDataSourcePath()
        {
            return ConfigurationManager.AppSettings["ShoppingCart.CsvDiscountsFilePath"];
        }

        public string GetProductsDataSourcePath()
        {
            return ConfigurationManager.AppSettings["ShoppingCart.CsvProductsFilePath"];
        }
    }
}
