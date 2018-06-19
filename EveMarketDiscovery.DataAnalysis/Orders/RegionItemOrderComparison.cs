using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis.Orders
{
    public class RegionItemOrderComparison
    {

        public long RegionId { get; set; }

        public long BaseRegionId { get; set; }

        public List<ItemOrderComparison> ItemOrderComparisons = new List<ItemOrderComparison>();
    }
}
