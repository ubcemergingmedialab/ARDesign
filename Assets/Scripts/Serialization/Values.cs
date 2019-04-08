using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARDesign.Serialize
{
    public struct Values
    {

        public System.DateTime t;
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

