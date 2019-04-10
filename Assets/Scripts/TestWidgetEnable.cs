using ARDesign.Widgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWidgetEnable : MonoBehaviour {
    private bool isEnable = false;
    private WidgetHandler parent;

	// Use this for initialization
	void Start () {
        parent = this.GetComponentInParent<WidgetHandler>();
	}
	
	void OnMouseDown () {
        parent.EnableWidget(!isEnable);
        isEnable = !isEnable;
	}
}
