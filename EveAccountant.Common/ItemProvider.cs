using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using YamlDotNet.Serialization;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace EveAccountant.Common
{
    public static class ItemProvider
    {

        public static Dictionary<long, string> Items()
        {
            if (_items.Count > 0)
                return _items;

            if (_ignoreList == null)
                LoadIgnoreList();

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
                if (item.Value.marketGroupID > 0 && !_ignoreList.Contains(item.Key))
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

        public static void AddToIgnoreList(long itemId)
        {
            if (_ignoreList == null)
                LoadIgnoreList();

            _ignoreList.Add(itemId);
            SaveIgnoreList();
        }

        private static void LoadIgnoreList()
        {
            string ignoreList = File.ReadAllText("ignoreList.txt");
            _ignoreList = JsonConvert.DeserializeObject<ConcurrentBag<long>>(ignoreList) ?? new ConcurrentBag<long>();
        }

        private static void SaveIgnoreList()
        {
            string ignoreListJson = JsonConvert.SerializeObject(_ignoreList);
            File.WriteAllText("ignoreList.txt", ignoreListJson);
        }


        private static Dictionary<long, string> _items = new Dictionary<long, string>();
        private static ConcurrentBag<long> _ignoreList;
    }
}
