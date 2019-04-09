using ARDesign.Influx;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARDesign
{
    namespace Widgets
    {

        /// <summary>
        /// Widget storing data for a specific type
        /// dataVal is of type IDictionary<DateTime, Vector2>
        /// </summary>
        public class DataWidget : InfluxType<IDictionary<DateTime, Vector2>>
        {
            #region PRIVATE_MEMBER_VARIABLES
            private DataWidgetHandler wid;
            private Vector2 curVal;
            private string label;
            #endregion //PRIVATE_MEMBER_VARIABLES

            #region PUBLIC_METHODS

            /// <summary>
            /// Returns the type of the widget
            /// </summary>
            /// <returns>Widget label</returns>
            public string GetLabel()
            {
                return label;
            }

            /// <summary>
            /// Sets the widget current value - the newest dictionary entry
            /// </summary>
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

            /// <summary>
            /// Returns the widget current value
            /// </summary>
            /// <returns>Widget newest value </returns>
            public Vector2 GetCurrentValue()
            {
                return curVal;
            }

            /// <summary>
            /// Manually sets the widget label
            /// </summary>
            /// <param name="toSet">String label of type</param>
            public void SetLabel(string toSet)
            {
                label = toSet;
                wid.UpdateLabel();
            }
            #endregion //PUBLIC_METHODS

            #region PRIVATE_METHODS
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
            #endregion //PRIVATE_METHODS

        }
    }
}
