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

        public void GetAnalysis(int itemLimit)
        {
            var baseRegionId = 10000002; //Jita
        }

        private EveMarketData _eveMarketData;
    }
}
