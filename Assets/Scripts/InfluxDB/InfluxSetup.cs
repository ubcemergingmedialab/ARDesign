using ARDesign.Serialize;
using UnityEngine;

namespace ARDesign
{
    namespace Influx
    {
        /// <summary>
        /// Object for storing scene configuration settings - should be parent to widgets
        /// </summary>
        public class InfluxSetup : MonoBehaviour
        {
            #region PRIVATE_MEMBER_VARIABLES
            [SerializeField]
            private string host;
            [SerializeField]
            private string port = "8086";
            [SerializeField]
            private string db;
            #endregion //PRIVATE MEMBER VARIABLES

            #region PUBLIC_METHODS
            /// <summary>
            /// Given a deserialized scene configuration, set all Influx server values from config
            /// </summary>
            /// <param name="sceneToBuild">DBScene object - should be deserializated from setup</param>
            public void Setup(DBScene sceneToBuild)
            {
                host = sceneToBuild.Host;
                port = sceneToBuild.Port;
                db = sceneToBuild.Db;
            }
         
            /// <summary>
            /// Manually sets Influx server values
            /// </summary>
            /// <param name="h">Host address to set</param>
            /// <param name="p">Port address to set</param>
            /// <param name="d">Database name to set</param>
            public void Setup(string h, string p, string d)
            {
                host = h;
                port = p;
                db = d;

            }

            /// <summary>
            /// Encodes a given plain text query into a InfluxDB https query
            /// </summary>
            /// <param name="query">Influx query language string to query</param>
            /// <returns>URL to call https query on set Influx server</returns>
            public string BuildUrlWithQuery(string query)
            {
                return Utility.EncodeQuery(host, port, false, db, query);
            }
            #endregion //PUBLIC METHODS
        }
    }
}

