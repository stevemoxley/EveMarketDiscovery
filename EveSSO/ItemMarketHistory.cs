using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO
{
    public class ItemMarketHistory
    {

        public long ItemId { get; set; }

        public string ItemName { get; set; }

        public MarketHistory[] MarketHistory { get; set; }
    }
}
