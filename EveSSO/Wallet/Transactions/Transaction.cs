using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO.Wallet.Transactions
{

    public class Transaction
    {
        public long client_id { get; set; }
        public DateTime date { get; set; }
        public bool is_buy { get; set; }
        public bool is_personal { get; set; }
        public long journal_ref_id { get; set; }
        public long location_id { get; set; }
        public long quantity { get; set; }
        public long transaction_id { get; set; }
        public long type_id { get; set; }
        public float unit_price { get; set; }
    }

}
