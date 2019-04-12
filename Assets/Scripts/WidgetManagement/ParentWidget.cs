using ARDesign.Influx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARDesign
{
    namespace Widgets
    {

        /// <summary>
        /// Reader class for parent widgets
        /// Parent widgets query a measurement for a list of types (ie. CO2, temp, etc), and then build a child data widget for each
        /// dataVals Data type is therefore string[]
        /// </summary>
        public class ParentWidget : InfluxType<string[]>
        {
            #region PRIVATE_MEMBER_VARIABLES

            /// <summary>
            /// Handler associated with parent widgets - handles animation triggers, etc
            /// </summary>
            private ParentWidgetHandler wid;

            /// <summary>
            /// List of all children - built dynamically
            /// </summary>
            private IList<DataWidget> children;
            #endregion //PRIVATE MEMBER VARIABLES

            #region PUBLIC_METHODS

            /// <summary>
            /// Adds a child widget to the parent
            /// </summary>
            /// <param name="dw">Widget to add</param>
            public void AddChild(DataWidget dw)
            {
                if (children == null)
                {
                    children = new List<DataWidget>();
                }

                children.Add(dw);
                SetupChildWidget(dw);
            }
            #endregion //PUBLIC_METHODS

            #region PRIVATE_METHODS
            protected override void CastWidget()
            {
                wid = (ParentWidgetHandler)widget;
            }


            /// <summary>
            /// Pareses the setup query and builds the widget and all its children
            /// </summary>
            /// <param name="webReturn">JSON string returned from query</param>
            protected override void ParseSetUpText(string webReturn)
            {
                dataVals = Utility.ParseLabels(webReturn);
                //Debug.Log(webReturn);
                wid.BuildChildren(dataVals);

                //Code for building children should go here
                JSONBuilder.instance.AddWidget(children);
            }

            /// <summary>
            /// Sets given child data widget to use the same Influx server info as its parent
            /// </summary>
            /// <param name="child">DataWidget child</param>
            private void SetupChildWidget(DataWidget child)
            {
                child.SetDBVals(measure,parent);
            }

            /// <summary>
            /// Parent widgets should never be updated - toUpdate is always false
            /// </summary>
            /// <param name="webResult"></param>
            protected override void ParseUpdateText(string webResult)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Queries a list of types in the measure
            /// </summary>
            /// <returns>Http query url</returns>
            protected override string SetQueryUrl()
            {
                //Debug.Log(BuildUrlTagList("type"));
                return BuildUrlTagList("type");
            }

            protected override string SetUpdateUrl()
            {
                return BuildUrlTagList("type");
            }
            #endregion //PRIVATE METHODS
        }
    }
}