using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis
{
    public class RegionComparison
    {
        public long BaseRegionId { get; set; }

        public long RegionId { get; set; }

        public List<ItemComparison> ItemComparisons { get; set; } = new List<ItemComparison>();
    }
}
