using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveAccountant.Common
{
    public class Item
    {
        public Description description { get; set; }

        public int graphicID { get; set; }

        public int groupID { get; set; }

        public string mass { get; set; }

        public Name name { get; set; }

        public int portionSize { get; set; }

        public bool published { get; set; }

        public decimal radius { get; set; }

        public long soundID { get; set; }

        public decimal volume { get; set; }

        public int iconID { get; set; }

        public int raceID { get; set; }

        public string sofFactionName { get; set; }

        public decimal basePrice { get; set; }

        public int marketGroupID { get; set; } = -1;

        public decimal capacity { get; set; }

        public int factionID { get; set; }

        public override string ToString()
        {
            return name.ToString();
        }
    }


    public class Description
    {
        public string de { get; set; }

        public string en { get; set; }

        public string fr { get; set; }

        public string ru { get; set; }

        public string zh { get; set; }

        public string ja { get; set; }

        public override string ToString()
        {
            return en;
        }
    }

    public class Name
    {
        public string de { get; set; }

        public string en { get; set; }

        public string fr { get; set; }

        public string ru { get; set; }

        public string zh { get; set; }

        public string ja { get; set; }

        public override string ToString()
        {
            return en;
        }
    }
}
