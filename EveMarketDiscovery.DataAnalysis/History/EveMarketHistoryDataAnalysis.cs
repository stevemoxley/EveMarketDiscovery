using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis.History
{
    public class EveMarketHistoryDataAnalysis
    {

        public List<RegionItemHistoryComparison> RegionComparisons = new List<RegionItemHistoryComparison>();

        public List<ItemHistoryComparison> GetTopItemComparisons(int numberToGet = -1, long singularRegionId = 0, float minimumProfitMargin = 0, float maximumProfitMargin = 0, int minVolume = 0)
        {
            var result = new List<ItemHistoryComparison>();

            foreach (var item in RegionComparisons)
            {
                result.AddRange(item.ItemComparisons);
            }

            result = result.OrderByDescending(s => s.PotentialDailyProfit).ToList();

            if(singularRegionId > 0)
            {
                result = result.Where(r => r.RegionId == singularRegionId).ToList();
            }

            if(minimumProfitMargin > 0)
            {
                result = result.Where(r => r.ProfitMargin >= minimumProfitMargin).ToList();
            }

            if(maximumProfitMargin > 0)
            {
                result = result.Where(r => r.ProfitMargin <= minimumProfitMargin).ToList();
            }


            if (minVolume > 0)
            {
                result = result.Where(r => r.Volume >= minVolume).ToList();
            }

            if (numberToGet > -1)
            {
                result = result.Take(numberToGet).ToList();
            }


            return result;
        }


    }
}
