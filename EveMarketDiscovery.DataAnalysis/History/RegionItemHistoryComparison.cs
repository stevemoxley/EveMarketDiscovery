using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis.History
{
    public class RegionItemHistoryComparison
    {
        public long BaseRegionId { get; set; }

        public long RegionId { get; set; }

        public List<ItemHistoryComparison> ItemComparisons { get; set; } = new List<ItemHistoryComparison>();
    }
}
