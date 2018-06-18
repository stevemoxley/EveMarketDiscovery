using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public static class ItemProvider
    {

        public static Dictionary<long, string> Items()
        {
            Dictionary<long, string> result = new Dictionary<long, string>();


            var text = File.ReadAllLines("items.txt");

            foreach (var line in text)
            {
                var parts = line.Split('|');
                var id = long.Parse(parts[0]);
                var name = parts[1].ToString();
                result.Add(id, name);
            }


            return result;
        }

    }
}
