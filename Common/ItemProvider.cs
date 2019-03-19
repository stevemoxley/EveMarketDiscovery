using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace Common
{
    public static class ItemProvider
    {

        public static Dictionary<long, string> Items()
        {
            Dictionary<long, string> result = new Dictionary<long, string>();

            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string location = Path.Combine(executableLocation, "items.txt");

            var text = File.ReadAllLines(System.Web.Hosting.HostingEnvironment.MapPath("~/")

            for (int i = 0; i < text.Length; i++)
            {
                var line = text[i];
                var parts = line.Split('|');
                var id = long.Parse(parts[0]);
                var name = parts[1].ToString();
                result.Add(id, name);
            }


            return result;
        }

    }
}
