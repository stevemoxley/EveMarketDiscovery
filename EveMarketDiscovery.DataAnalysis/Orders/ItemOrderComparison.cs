using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis.Orders
{
    public class ItemOrderComparison
    {

        public long Id { get; set; }
        public string Name { get; set; }

        public float Price { get; set; }

        public float BasePrice { get; set; }

        public float TotalMarketInventory { get; set; }

        public float TotalBaseMarketInventory { get; set; }

        public float WeightedAveragePrice { get; set; }

        public float WeightedAverageBasePrice { get; set; }

        public float Difference
        {
            get
            {
                return Price - BasePrice;
            }
        }

        public float WeightedAverageDifferece
        {
            get
            {
                return WeightedAveragePrice - WeightedAverageBasePrice;
            }
        }

        public float ProfitMargin
        {
            get
            {
                return (Difference / Price) * 100;
            }
        }

        public float WeightedAverageProfitMargin
        {
            get
            {
                return (WeightedAverageDifferece / WeightedAveragePrice) * 100;
            }
        }

        public long AverageVolume { get; set; }

        public decimal AverageDailyProfitPotential
        {
            get
            {
                return (decimal)(Difference * AverageVolume);
            }
        }

        public decimal PercentMarketInventorySold { get; set; }

    }
}
