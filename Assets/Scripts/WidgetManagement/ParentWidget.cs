using ARDesign.Influx;
using System;
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
        public class ParentWidget : WidgetReader
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

            private Serialize.DBWidget dbwid;

            private string[] DataVals
            {
                get
                {
                    return dbwid.Values;
                }

                set
                {
                    dbwid.Values = value;
                }
            }
            #endregion //PRIVATE MEMBER VARIABLES

            #region PUBLIC_METHODS
            /// <summary>
            /// Sets the base database values - should be called before anything else!
            /// </summary>
            /// <param name="wid">Deserialized struct to build parent from</param>
            public new void SetDBVals(Serialize.DBWidget wid)
            {
                base.SetDBVals(wid);
                dbwid = wid;
            }

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

            /// <summary>
            /// Returns the list of values
            /// </summary>
            /// <returns>dataVals for this widget, of type specified in InfluxType</returns>
            public string[] GetData()
            {
                return DataVals;
            }
            #endregion //PUBLIC_METHODS

            #region UNITY_MONOBEHAVIOUR_METHODS
            // Use this for initialization
            void Awake()
            {
                widget = this.GetComponent<ParentWidgetHandler>();
            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS

            #region PRIVATE_METHODS

            /// <summary>
            /// Sets given child data widget to use the same Influx server info as its parent
            /// </summary>
            /// <param name="child">DataWidget child</param>
            private void SetupChildWidget(DataWidget child)
            {
                child.SetDBVals(measure,parent);
            }

            /// <summary>
            /// Statically sets widget values from deserialized parent
            /// </summary>
            public override void SetVals()
            {
                DataVals = dbwid.Values;
                wid.BuildChildren(DataVals);

                //Code for building children should go here
                JSONBuilder.instance.AddWidget(children);
            }
            #endregion //PRIVATE METHODS
        }
    }
}