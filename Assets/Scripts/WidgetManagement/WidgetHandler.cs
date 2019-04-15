using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ARDesign
{
    namespace Widgets
    {
        /// <summary>
        /// Typed widget handler class. Type indicates form of Widget content and should derive from InfluxReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class WidgetType<T> : WidgetHandler where T : WidgetReader
        {
            protected new T reader;

            #region UNITY_MONOBEHAVIOUR_METHODS
            void Awake()
            {
                reader = this.GetComponent<T>();
            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS
        }

        /// <summary>
        /// Refers to general data-agnostic widget functionality
        /// </summary>
        public abstract class WidgetHandler : MonoBehaviour
        {
            #region PRIVATE_MEMBER_VARIABLES
            protected bool isEnable = false;

            [SerializeField]
            protected GameObject WidgetObj;
            [SerializeField]
            protected GameObject Connector;

            protected WidgetReader reader;
            #endregion //PRIVATE_MEMBER_VARIABLES

            #region PUBLIC_METHODS
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

            /// <summary>
            /// Sets distance from object to root node
            /// </summary>
            /// <param name="dist">Distance away from root node</param>
            public void SetWidgetObjDistance(float dist)
            {
                Vector3 oldPos = WidgetObj.transform.localPosition;
                WidgetObj.transform.localPosition = new Vector3(oldPos.x, oldPos.y, dist);
            }
            #endregion //PUBLIC_METHODS
        }
    }
}
