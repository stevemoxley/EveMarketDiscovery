using Common;
using EveMarketDiscovery.DataAnalysis;
using EveSSO;
using EveSSO.Market.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMarketDiscovery
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Eve Market Discovery");
            Console.WriteLine("-------------------------");

            //Console.WriteLine("Press [W] to Download Data from Web, or press anything else to continue...");

            //if (Console.ReadLine().ToLower() == "w")
            //{
            //    EveSSO.MarketHistoryProvider.GetMarketHistoryFromWeb(3000);
            //}

            //Console.WriteLine("Press [O] To Download Order Data From Web, or press anything else to continue...");

            //if(Console.ReadKey().KeyChar == 'o')
            //{
            //    var orders = MarketOrderProvider.GetItemMarketOrders(10000002, 34, "sell", false);
            //}

            //Console.WriteLine("Enter a region id:");
            //var regionId = long.Parse(Console.ReadLine());
            //long jitaRegionId = 10000002;
            //long amarrRegionId = 10000043;
            //long heimatarRegionId - rens = 10000030;
            //long metropolisRegionId - hek = 10000042;

            Console.WriteLine("Getting cached data. This can take awhile...");

            int itemLimit = 1000;
            var eveMarketData = new EveMarketData(itemLimit);
            var dataAnalyzer = new EveMarketDataAnalyzer(eveMarketData);
            Console.WriteLine("Cache loaded..");

            Console.WriteLine("Getting Analysis");
            var analysis = dataAnalyzer.GetAnalysis(itemLimit);

            Console.WriteLine("Getting comparisons");

            var topPriceDifferences = analysis.GetTopItemComparisons(20, minVolume: 10);

            var regionProvider = new RegionProvider();

            //Console.WriteLine("Showing Potential Profit Margin");
            //foreach (var item in topPriceDifferences)
            //{
            //    var regionName = regionProvider.RegionNames[item.RegionId];
            //    //Console.WriteLine($"Jita - { regionName } : { item.ItemName }  { item.ProfitMargin }% ||| { item.BaseVolume  } ||| { item.Volume }  ");
            //    Console.WriteLine($"Jita - {regionName} { item.ItemName } ||||  PDP: { item.PotentialDailyProfit } ||||  PM:{ Math.Round(item.ProfitMargin, 3) }  ");
            //}

            Console.WriteLine("Save as csv? y/n");

            if(Console.ReadLine().ToLower() == "y")
            {
                Console.WriteLine("Enter a file name: ");
                var fileName = Console.ReadLine();
                dataAnalyzer.SaveEveMarketDataAnalysisAsCSV(analysis, $"{fileName}.csv");
            }

            Console.ReadLine();


        }
    }
}
