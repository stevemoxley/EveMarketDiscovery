using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using Common;
using System.Linq;

namespace EveSSO
{
    public static class MarketHistoryProvider
    {

        public static MarketHistory[] GetMarketHistory(long regionId, long itemId, bool cacheOnly)
        {
            //Try to get cached data
            string marketDataJson = string.Empty;
            if (!Directory.Exists("cache"))
            {
                Directory.CreateDirectory("cache");
            }
            string fileFormat = $"cache/{regionId}_{itemId}.txt";
            if (File.Exists(fileFormat))
            {
                //Check the age of the file
                var fileInfo = new FileInfo(fileFormat);
                var lastEditSpan = DateTime.Now - fileInfo.LastWriteTime;
                if (lastEditSpan.TotalDays >= 1)
                {
                    //Get from the web
                    if (!cacheOnly)
                    {
                        marketDataJson = GetHistoryJsonFromWeb(regionId, itemId);
                    }
                }
                else
                {
                    marketDataJson = File.ReadAllText(fileFormat);
                }
            }
            else
            {
                if (!cacheOnly)
                {
                    marketDataJson = GetHistoryJsonFromWeb(regionId, itemId);
                }
            }

            var marketData = JsonConvert.DeserializeObject<MarketHistory[]>(marketDataJson);
            return marketData;
        }


        public static List<RegionMarketHistory> GetMarketHistoryFromCache(int itemLimit)
        {
            var result = new List<RegionMarketHistory>();
            foreach (var region in Regions)
            {
                result.Add(GetRegionMarketHistory(region, true, itemLimit));
            }

            return result;
        }

        public static List<RegionMarketHistory> GetMarketHistoryFromWeb(int itemLimit)
        {
            var result = new List<RegionMarketHistory>();
            foreach (var region in Regions)
            {
                result.Add(GetRegionMarketHistory(region, false, itemLimit));
            }

            return result;
        }



        private static RegionMarketHistory GetRegionMarketHistory(long regionId, bool cacheOnly, int itemLimit)
        {
            RegionMarketHistory result = new RegionMarketHistory();
            result.RegionId = regionId;

            var items = ItemProvider.Items();

            if (itemLimit > -1)
            {
                items = items.Take(itemLimit).ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            foreach (var item in items)
            {
                var itemHistory = GetItemMarketHistory(regionId, item.Key, item.Value, cacheOnly);
                if (itemHistory == null)
                {
                    continue;
                }
                result.ItemMarketHistories.Add(itemHistory);
            }

            return result;
        }

        private static ItemMarketHistory GetItemMarketHistory(long regionId, long itemId, string itemName, bool cacheOnly)
        {
            ItemMarketHistory itemMarketHistory = new ItemMarketHistory();
            itemMarketHistory.ItemId = itemId;
            itemMarketHistory.ItemName = itemName;
            itemMarketHistory.MarketHistory = GetMarketHistory(regionId, itemId, cacheOnly);
            return itemMarketHistory;
        }


        private static string GetHistoryJsonFromWeb(long regionId, long itemId)
        {
            try
            {
                Console.WriteLine($"Downloading web history for {regionId} - {itemId}");
                string fileFormat = $"cache/{regionId}_{itemId}.txt";
                var webClient = new WebClient();
                string marketDataJson = webClient.DownloadString($"https://esi.evetech.net/latest/markets/{ regionId }/history/?datasource=tranquility&type_id={ itemId }");
                File.WriteAllText(fileFormat, marketDataJson);

                return marketDataJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception from web: { ex.Message }");
                return string.Empty;
            }
        }

        private static long[] Regions = { 10000002, 10000043, 10000030, 10000042 };
    }
}
