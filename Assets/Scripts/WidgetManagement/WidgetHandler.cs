using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Any widget functionality that depends on data types should implement from this
public abstract class WidgetType<T> : WidgetHandler where T:InfluxReader {

    protected T reader;

    void Awake()
    {
        reader = this.GetComponent<T>();
        influxWidget = reader;
    }

}


// General widget functions go here
public abstract class WidgetHandler : MonoBehaviour {

    protected bool isEnable = false;

    [SerializeField]
    protected GameObject WidgetObj;
    [SerializeField]
    protected GameObject Connector;

    protected InfluxReader influxWidget;
    
	
	// Update is called once per frame
	void Update () {
		
	}

    // If false, hides everything but the base node. Otherwise, whole widget is visible
    public virtual void EnableWidget(bool enable)
    {
        Connector.SetActive(enable);
        WidgetObj.SetActive(enable);
        isEnable = !isEnable;
    }

    public void SetWidgetObjDistance(float dist)
    {
        Vector3 oldPos = WidgetObj.transform.localPosition;
        WidgetObj.transform.localPosition = new Vector3(oldPos.x, oldPos.y, dist);
    }

}
