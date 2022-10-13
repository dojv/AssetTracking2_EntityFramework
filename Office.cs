using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2_EF
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public List<Product> Products = new List<Product>();
    }
}
