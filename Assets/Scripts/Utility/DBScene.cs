using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DBScene {

    public string Host;
    public string Port;
    public string Db;
    // List of widgets in the room
    public IList<DBWidget> Widgets;
}
