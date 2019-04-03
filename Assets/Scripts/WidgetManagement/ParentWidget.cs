using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentWidget : InfluxType<string[]>
{
    private ParentWidgetHandler wid;

    private IList<DataWidget> children;

    protected override void CastWidget()
    {
        wid = (ParentWidgetHandler)widget;
    }

    protected override void ParseSetUpText(string webReturn)
    {
        dataVals = JSONHelper.ParseLabels(webReturn);
        //Debug.Log(webReturn);
        wid.BuildChildren(dataVals);
        
        //Code for building children should go here
        JSONBuilder.instance.AddWidget(children);
        
    }

    private void SetupChildWidget(DataWidget child)
    {
        child.SetDBVals(measure, building, room);
    }

    protected override void ParseUpdateText(string webResult)
    {
        throw new System.NotImplementedException();
    }

    protected override string SetQueryUrl()
    {
        //Debug.Log(BuildUrlTagList("type"));
        return BuildUrlTagList("type");
    }

    public void AddChild(DataWidget dw)
    {
        if(children == null)
        {
            children = new List<DataWidget>();
        }

        children.Add(dw);
        SetupChildWidget(dw);
    }

    protected override string SetUpdateUrl()
    {
        return BuildUrlTagList("type");
    }
}
