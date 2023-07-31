using Newtonsoft.Json;
using Noisrev.League.IO.RST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translate_app
{
    internal class DataFormatter
    {
        static public string ToJSON(RSTFile data)
        {
            var formattedData = new Dictionary<string, string>();

            foreach (var entry in data.Entries)
            {
                formattedData.Add(entry.Key.ToString(), entry.Value);
            }

            string json = JsonConvert.SerializeObject(formattedData);
            return json;
        }
    }
}
