using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2_EF
{
    public class Mobile : Product
    {
        public Mobile() { }
        public Mobile(string productType, string brand, string model, double priceDollar, DateTime purchaseDate, Office office)
        {
            this.Type = productType;
            this.Brand = brand;
            this.Model = model;
            this.PriceDollar = priceDollar;
            this.PurchaseDate = purchaseDate;
            this.Office = office;
        }
    }
}
