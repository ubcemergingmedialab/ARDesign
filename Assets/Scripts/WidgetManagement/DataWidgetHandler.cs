using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ARDesign
{
    namespace Widgets
    {
        /// <summary>
        /// Handler class for data displaying widgets
        /// </summary>
        public class DataWidgetHandler : WidgetType<DataWidget>
        {

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

            #region UNITY_MONOBEHAVIOUR_METHODS
            private void Update()
            {
                curval.text = reader.GetCurrentValue().y.ToString();
                DateTime time = new DateTime((long)reader.GetCurrentValue().x);
                curtime.text = time.ToString();
            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS

            #region PUBLIC_METHODS
            /// <summary>
            /// Sets label of widget ot displat type of data
            /// </summary>
            public void UpdateLabel()
            {
                label.text = reader.GetLabel();
                labelText.text = reader.GetLabel();
            }
            #endregion //PUBLIC_METHODS
        }
    }
}
