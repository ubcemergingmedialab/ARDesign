using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARDesign.Serialize
{
    public struct DBWidget
    {

        public Vector3 Position;
        public string Measure;

        // Possibly move these to DBScene?
        public string Building;
        public string Room;
    }
}

