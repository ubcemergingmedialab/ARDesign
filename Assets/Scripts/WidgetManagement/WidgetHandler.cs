using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARDesign.Widgets
{
    /// <summary>
    /// Typed widget handler class. Type indicates form of Widget content and should derive from InfluxReader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WidgetType<T> : WidgetHandler where T : InfluxReader
    {

        /// <summary>
        /// Reader indicates the data loading functions of the widget. Should derive from InfluxReader
        /// </summary>
        protected T reader;

        void Awake()
        {
            reader = this.GetComponent<T>();
            influxWidget = reader;
        }

    }

    
    /// <summary>
    /// WidgetHandler refers to general data-agnostic widget functionality
    /// </summary>
    public abstract class WidgetHandler : MonoBehaviour
    {

        protected bool isEnable = false;

        [SerializeField]
        protected GameObject WidgetObj;
        [SerializeField]
        protected GameObject Connector;

        protected InfluxReader influxWidget;
        
        /// <summary>
        /// Turns widget off and on - default behavior hides widget object but exposes root node. Override for custom behaviour
        /// </summary>
        /// <param name="enable"></param>
        public virtual void EnableWidget(bool enable)
        {
            Connector.SetActive(enable);
            WidgetObj.SetActive(enable);
            isEnable = !isEnable;
        }

        public void SetWidgetObjDistance(float dist)
        {
            Vector3 oldPos = WidgetObj.transform.localPosition;
            WidgetObj.transform.localPosition = new Vector3(oldPos.x, oldPos.y, dist);
        }

    }
}
