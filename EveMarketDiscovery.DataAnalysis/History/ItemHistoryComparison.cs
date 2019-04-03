using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis.History
{
    public class ItemHistoryComparison
    {
        public long BaseRegionId { get; set; }

        public long RegionId { get; set; }

        public long ItemId { get; set; }

        public string ItemName { get; set; }

        public float BaseAveragePrice { get; set; }

        public float AveragePrice { get; set; }

        public float PriceDifference
        {
            get
            {
                return AveragePrice - BaseAveragePrice;
            }
        }

        public float ProfitMargin
        {
            get
            {
                return (PriceDifference / AveragePrice) * 100;
            }
        }

        public float PotentialDailyProfit
        {
            get
            {
                return Volume * PriceDifference;
            }
        }

        public long BaseVolume { get; set; }

        public long Volume { get; set; }

    }
}
