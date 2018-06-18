using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis
{
    public class EveMarketDataAnalysis
    {

        public List<RegionComparison> RegionComparisons = new List<RegionComparison>();

        public List<ItemComparison> GetTopItemComparisons(int numberToGet = -1, long singularRegionId = 0, float minimumProfitMargin = 0, float maximumProfitMargin = 0)
        {
            var result = new List<ItemComparison>();

            foreach (var item in RegionComparisons)
            {
                result.AddRange(item.ItemComparisons);
            }

            result = result.OrderByDescending(s => s.ProfitMargin).ToList();

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

            if (numberToGet > -1)
            {
                result = result.Take(numberToGet).ToList();
            }

            return result;
        }


    }
}
