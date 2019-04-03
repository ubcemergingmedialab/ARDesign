using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataWidget : InfluxType<IDictionary<DateTime, Vector2>> {

    private DataWidgetHandler wid;
    private Vector2 curVal;

    //Data source to graph

    private string label;

    /* Uncomment for graphing
    private List<Vector2> _graphableData = null;
    public List<Vector2> GraphableData
    {
        get
        {
            if(_graphableData == null)
            {
                _graphableData = DataToGraph();
            }
            return _graphableData;
        }
    }
    */
    protected override void CastWidget()
    {
        wid = (DataWidgetHandler)widget;
    }

    protected override void ParseSetUpText(string webReturn)
    {
        dataVals = JSONHelper.ParseValuesNoType(webReturn);
        
        isDataBuilt = true;
        SetCurrentValues();
        //StartCoroutine(wid.BuildGraph());
    }


    protected override void ParseUpdateText(string webResult)
    {
        dataVals = JSONHelper.ParseValuesNoType(webResult, dataVals);
        SetCurrentValues();
    }

    protected override string SetQueryUrl()
    {
        return BuildUrlWithoutLimitSetType(GetLabel());
    }

    protected override string SetUpdateUrl()
    {
        return BuildUrlWithLimitSetType(GetLabel(), 1);
    }

    public string GetLabel()
    {
        return label;
    }

    public void SetCurrentValues()
    {
        if (!isDataBuilt)
        {
            throw new Exception("Error!data not build");
        }
        else
        {
            curVal = dataVals[dataVals.Keys.Max()];
        }

    }

    public Vector2 GetCurrentValue()
    {
        return curVal;
    }

    public void SetLabel(string toSet)
    {
        label = toSet;
        wid.UpdateLabel();
    }
    /* Graphing functionality
    public List<Vector2> DataToGraph()
    {
        return dataVals.Where(p => (p.Key.Day == dataVals.Keys.Max().Day)).Select(p => p.Value).ToList();
    }
    */
}
