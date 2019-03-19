using System;
using System.Collections.Generic;
using System.Text;

namespace EveAccountant.Common
{
    public class RegionProvider
    {
        public RegionProvider()
        {
            RegionNames = new Dictionary<long, string>();
            RegionNames.Add(10000002, "Jita");
            RegionNames.Add(10000043, "Amarr");
            RegionNames.Add(10000030, "Heimatar");
            RegionNames.Add(10000042, "Metropolis");
        }

        public Dictionary<long, string> RegionNames { get; set; }
    }
}
