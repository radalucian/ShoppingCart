using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using ShoppingCart.Abstractions;
using ShoppingCart.Domain;

namespace ShoppingCart.Common
{
    public class CsvDataSource : IDataSource
    {
        public IEnumerable<Discount> LoadDiscounts(string pathDiscounts)
        {
            List<Discount> discounts = new List<Discount>();

            if (File.Exists(pathDiscounts))
            {
                CsvReader csvReader = new CsvReader(File.OpenText(pathDiscounts));

                try
                {
                    discounts = csvReader.GetRecords<Discount>().ToList();
                }
                catch
                {
                    throw new System.ApplicationException("Found Discounts.csv file does not respect the needed format!");
                }
                finally
                {
                    csvReader.Dispose();
                }
            }
            else
            {
                // in mod normal, ar exista o tabela de loguri ale aplicatiei (disponibila la nivel de BO, pentru un user cu un drept de administrare) in care se scriu aceste informatii.
                // Nu ar trebui ca aplicatia sa se opreasca la o exceptie precum cea din cazul de fata. Similar pt toate erorile din acest modul
                throw new System.ApplicationException(string.Format("Discounts.csv file does not exist at location configured within application. Required location is: {0}!", pathDiscounts));
            }

            return discounts;
        }

        public IEnumerable<Product> LoadProducts(string pathProducts)
        {
            List<Product> products = new List<Product>();

            if (File.Exists(pathProducts))
            {
                CsvReader csvReader = new CsvReader(File.OpenText(pathProducts));

                try
                {
                    products = csvReader.GetRecords<Product>().ToList();
                }
                catch
                {
                    throw new System.ApplicationException("Found Products.csv file does not respect the needed format!");
                }
                finally
                {
                    csvReader.Dispose();
                }
            }
            else
            {
                throw new System.ApplicationException(string.Format("Products.csv file does not exist at location configured within application. Required location is: {0}!", pathProducts));
            }
            return products;
        }
    }
}
