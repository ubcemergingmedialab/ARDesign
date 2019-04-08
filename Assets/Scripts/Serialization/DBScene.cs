using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARDesign.Serialize {

    /// <summary>
    /// Struct for storing configuration of a room - designed for deserializing from JSON
    /// </summary>
    public struct DBScene
    {
        public string Host;
        public string Port;
        public string Db;
        // List of widgets in the room
        public IList<DBWidget> Widgets;
    }
}


