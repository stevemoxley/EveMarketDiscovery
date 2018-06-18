using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO
{
    public class RegionMarketHistory
    {
        public long RegionId { get; set; }

        public List<ItemMarketHistory> ItemMarketHistories { get; set; } = new List<ItemMarketHistory>();



    }
}
