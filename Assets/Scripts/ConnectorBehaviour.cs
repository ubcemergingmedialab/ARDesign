using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws a line from the widget to the rootnode, updating as widget position changes
/// </summary>
public class ConnectorBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject widget;

    [SerializeField]
    private GameObject rootNode;

    #region PRIVATE_MEMBER_VARIABLES
    private Vector3 root;
    private Vector3 widgetPos;

    private LineRenderer line;
    #endregion //PRIVATE MEMBER VARIABLES

    #region UNITY_MONOBEHAVIOUS_METHODS
    // Use this for initialization
    void Start () {
        root = rootNode.transform.position;
        widgetPos = widget.transform.position;
        line = gameObject.GetComponent<LineRenderer>();

        line.SetPosition(0, root);
        line.SetPosition(1, widgetPos);
    }
    

    // Update is called once per frame
    void Update () {
        root = rootNode.transform.position;
        widgetPos = widget.transform.position;
        line.SetPosition(0, root);
        line.SetPosition(1, widgetPos);
    }
    #endregion //UNITY MONOBEHAVIOR METHODS
}
