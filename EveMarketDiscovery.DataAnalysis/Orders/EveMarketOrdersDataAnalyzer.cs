using EveAccountant.Common;
using EveSSO;
using System;
using System.Collections.Generic;
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

        public void GetAnalysis(long regionId, int itemLimit)
        {
            var baseRegionId = 10000002; //Jita
            var items = ItemProvider.Items().Take(itemLimit);

            var regionOrders = _eveMarketData.RegionMarketOrders.Where(r => r.RegionId == regionId).FirstOrDefault();

            foreach (var item in items)
            {
                //Get all the orders for this item
                var orders = regionOrders.ItemMarketOrders.Where(i => i.ItemId == item.Key).FirstOrDefault().MarketOrders.ToList();
                var buyOrders = orders.Where(o => o.is_buy_order).OrderByDescending(p => p.price).ToList();
                var sellOrders = orders.Where(o => !o.is_buy_order).OrderBy(p => p.price).ToList();
            }

        }

        private EveMarketData _eveMarketData;
    }
}
