﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARDesign
{
    namespace Widgets
    {
        /// <summary>
        /// Handler class for parent widgets - includes building child widget objects
        /// </summary>
        public class ParentWidgetHandler : WidgetType<ParentWidget>
        {
            [SerializeField]
            private GameObject GenericDataWidgetObj;

            //[SerializeField]
            //private float radius;

            public List<GameObject> currentChild;

            //TODO: get rid of this
            [SerializeField]
            private Sprite CO2;
            [SerializeField]
            private Sprite Humid;
            [SerializeField]
            private Sprite PM25;
            [SerializeField]
            private Sprite PM10;
            [SerializeField]
            private Sprite Temp;
            [SerializeField]
            private Sprite VOCs;

            #region PRIVATE_MEMBER_VARIABLES
            private IList<DataWidgetHandler> children;
            private Animator anim;
            #endregion //PRIVATE_MEMBER_VARIABLES

            #region UNITY_MONOBEHAVIOUR_METHODS
            private void Start()
            {
                anim = WidgetObj.GetComponent<Animator>();
            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS

            #region PUBLIC_METHODS
            /// <summary>
            /// Builds a child widget for each label
            /// </summary>
            /// <param name="labels">List of all types to build</param>
            public void BuildChildren(string[] labels)
            {
                children = new List<DataWidgetHandler>();
                for (int i = 0; i < labels.Length; i++)
                {
                    //Vector3 position = new Vector3();
                    GameObject go = Instantiate(GenericDataWidgetObj, currentChild[i].transform);
                    go.transform.localPosition = Vector3.zero;
                    DataWidget toAdd = go.GetComponent<DataWidget>();
                    DataWidgetHandler toAdd2 = go.GetComponent<DataWidgetHandler>();
                    toAdd2.SetWidgetObjDistance(Random.Range(4f, 8f));

                    toAdd.SetLabel(labels[i]);

                    // TODO: remove this
                    toAdd2.graph.sprite = setGraph(labels[i]);

                    reader.AddChild(toAdd);
                    children.Add(toAdd2);
                    toAdd2.EnableWidget(false);
                }

            }
            
            /// <summary>
            /// Turns widget on or off
            /// </summary>
            /// <param name="enable">True if widget is on</param>
            public override void EnableWidget(bool enable)
            {
                if (enable) { anim.SetTrigger("OpenWidget"); };
                Connector.SetActive(!enabled);
                this.WidgetObj.GetComponent<Collider>().enabled = !enable;
                isEnable = !isEnable;
            }
            #endregion //PUBLIC_METHODS

            #region PRIVATE_METHODS
            //TODO: Remove this
            private Sprite setGraph(string label)
            {
                switch (label)
                {
                    case "CO2":
                        return CO2;
                    case "VOCs":
                        return VOCs;
                    case "PM2.5":
                        return PM25;
                    case "PM10":
                        return PM10;
                    case "rel_humidity":
                        return Humid;
                    case "temperature":
                        return Temp;
                    default:
                        return CO2;
                }
            }
            #endregion //PRIVATE_METHODS
        }
    }
}