using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO
{
    public class EveMarketData
    {

        public EveMarketData(int itemLimit)
        {
            RegionMarketHistories = MarketHistoryProvider.GetMarketHistoryFromCache(itemLimit);
        }

        public List<RegionMarketHistory> RegionMarketHistories { get; set; }

    }
}
