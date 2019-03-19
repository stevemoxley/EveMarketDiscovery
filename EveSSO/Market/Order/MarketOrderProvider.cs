using EveAccountant.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EveSSO.Market.Order
{
    public static class MarketOrderProvider
    {
        public static List<RegionMarketOrders> GetMarketOrders(int itemLimit, bool cacheOnly = false)
        {
            List<RegionMarketOrders> result = new List<RegionMarketOrders>();
            foreach (var region in Regions)
            {
                result.Add(GetRegionMarketOrders(region, itemLimit, cacheOnly));
            }

            return result;
        }

        public static RegionMarketOrders GetRegionMarketOrders(long regionId, int itemLimit = -1, bool cacheOnly = false)
        {
            RegionMarketOrders result = new RegionMarketOrders();
            result.RegionId = regionId;
         
            var items = ItemProvider.Items();

            if (itemLimit > -1)
            {
                items = items.Take(itemLimit).ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            foreach (var item in items)
            {
                result.ItemMarketOrders.Add(GetItemMarketOrders(regionId, item.Key, cacheOnly));
            }

            return result;
        }

        public static ItemMarketOrders GetItemMarketOrders(long regionId, long itemId, bool cacheOnly = false)
        {
            ItemMarketOrders result = new ItemMarketOrders();
            result.RegionId = regionId;
            result.ItemId = itemId;
            string ordersJson = string.Empty;
            string fileFormat = $"{_ordersCacheDirectory}/{regionId}_{itemId}.txt";

            //Caching check
            if (File.Exists(fileFormat))
            {
                //Check the age of the file
                var fileInfo = new FileInfo(fileFormat);
                var fileAge = DateTime.Now - fileInfo.LastWriteTime;
                if(fileAge.TotalHours >= 24 && !cacheOnly)
                {
                    ordersJson = GetMarketOrderJsonFromWeb(regionId, itemId);
                }
                else
                {
                    ordersJson = File.ReadAllText(fileFormat);
                }
            }
            else
            {
                if(!cacheOnly)
                    ordersJson = GetMarketOrderJsonFromWeb(regionId, itemId);
            }

            result.MarketOrders = JsonConvert.DeserializeObject<MarketOrder[]>(ordersJson);


            return result;
        }

        private static string GetMarketOrderJsonFromWeb(long regionId, long itemId)
        {
            try
            {
                if (!Directory.Exists(_ordersCacheDirectory))
                {
                    Directory.CreateDirectory(_ordersCacheDirectory);
                }

                Console.WriteLine($"Downloading web orders for {regionId} - {itemId}");
                string fileFormat = $"{_ordersCacheDirectory}/{regionId}_{itemId}.txt";
                var webClient = new WebClient();
                string marketDataJson = webClient.DownloadString($"https://esi.evetech.net/latest/markets/{ regionId }/orders/?datasource=tranquility&type_id={ itemId }");
                File.WriteAllText(fileFormat, marketDataJson);

                return marketDataJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception from web: { ex.Message }");
                return string.Empty;
            }

        }

        private static string _ordersCacheDirectory = "cache/orders";

        private static long[] Regions = { 10000002, /*10000043, 10000030,*/ 10000042 };

    }
}
