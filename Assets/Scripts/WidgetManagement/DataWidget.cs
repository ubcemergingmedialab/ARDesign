using ARDesign.Influx;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARDesign.Widgets { 
    public class DataWidget : InfluxType<IDictionary<DateTime, Vector2>> {

        private DataWidgetHandler wid;
        private Vector2 curVal;

        private string label;

        protected override void CastWidget()
        {
            wid = (DataWidgetHandler)widget;
        }

        protected override void ParseSetUpText(string webReturn)
        {
            dataVals = Utility.ParseValuesNoType(webReturn);
        
            isDataBuilt = true;
            SetCurrentValues();
            //StartCoroutine(wid.BuildGraph());
        }


        protected override void ParseUpdateText(string webResult)
        {
            dataVals = Utility.ParseValuesNoType(webResult, dataVals);
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
    }
}
