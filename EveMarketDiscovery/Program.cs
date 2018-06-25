using Common;
using DataAccess;
using EveMarketDiscovery.DataAnalysis;
using EveMarketDiscovery.DataAnalysis.History;
using EveMarketDiscovery.DataAnalysis.Orders;
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

            try
            {

                using (var db = new DataContext())
                {
                    var transactions = db.Transactions.ToList();

                    db.Transactions.Add(new Common.Models.Transaction
                    {
                        DateTime = DateTime.Now,
                        Price = 100,
                        Quantity = 10,
                        TypeId = 1,
                        LocationId = 100
                    });
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //long jitaRegionId = 10000002;
            //long amarrRegionId = 10000043;
            //long heimatarRegionId - rens = 10000030;
            //long metropolisRegionId - hek = 10000042;

            Console.WriteLine("Getting cached data. This can take awhile...");

            int itemLimit = 2000;
            var eveMarketData = new EveMarketData(itemLimit);
            Console.WriteLine("Cache loaded..");
            Console.WriteLine("Press [H] For History Analysis. Press [O] For Order Analysis");

            var @char = Console.ReadKey().KeyChar;

            if (@char == 'h')
            {
                GetHistoryAnalysis(eveMarketData, itemLimit);
            }
            else if (@char == 'o')
            {
                GetOrderAnalysis(eveMarketData, itemLimit);
            }

            Console.ReadLine();
        }


        static void GetHistoryAnalysis(EveMarketData data, int itemLimit)
        {
            var historyDataAnalyzer = new EveMarketDataHistoryAnalyzer(data);

            Console.WriteLine("Getting Market History Analysis...");
            var analysis = historyDataAnalyzer.GetAnalysis(itemLimit);

            Console.WriteLine("Done. Save as csv? y/n");

            if (Console.ReadKey().KeyChar == 'y')
            {
                Console.WriteLine("Enter a file name: ");
                var fileName = Console.ReadLine();
                historyDataAnalyzer.SaveEveMarketDataAnalysisAsCSV(analysis, $"{fileName}.csv");
            }
        }

        static void GetOrderAnalysis(EveMarketData data, int itemLimit)
        {

            var orderDataAnalyzer = new EveMarketOrdersDataAnalyzer(data);

            long hek = 10000042;

            orderDataAnalyzer.GetAnalysis(hek, itemLimit);
        }
    }
}
