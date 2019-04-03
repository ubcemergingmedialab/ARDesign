using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject widget;

    [SerializeField]
    private GameObject rootNode;

    private Vector3 root;
    private Vector3 widgetPos;

    private LineRenderer line;

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
}
