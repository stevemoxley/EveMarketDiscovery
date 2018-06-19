using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO.Market.Order
{
    public class RegionMarketOrders
    {
        public long RegionId { get; set; }

        public List<ItemMarketOrders> ItemMarketOrders { get; set; } = new List<ItemMarketOrders>();
    }
}
