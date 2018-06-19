using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO.Market.History
{ 
    public class MarketHistory
    {
        public float average { get; set; }
        public string date { get; set; }
        public float highest { get; set; }
        public float lowest { get; set; }
        public long order_count { get; set; }
        public long volume { get; set; }
    }
}
