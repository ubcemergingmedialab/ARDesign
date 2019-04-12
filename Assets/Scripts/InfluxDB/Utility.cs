using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARDesign
{
    namespace Influx
    {
        /// <summary>
        /// Static helper functions for Influx querying and serialization
        /// </summary>
        public static class Utility
        {
            #region PUBLIC_METHODS
            /// <summary>
            /// Builds https query for use with InfluxDB https query language
            /// </summary>
            /// <param name="host">Influx host</param>
            /// <param name="port">Influx post</param>
            /// <param name="isPretty">If true, query result will be in structured json. For use with testing.</param>
            /// <param name="db">Influx database</param>
            /// <param name="query">Query in Influx query language - supports most SQL queries</param>
            /// <returns>Returns formatted URL</returns>
            public static string EncodeQuery(string host, string port, bool isPretty, string db, string query)
            {
                return "http://" + host + ":" + port + "/query?db=" + db + (isPretty ? "&pretty=true" : "") + "&q=" + System.Uri.EscapeDataString(query);
            }

            /// <summary>
            /// \deprecated Parses the values from a given json string - for use in populating widgets
            /// </summary>
            /// <param name="jsonToParse">JSON string to parse for values</param>
            /// <returns>Returns list of Values - deprecated in favor of simpler data structures</returns>
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

            /// <summary>
            /// Parses the values from a given json string - for use in populating widgets
            /// </summary>
            /// <param name="jsonToParse">JSON string to parse for values</param>
            /// <returns>Returns dictionary of time-value pairs. Vector2 structure stores time as float in x, and value in y</returns>
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

            /// <summary>
            /// Parses the values from a given json string - for use in populating widgets
            /// </summary>
            /// <param name="jsonToParse">JSON string to parse for values</param>
            /// <param name="dict">Existing dictionary to add new values to</param>
            /// <returns>Returns dictionary of time-value pairs. Vector2 structure stores time as float in x, and value in y</returns>
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

            /// <summary>
            /// Queries database for type labels (ie CO2, temperature, etc)
            /// </summary>
            /// <param name="jsonToParse">JSON string to parse - should be returned from query for type labels</param>
            /// <returns>String array containing all labels</returns>
            public static string[] ParseLabels(string jsonToParse)
            {
                JToken results = JToken.Parse(jsonToParse);
                IList<JToken> vals = results["results"][0]["series"][0]["values"].Children().ToList();
                return vals.Select(val => val[1].ToString()).ToArray<string>();
            }
            #endregion //PUBLIC METHODS
        }

        /// <summary>
        /// \deprecated Struct for storing deserialized values from widgets
        /// </summary>
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

