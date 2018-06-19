using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis.Orders
{
    public class ItemOrderComparison
    {
        public float Price { get; set; }

        public float BasePrice { get; set; }

        public float Difference { get; set; }

        public float ProfitMargin
        {
            get
            {
                return Math.Abs(Difference) / Math.Max(BasePrice, Price);
            }
        }

    }
}
