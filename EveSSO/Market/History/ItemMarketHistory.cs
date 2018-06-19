using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO.Market.History
{
    public class ItemMarketHistory
    {

        public long ItemId { get; set; }

        public long RegionId { get; set; }

        public MarketHistory[] MarketHistory { get; set; }
    }
}
