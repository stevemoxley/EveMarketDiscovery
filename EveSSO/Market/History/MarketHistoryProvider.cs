using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Linq;
using EveAccountant.Common;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EveSSO.Market.History
{
    public static class MarketHistoryProvider
    {

        public static List<RegionMarketHistory> GetMarketHistoryFromWeb(int itemLimit)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 160;
            var sw = new Stopwatch();
            sw.Start();

            var result = new List<RegionMarketHistory>();
            foreach (var region in Regions)
            {
                result.Add(GetRegionMarketHistory(region, false, itemLimit));
            }

            sw.Stop();
            Console.WriteLine($"Downloaded { result.Sum(f=>f.ItemMarketHistories.Count) } items in { (sw.ElapsedMilliseconds / 1000) } s");

            return result;
        }

        private static async Task<MarketHistory[]> GetMarketHistoryAsync(long regionId, long itemId, bool cacheOnly)
        {
            //Try to get cached data
            string marketDataJson = string.Empty;
            if (!Directory.Exists("cache/history"))
            {
                Directory.CreateDirectory("cache/history");
            }
            string fileFormat = $"cache/history/{regionId}_{itemId}.txt";
            if (File.Exists(fileFormat))
            {
                //Check the age of the file
                var fileInfo = new FileInfo(fileFormat);
                var lastEditSpan = DateTime.Now - fileInfo.LastWriteTime;
                if (lastEditSpan.TotalDays >= 1 && !cacheOnly)
                {
                    marketDataJson = await GetHistoryJsonFromWebAsync(regionId, itemId);
                }
                else
                {
                    Console.WriteLine($"Pulled cached history for {regionId} - {itemId}");
                    marketDataJson = File.ReadAllText(fileFormat);
                }
            }
            else
            {
                if (!cacheOnly)
                {
                    marketDataJson = await GetHistoryJsonFromWebAsync(regionId, itemId);
                }
            }

            var marketData = JsonConvert.DeserializeObject<MarketHistory[]>(marketDataJson);
            return marketData;
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

            List<Task<ItemMarketHistory>> marketHistoryTasks = new List<Task<ItemMarketHistory>>();
            foreach (var item in items)
            {
                marketHistoryTasks.Add(GetItemMarketHistoryAsync(regionId, item.Key, cacheOnly));
            }

            Task.WaitAll(marketHistoryTasks.ToArray());

            foreach (var task in marketHistoryTasks)
            {
                result.ItemMarketHistories.Add(task.Result);
            }

            return result;
        }

        private static async Task<ItemMarketHistory> GetItemMarketHistoryAsync(long regionId, long itemId, bool cacheOnly)
        {
            ItemMarketHistory itemMarketHistory = new ItemMarketHistory();
            itemMarketHistory.ItemId = itemId;
            itemMarketHistory.MarketHistory = await GetMarketHistoryAsync(regionId, itemId, cacheOnly);
            itemMarketHistory.RegionId = regionId;
            return itemMarketHistory;
        }

        private static async Task<string> GetHistoryJsonFromWebAsync(long regionId, long itemId)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    string fileFormat = $"cache/history/{regionId}_{itemId}.txt";
                    var uri = new Uri($"https://esi.evetech.net/latest/markets/{ regionId }/history/?datasource=tranquility&type_id={ itemId }");
                    string marketDataJson = await webClient.DownloadStringTaskAsync(uri);

                    Console.WriteLine($"Downloaded web history for {regionId} - {itemId}");

                    File.WriteAllText(fileFormat, marketDataJson);

                    return marketDataJson;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception from web: { ex.Message } on item: { itemId }");
                return string.Empty;
            }
        }

        private static string GetHistoryJsonFromWeb(long regionId, long itemId)
        {
            try
            {
                Console.WriteLine($"Downloading web history for {regionId} - {itemId}");
                string fileFormat = $"cache/history/{regionId}_{itemId}.txt";
                using (var webClient = new WebClient())
                {
                    string marketDataJson = webClient.DownloadString($"https://esi.evetech.net/latest/markets/{ regionId }/history/?datasource=tranquility&type_id={ itemId }");
                    File.WriteAllText(fileFormat, marketDataJson);

                    return marketDataJson;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception from web: { ex.Message }");
                return string.Empty;
            }
        }

        private static long[] Regions = { 10000002, /*10000043, 10000030,*/ 10000042 };

    }
}
