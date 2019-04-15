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
        /// Abstract class for general widgets
        /// </summary>
        public abstract class WidgetReader : MonoBehaviour
        {
            [NonSerialized]
            public bool isDataBuilt = false;
            [NonSerialized]
            public bool isSetup = false;

            #region PRIVATE_MEMBER_VARIABLES
            protected string measure;
            protected InfluxSetup parent = null;

            protected WidgetHandler widget;
            #endregion //PRIVATE_MEMBER_VARIABLES

            #region PUBLIC_METHODS
            /// <summary>
            /// Sets the base database values - should be called before anything else!
            /// </summary>
            /// <param name="m">Measure name</param>
            public void SetDBVals(Serialize.DBWidget w)
            {
                parent = gameObject.GetComponentInParent<InfluxSetup>();
                measure = w.Measure;
            }

            /// <summary>
            /// Sets the base database values - should be called before anything else!
            /// For use in child widgets, where the parent is likely another widget
            /// </summary>
            /// <param name="m">Measure name</param>
            /// <param name="parent">Database configuration container</param>
            public void SetDBVals(string m, InfluxSetup parent)
            {
                this.parent = parent;
                measure = m;

            }
            #endregion //PUBLIC_METHODS

            #region ABSTRACT_METHODS
            /// <summary>
            /// Set the values in the widget
            /// </summary>
            public abstract void SetVals();
            #endregion //ABSTRACT_METHODS

            #region PRIVATE_METHODS

            #endregion //PRIVATE_METHODS

        }
    }
}
