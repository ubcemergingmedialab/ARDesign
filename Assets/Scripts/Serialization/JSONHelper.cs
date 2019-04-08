using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARDesign.Serialize.Utility
{
    // Helper utilities for JSON parsing
    public static class JSONHelper
    {

        public static string EncodeQuery(string host, string port, bool isPretty, string db, string query)
        {
            return "http://" + host + ":" + port + "/query?db=" + db + (isPretty ? "&pretty=true" : "") + "&q=" + Uri.EscapeDataString(query);
        }

        // Given a json string, deserializes it into a scene configuration
        public static DBScene CreateFromJSON(string json)
        {
            return JsonConvert.DeserializeObject<DBScene>(json);
        }

        // Parses the values from a given json string - for use in populating widgets
        public static IList<Values> ParseValues(string jsonToParse)
        {

            JToken results = JToken.Parse(jsonToParse);
            IList<JToken> vals = results["results"][0]["series"][0]["values"].Children().ToList();
            IList<Values> toReturn = new List<Values>();
            foreach (JToken val in vals)
            {
                Values result = new Values
                {
                    t = DateTime.Parse(val[0].ToString()),
                    type = val[1].ToString()
                };
                result.time = result.t.Ticks;
                double.TryParse(val[2].ToString(), out result.val);
                toReturn.Add(result);
            }

            return toReturn;

        }
        // Parses the values from a given json string - for use in populating widgets
        public static IDictionary<DateTime, Vector2> ParseValuesNoType(string jsonToParse)
        {

            JToken results = JToken.Parse(jsonToParse);
            IList<JToken> vals = results["results"][0]["series"][0]["values"].Children().ToList();
            IDictionary<DateTime, Vector2> toReturn = new Dictionary<DateTime, Vector2>();
            foreach (JToken val in vals)
            {
                DateTime key = DateTime.Parse(val[0].ToString());
                float t = key.Ticks;
                float value;
                float.TryParse(val[1].ToString(), out value);
                toReturn[key] = new Vector2(t, value);
            }

            return toReturn;

        }

        // Parses the values from a given json string - for use in populating widgets
        public static IDictionary<DateTime, Vector2> ParseValuesNoType(string jsonToParse, IDictionary<DateTime, Vector2> dict)
        {
            JToken results = JToken.Parse(jsonToParse);
            IList<JToken> vals = results["results"][0]["series"][0]["values"].Children().ToList();
            foreach (JToken val in vals)
            {
                DateTime key = DateTime.Parse(val[0].ToString());
                float t = key.Ticks;
                float value;
                float.TryParse(val[1].ToString(), out value);
                dict[key] = new Vector2(t, value);
            }

            return dict;

        }


        public static string[] ParseLabels(string jsonToParse)
        {
            JToken results = JToken.Parse(jsonToParse);
            IList<JToken> vals = results["results"][0]["series"][0]["values"].Children().ToList();
            return vals.Select(val => val[1].ToString()).ToArray<string>();
        }
    }
}
