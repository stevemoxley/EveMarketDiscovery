using Common;
using EveSSO;
using System;
using System.Collections.Generic;
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
                        var history = itemMarketHistory.MarketHistory[0];
                        var baseHistory = baseRegionData.ItemMarketHistories.Where(h => h.ItemId == itemMarketHistory.ItemId).FirstOrDefault().MarketHistory[0];

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
                    catch(IndexOutOfRangeException)
                    {
                        continue;
                    }

                }
            }
            return result;
        }




        private EveMarketData _eveMarketData;
        private RegionProvider _regionProvider;

    }
}
