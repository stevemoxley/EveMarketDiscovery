using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO.Market.Order
{
    public class MarketOrder
    {
        public long duration { get; set; }
        public bool is_buy_order { get; set; }
        public DateTime issued { get; set; }
        public long location_id { get; set; }
        public long min_volume { get; set; }
        public long order_id { get; set; }
        public float price { get; set; }
        public string range { get; set; }
        public long system_id { get; set; }
        public long type_id { get; set; }
        public long volume_remain { get; set; }
        public long volume_total { get; set; }
    }

}
