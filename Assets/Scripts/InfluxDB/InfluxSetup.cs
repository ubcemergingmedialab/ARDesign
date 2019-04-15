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
            [SerializeField]
            public string Building
            {
                get
                {
                    return scene.Building;
                }

                set
                {
                    scene.Building = value;
                }
            }
            [SerializeField]
            public string Room
            {
                get
                {
                    return scene.Room;
                }

                set
                {
                    scene.Room = value;
                }
            }
            #region PRIVATE_MEMBER_VARIABLES
            private DBScene scene;
            [SerializeField]
            private string Host
            {
                get
                {
                    return scene.Host;
                }

                set
                {
                    scene.Host = value;
                }
            }
            [SerializeField]
            private string Port
            {
                get
                {
                    return scene.Port;
                }

                set
                {
                    scene.Port = value;
                }
            }
            [SerializeField]
            private string DB
            {
                get
                {
                    return scene.Db;
                }

                set
                {
                    scene.Db = value;
                }
            }
            #endregion //PRIVATE MEMBER VARIABLES


            #region PUBLIC_METHODS
            /// <summary>
            /// Given a deserialized scene configuration, set all Influx server values from config
            /// </summary>
            /// <param name="sceneToBuild">DBScene object - should be deserializated from setup</param>
            public void Setup(DBScene sceneToBuild)
            {
                scene = sceneToBuild;
            }

            /// <summary>
            /// Manually sets Influx server values
            /// </summary>
            /// <param name="h">Host address to set</param>
            /// <param name="p">Port address to set</param>
            /// <param name="d">Database name to set</param>
            /// <param name="b">Building name to set</param>
            /// <param name="r">Room name to set</param>
            public void Setup(string h, string p, string d, string b, string r)
            {
                scene.Host = h;
                scene.Port = p;
                scene.Db = d;
                scene.Building = b;
                scene.Room = r;
            }

            /// <summary>
            /// Encodes a given plain text query into a InfluxDB https query
            /// </summary>
            /// <param name="query">Influx query language string to query</param>
            /// <returns>URL to call https query on set Influx server</returns>
            public string BuildUrlWithQuery(string query)
            {
                return Utility.EncodeQuery(Host, Port, false, DB, query);
            }
            #endregion //PUBLIC METHODS
        }
    }
}

