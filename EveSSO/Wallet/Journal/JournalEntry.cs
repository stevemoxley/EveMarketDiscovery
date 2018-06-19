using System;
using System.Collections.Generic;
using System.Text;

namespace EveSSO.Wallet.Journal
{
    public class JournalEntry
    {
        public float amount { get; set; }
        public float balance { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public long first_party_id { get; set; }
        public long id { get; set; }
        public string ref_type { get; set; }
        public long second_party_id { get; set; }
    }

}
