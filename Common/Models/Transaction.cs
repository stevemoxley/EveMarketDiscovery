using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Transaction
    {
        public long TransactionId { get; set; }

        public float Price { get; set; }

        public long Quantity { get; set; }

        public DateTime DateTime { get; set; }

        public long TypeId { get; set; }

        public long LocationId { get; set; }

    }
}
