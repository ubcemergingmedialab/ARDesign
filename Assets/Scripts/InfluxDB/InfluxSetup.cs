using ARDesign.Serialize;
using UnityEngine;

namespace ARDesign
{
    namespace Influx
    {
        public class InfluxSetup : MonoBehaviour
        {

            [SerializeField]
            private string host;
            [SerializeField]
            private int port = 8086;
            [SerializeField]
            private string db;

            // Sets up scene with SBScene object, for use with JSON deserialization
            public void Setup(DBScene sceneToBuild)
            {
                host = sceneToBuild.Host;
                port = int.Parse(sceneToBuild.Port);
                db = sceneToBuild.Db;
            }

            // Optionally lets you manually set DB settings - kinda redundant, but nice to have i guess
            public void Setup(string h, string p, string d)
            {
                host = h;
                port = int.Parse(p);
                db = d;

            }

            public string BuildUrlWithQuery(string query)
            {
                return Utility.EncodeQuery(host, port.ToString(), false, db, query);
            }
        }
    }
}

