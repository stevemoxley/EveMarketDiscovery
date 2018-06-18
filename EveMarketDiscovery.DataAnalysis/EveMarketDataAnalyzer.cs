using Common;
using EveSSO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery.DataAnalysis
{
    public class EveMarketDataAnalyzer
    {
        public EveMarketDataAnalyzer(EveMarketData eveMarketData)
        {
            _eveMarketData = eveMarketData;
            _regionProvider = new RegionProvider();
        }

        public EveMarketDataAnalysis GetAnalysis(int itemLimit)
        {
            var data = _eveMarketData.RegionMarketHistories;
            var baseRegionId = 10000002; //Jita
            var baseRegionData = data.Where(h => h.RegionId == baseRegionId).FirstOrDefault();
            data.Remove(baseRegionData);

            EveMarketDataAnalysis result = new EveMarketDataAnalysis();

            foreach (var regionDataHistory in data)
            {
                var itemMarketHistories = regionDataHistory.ItemMarketHistories;

                var regionComparison = new RegionComparison
                {
                    BaseRegionId = baseRegionId,
                    RegionId = regionDataHistory.RegionId
                };

                result.RegionComparisons.Add(regionComparison);

                if (itemLimit > -1)
                {
                    itemMarketHistories = itemMarketHistories.Take(itemLimit).ToList();
                }

                foreach (var itemMarketHistory in itemMarketHistories)
                {
                    try
                    {
                        var itemComparison = new ItemComparison();
                        var history = itemMarketHistory.MarketHistory[itemMarketHistory.MarketHistory.Length - 1];
                        var baseHistories = baseRegionData.ItemMarketHistories.Where(h => h.ItemId == itemMarketHistory.ItemId).FirstOrDefault();
                        var baseHistory = baseHistories.MarketHistory[baseHistories.MarketHistory.Length - 1];

                        itemComparison.BaseRegionId = baseRegionId;
                        itemComparison.ItemId = itemMarketHistory.ItemId;
                        itemComparison.RegionId = regionComparison.RegionId;

                        itemComparison.AveragePrice = history.average;
                        itemComparison.BaseAveragePrice = baseHistory.average;
                        itemComparison.ItemName = ItemProvider.Items()[itemComparison.ItemId];
                        itemComparison.Volume = history.volume;
                        itemComparison.BaseVolume = baseHistory.volume;

                        string regionName = _regionProvider.RegionNames[itemComparison.RegionId];

                        regionComparison.ItemComparisons.Add(itemComparison);
                    }
                    catch (NullReferenceException)
                    {
                        continue;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }

                }
            }
            return result;
        }

        public void SaveEveMarketDataAnalysisAsCSV(EveMarketDataAnalysis eveMarketDataAnalysis, string fileName)
        {
            StringBuilder csvBuild = new StringBuilder();
            csvBuild.AppendLine($"ItemName,Region,BaseRegion,AveragePrice,BaseAveragePrice,Difference,ProfitMargin,Volume,BaseVolume,AverageVolumeProfitMarginPotential,PotentialDailyProfit"); //header

            foreach (var regionComparison in eveMarketDataAnalysis.RegionComparisons)
            {
                string regionName = new RegionProvider().RegionNames[regionComparison.RegionId];

                foreach (var itemComparison in regionComparison.ItemComparisons)
                {
                    csvBuild.AppendLine($"{ itemComparison.ItemName },{ regionName },Jita," +
                                        $"{ itemComparison.AveragePrice },{ itemComparison.BaseAveragePrice },{ itemComparison.PriceDifference }," +
                                        $"{ itemComparison.ProfitMargin },{itemComparison.Volume},{ itemComparison.BaseVolume}," +
                                        $"{ itemComparison.AverageVolumeProfitMarginPotential },{ itemComparison.PotentialDailyProfit }");
                }
            }

            if (!Directory.Exists("exports"))
            {
                Directory.CreateDirectory("exports");
            }

            File.WriteAllText(Path.Combine("exports", fileName), csvBuild.ToString());
            Console.WriteLine($"Saved CSV as { fileName }");
        }


        private EveMarketData _eveMarketData;
        private RegionProvider _regionProvider;

    }
}
