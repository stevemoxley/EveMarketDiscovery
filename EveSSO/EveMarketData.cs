using EveSSO.Market.History;
using EveSSO.Market.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO
{
    public class EveMarketData
    {

        public EveMarketData(int itemLimit)
        {
            Console.WriteLine("Loading market history");
            RegionMarketHistories = MarketHistoryProvider.GetMarketHistoryFromWeb(itemLimit);
            Console.WriteLine("Loading market orders");
            RegionMarketOrders = MarketOrderProvider.GetMarketOrders(itemLimit, false);
        }

        public List<RegionMarketHistory> RegionMarketHistories { get; set; }

        public List<RegionMarketOrders> RegionMarketOrders { get; set; }

    }
}
