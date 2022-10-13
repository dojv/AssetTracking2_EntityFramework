using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2_EF
{
    public class Product
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double PriceDollar { get; set; }
        public double PriceLocal { get; set; }
        public string Currency { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Office Office { get; set; }

        public void SetPriceLocal(User user, double priceDollar)
        {
            string currency = this.Office.Currency;
            this.PriceLocal = priceDollar * user.Currencies[currency];
            this.Currency = currency;
        }
        public void SetPriceDollar(User user, double priceLocal)
        {
            string currency = this.Office.Currency;
            this.PriceDollar = priceLocal / user.Currencies[currency];
            this.Currency = currency;
        }
        public void SetCurrency()
        {
            this.Currency = this.Office.Currency;
        }
    }
}
