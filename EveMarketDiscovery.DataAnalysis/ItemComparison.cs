using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis
{
    public class ItemComparison
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
                return Math.Abs(AveragePrice - BaseAveragePrice);
            }
        }

        public float ProfitMargin
        {
            get
            {
                var max = Math.Max(AveragePrice, BaseAveragePrice);
                return (PriceDifference / max) * 100;
            }
        }

        public float AverageVolumeProfitMarginPotential
        {
            get
            {
                var averageVolume = (BaseVolume + Volume) / 2;
                return averageVolume * ProfitMargin;
            }
        }

        public float PotentialDailyProfit
        {
            get
            {
                if(BaseVolume > Volume)
                {
                    return BaseVolume * PriceDifference;
                }
                else
                {
                    return Volume * PriceDifference;
                }
            }
        }

        public long BaseVolume { get; set; }

        public long Volume { get; set; }

    }
}
