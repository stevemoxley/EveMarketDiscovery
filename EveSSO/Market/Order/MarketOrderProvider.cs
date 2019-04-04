using EveAccountant.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EveSSO.Market.Order
{
    public static class MarketOrderProvider
    {
        public static List<RegionMarketOrders> GetMarketOrders(int itemLimit, bool cacheOnly = false)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 250;
            var sw = new Stopwatch();
            sw.Start();

            List<RegionMarketOrders> result = new List<RegionMarketOrders>();
            foreach (var region in Regions)
            {
                result.Add(GetRegionMarketOrders(region, itemLimit, cacheOnly));
            }

            sw.Stop();
            Console.WriteLine($"Downloaded { result.Sum(f => f.ItemMarketOrders.Count) } order histories in { (sw.ElapsedMilliseconds / 1000) } s");

            return result;
        }

        private static RegionMarketOrders GetRegionMarketOrders(long regionId, int itemLimit = -1, bool cacheOnly = false)
        {
            RegionMarketOrders result = new RegionMarketOrders();
            result.RegionId = regionId;

            var items = ItemProvider.Items();

            if (itemLimit > -1)
            {
                items = items.Take(itemLimit).ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            List<Task<ItemMarketOrders>> allTasks = new List<Task<ItemMarketOrders>>();

            foreach (var item in items)
            {
                allTasks.Add(GetItemMarketOrdersAsync(regionId, item.Key, cacheOnly));
            }

            Task.WaitAll(allTasks.ToArray());

            foreach (var task in allTasks)
            {
                result.ItemMarketOrders.Add(task.Result);
            }

            return result;
        }

        public static async Task<ItemMarketOrders> GetItemMarketOrdersAsync(long regionId, long itemId, bool cacheOnly = false)
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
                if (fileAge.TotalHours >= 24 && !cacheOnly)
                {
                    ordersJson = await GetMarketOrderJsonFromWebAsync(regionId, itemId);
                }
                else
                {
                    //Console.WriteLine($"Pulled orders from cache for {regionId} - { itemId }");
                    using (var reader = File.OpenText(fileFormat))
                    {
                        ordersJson = await reader.ReadToEndAsync();
                    }
                }
            }
            else
            {
                if (!cacheOnly)
                    ordersJson = await GetMarketOrderJsonFromWebAsync(regionId, itemId);
            }

            result.MarketOrders = JsonConvert.DeserializeObject<MarketOrder[]>(ordersJson);


            return result;
        }

        private static async Task<string> GetMarketOrderJsonFromWebAsync(long regionId, long itemId)
        {
            try
            {
                if (!Directory.Exists(_ordersCacheDirectory))
                {
                    Directory.CreateDirectory(_ordersCacheDirectory);
                }

               // Console.WriteLine($"Downloading web orders for {regionId} - {itemId}");
                string fileFormat = $"{_ordersCacheDirectory}/{regionId}_{itemId}.txt";
                using (var webClient = new WebClient())
                {
                    var uri = new Uri($"https://esi.evetech.net/latest/markets/{ regionId }/orders/?datasource=tranquility&type_id={ itemId }");
                    string marketDataJson = await webClient.DownloadStringTaskAsync(uri);
                    File.WriteAllText(fileFormat, marketDataJson);

                    return marketDataJson;
                }
            }
            catch (WebException webEx)
            {
                HttpWebResponse response = (HttpWebResponse)webEx.Response;
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Item { itemId } not found. Adding to ignore list.");
                    ItemProvider.AddToIgnoreList(itemId);
                }
                else
                {
                    Console.WriteLine($"Exception from web: { webEx.Message } on item: { itemId }");
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: { ex.Message }");
                return string.Empty;
            }

        }

        private static string _ordersCacheDirectory = "cache/orders";

        private static long[] Regions = { 10000002, /*10000043, 10000030,*/ 10000042 };

    }
}
