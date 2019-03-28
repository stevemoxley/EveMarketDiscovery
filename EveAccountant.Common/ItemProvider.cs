using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using YamlDotNet.Serialization;
using System.Linq;

namespace EveAccountant.Common
{
    public static class ItemProvider
    {

        public static Dictionary<long, string> Items()
        {
            if (_items.Count > 0)
                return _items;

            string text = null;

            try
            {
                text = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(@"typeIDs.yaml"));
            }
            catch (NullReferenceException ex)
            {
                text = File.ReadAllText("typeIDs.yaml");
            }


            var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

            var items = deserializer.Deserialize<Dictionary<long, Item>>(text);

            foreach (var item in items)
            {
                if (item.Value.marketGroupID > 0)
                {
                    _items.Add(item.Key, item.Value.ToString());
                }
            }

            return _items;
        }

        public static bool TryGetItem(long key, out string result)
        {
            result = "";
            try
            {
                result = Items()[key];
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        private static Dictionary<long, string> _items = new Dictionary<long, string>();
    }
}
