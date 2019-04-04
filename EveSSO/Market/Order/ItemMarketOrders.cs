using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO.Market.Order
{
    public class ItemMarketOrders
    {
        public long ItemId { get; set; }

        public long RegionId { get; set; }

        public MarketOrder[] MarketOrders { get; set; }

        public ItemMarketOrders()
        {
            MarketOrders = new MarketOrder[0];
        }
    }
}
