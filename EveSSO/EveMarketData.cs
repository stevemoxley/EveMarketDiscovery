﻿using EveSSO.Market.History;
using EveSSO.Market.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EveSSO
{
    public class EveMarketData
    {

        public EveMarketData(int itemLimit)
        {
            LoadAllData(itemLimit);
        }

        public void LoadAllData(int itemLimit)
        {
            Console.WriteLine("Loading market history");
            RegionMarketHistories = GetRegionMarketHistories(itemLimit);
            Console.WriteLine("Loading market orders");
            RegionMarketOrders = GetRegionMarketOrders(itemLimit);
        }

        public List<RegionMarketOrders> GetRegionMarketOrders(int itemLimit)
        {
            return MarketOrderProvider.GetMarketOrders(itemLimit, false);
        }

        public List<RegionMarketHistory> GetRegionMarketHistories(int itemLimit)
        {
            return MarketHistoryProvider.GetMarketHistoryFromWeb(itemLimit);
        }

        public List<RegionMarketHistory> RegionMarketHistories { get; set; }

        public List<RegionMarketOrders> RegionMarketOrders { get; set; }

    }
}
