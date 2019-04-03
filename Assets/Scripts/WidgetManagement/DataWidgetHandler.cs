using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataWidgetHandler : WidgetType<DataWidget> {

    [SerializeField]
    private TextMesh label;
    [SerializeField]
    private Text labelText;
    [SerializeField]
    private Text curval;
    [SerializeField]
    private Text curtime;

    //TODO: get rid of this kinda asap
    public SpriteRenderer graph;

    /* Uncomment for graphing
    [SerializeField]
    private WMG_Axis_Graph graph;

    private List<Vector2> toGraph;
    private WMG_Data_Source ds;
    */


    private void Start()
    {
        //Uncomment for graphing ds = GetComponent<WMG_Data_Source>();
    }

    private void Update()
    {
        curval.text = reader.GetCurrentValue().y.ToString();
        DateTime time = new DateTime((long)reader.GetCurrentValue().x);
        curtime.text = time.ToString();
    }

    public void UpdateLabel()
    {
        //Debug.Log(reader.name);
        label.text = reader.GetLabel();
        labelText.text = reader.GetLabel();
    }
    /* Uncomment for graphing
    public IEnumerator BuildGraph()
    {
        while (!reader.isDataBuilt)
            yield return null;
        graph.Init();
        ds.setDataProvider(reader);
        ds.variableType = WMG_Data_Source.WMG_VariableTypes.Property;
        ds.setVariableName("GraphableData");

        while (!isEnable)
            yield return null;
        
        WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
        s1.name = reader.GetLabel();
        s1.pointValuesDataSource = ds; // assign the data source to a series.
    }
    */
}
