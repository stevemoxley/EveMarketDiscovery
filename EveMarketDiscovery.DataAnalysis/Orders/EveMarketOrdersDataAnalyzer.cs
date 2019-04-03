using EveAccountant.Common;
using EveSSO;
using EveSSO.Market.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis.Orders
{
    public class EveMarketOrdersDataAnalyzer
    {
        public EveMarketOrdersDataAnalyzer(EveMarketData eveMarketData)
        {
            _eveMarketData = eveMarketData;
        }

        public EveMarketOrdersDataAnalysis GetAnalysis(long regionId, int itemLimit)
        {

            var baseRegionId = 10000002; //Jita

            EveMarketOrdersDataAnalysis result = new EveMarketOrdersDataAnalysis();
            result.BaseRegionId = baseRegionId;
            result.RegionId = regionId;


            var items = ItemProvider.Items();

            if (itemLimit > 0)
            {
                items = items.Take(itemLimit).ToDictionary(i => i.Key, j => j.Value);
            }

            var regionOrders = _eveMarketData.RegionMarketOrders.FirstOrDefault(r => r.RegionId == regionId);
            var baseRegionOrders = _eveMarketData.RegionMarketOrders.FirstOrDefault(r => r.RegionId == baseRegionId);

            var regionHistory = _eveMarketData.RegionMarketHistories.FirstOrDefault(r => r.RegionId == regionId);

            foreach (var item in items)
            {

                var itemHistory = regionHistory.ItemMarketHistories.FirstOrDefault(h => h.ItemId == item.Key);

                //Get all the orders for this item
                var orders = regionOrders.ItemMarketOrders.FirstOrDefault(i => i.ItemId == item.Key).MarketOrders.ToList();
                var baseOrders = baseRegionOrders.ItemMarketOrders.FirstOrDefault(i => i.ItemId == item.Key).MarketOrders.ToList();

                var sellOrders = orders.Where(o => !o.is_buy_order).OrderBy(p => p.price).ToList();
                var baseSellOrders = baseOrders.Where(o => !o.is_buy_order).OrderBy(p => p.price).ToList();

                float sellOrderWeightedAveragePrice = CalculateWeightedAverage(sellOrders);
                float baseSellOrderWeightedAveragePrice = CalculateWeightedAverage(baseSellOrders);

                long totalMarketInventory = sellOrders.Sum(t => t.volume_total);
                long totalSoldInventory = sellOrders.Sum(t => (t.volume_total - t.volume_remain));
                decimal percentSoldInventory =  totalMarketInventory > 0 ? ((decimal)totalSoldInventory / totalMarketInventory) * 100 : 0;


                float lowestSellOrder = sellOrders.Any() ? sellOrders.OrderBy(p => p.price).FirstOrDefault().price : 0;
                float highestSellOrder = sellOrders.Any() ? sellOrders.OrderByDescending(p => p.price).FirstOrDefault().price : 0;

                float baseLowestSellOrder = baseSellOrders.Any() ? baseSellOrders.OrderBy(p => p.price).FirstOrDefault().price : 0;
                float baseHighestSellOrder = baseSellOrders.Any() ? baseSellOrders.OrderByDescending(p => p.price).FirstOrDefault().price : 0;

                double averageVolume = itemHistory.MarketHistory.Any() ? itemHistory.MarketHistory.Average(h => h.volume) : 0;

                ItemOrderComparison itemOrderComparison = new ItemOrderComparison
                {
                    Id = item.Key,
                    Name = item.Value,
                    BasePrice = baseLowestSellOrder,
                    Price = lowestSellOrder,
                    WeightedAveragePrice = sellOrderWeightedAveragePrice,
                    WeightedAverageBasePrice = baseSellOrderWeightedAveragePrice,
                    AverageVolume = (long)averageVolume,
                    PercentMarketInventorySold = percentSoldInventory
                };

                result.ItemOrderComparisons.Add(itemOrderComparison);
            }


            return result;

        }

        public void SaveEveMarketOrdersDataAnalysisAsCSV(EveMarketOrdersDataAnalysis eveMarketDataAnalysis, string fileName)
        {
            StringBuilder csvBuild = new StringBuilder();
            csvBuild.AppendLine($"ItemName,Price,BasePrice,Difference,ProfitMargin,AverageVolume,AverageDailyProfitPotential,PercentSoldInventory"); //header

            foreach (var itemOrderComparison in eveMarketDataAnalysis.ItemOrderComparisons)
            {
                csvBuild.AppendLine($"{ itemOrderComparison.Name }," +
                                    $"{ itemOrderComparison.Price },{ itemOrderComparison.BasePrice },{ itemOrderComparison.Difference }, { itemOrderComparison.ProfitMargin}," +
                                    $"{ itemOrderComparison.AverageVolume }, { itemOrderComparison.AverageDailyProfitPotential }, { itemOrderComparison.PercentMarketInventorySold }");
            }

            if (!Directory.Exists("exports"))
            {
                Directory.CreateDirectory("exports");
            }

            File.WriteAllText(Path.Combine("exports", fileName), csvBuild.ToString());
            Console.WriteLine($"Saved CSV as { fileName }");
        }

        private float CalculateWeightedAverage(List<MarketOrder> orders)
        {
            float numerator = 0;
            float denominator = orders.Sum(o => o.volume_remain);

            foreach (var order in orders)
            {
                numerator += (order.volume_remain * order.price);
            }

            return numerator / denominator;
        }

        private readonly EveMarketData _eveMarketData;
    }
}
