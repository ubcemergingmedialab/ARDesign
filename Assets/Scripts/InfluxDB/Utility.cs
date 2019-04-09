using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARDesign
{
    namespace Influx
    {
        public static class Utility
        {
            public static string EncodeQuery(string host, string port, bool isPretty, string db, string query)
            {
                return "http://" + host + ":" + port + "/query?db=" + db + (isPretty ? "&pretty=true" : "") + "&q=" + System.Uri.EscapeDataString(query);
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

        public struct Values
        {

            public DateTime t;
            public long time;
            public string type;
            public double val;
            public bool isSet;

            override public string ToString()
            {
                return "DateTime: " + t.ToString() + ", type: " + type + ", val: " + val + ", isSet: " + isSet;
            }
        }
    }
}

