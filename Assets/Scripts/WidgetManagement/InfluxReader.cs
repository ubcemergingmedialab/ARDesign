using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ARDesign.Serialize;
using ARDesign.Influx;

namespace ARDesign
{
    namespace Widgets
    {
        /// <summary>
        /// This ensures type agnostic functions can be run on InfluxReaders - 
        /// All widgets should inherit from THIS, NOT InfluxReader!
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        public abstract class InfluxType<T> : InfluxReader
        {
            protected T dataVals;

            protected abstract void CastWidget();

            #region UNITY_MONOBEHAVIOUR_METHODS
            // Use this for initialization
            void Awake()
            {
                widget = this.GetComponent<WidgetHandler>();
                //Debug.Log("Start called on " + widget.name);
                CastWidget();
            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS

            #region PUBLIC_METHODS
            /// <summary>
            /// Returns the data for the widget
            /// </summary>
            /// <returns>dataVals for this widget, of type specified in InfluxType</returns>
            public T GetData()
            {
                return dataVals;
            }
            #endregion //PUBLIC_METHODS
        }

        /// <summary>
        /// Abstract class for querying Influx data to widgets.
        /// Includes implemented methods for building useful queries - see Query Helper Functions
        /// </summary>
        public abstract class InfluxReader : MonoBehaviour
        {
            //determines if widget needs to be updated or is static
            public bool toUpdate = false;
            [NonSerialized]
            public bool isDataBuilt = false;
            [NonSerialized]
            public bool isSetup = false;

            #region PRIVATE_MEMBER_VARIABLES
            protected string measure;
            protected InfluxSetup parent = null;

            private string urlToQuery;
            private string urlToUpdate;

            protected WidgetHandler widget;
            #endregion //PRIVATE_MEMBER_VARIABLES

            #region PUBLIC_METHODS
            /// <summary>
            /// Sets the base database values - should be called before anything else!
            /// </summary>
            /// <param name="m">Measure name</param>
            public void SetDBVals(string m)
            {
                parent = gameObject.GetComponentInParent<InfluxSetup>();
                measure = m;

                urlToQuery = SetQueryUrl();
                urlToUpdate = SetQueryUrl();
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

                urlToQuery = SetQueryUrl();
                urlToUpdate = SetQueryUrl();
            }

            /// <summary>
            /// Update the values in the widget, by querying the database.
            /// </summary>
            public void UpdateVals()
            {
                StartCoroutine(RefreshCurVals());
            }

            /// <summary>
            /// Set the values in the widget, by querying the database.
            /// Heavy overhead, depending on data type
            /// </summary>
            public void SetVals()
            {
                StartCoroutine(BuildValues());
                isSetup = true;
            }
            #endregion //PUBLIC_METHODS

            #region ABSTRACT_METHODS
            /// <summary>
            /// Sets the URL and query for setting up the widget
            /// Called automatically on widget creation
            /// </summary>
            /// <returns>URL for setup query</returns>
            protected abstract string SetQueryUrl();

            /// <summary>
            /// Sets the URL and query for updating up the widget
            /// Called automatically on widget creation
            /// </summary>
            /// <returns>URL for update query</returns>
            protected abstract string SetUpdateUrl();

            /// <summary>
            /// Call back function for building the widget
            /// </summary>
            /// <param name="webReturn">Result from setup query</param>
            protected abstract void ParseSetUpText(string webReturn);

            /// <summary>
            /// Call back function for updating the widget
            /// </summary>
            /// <param name="webReturn">Result from update query</param>
            protected abstract void ParseUpdateText(string webReturn);

            #endregion //ABSTRACT_METHODS

            #region PRIVATE_METHODS
            /// <summary>
            /// Updates values
            /// </summary>
            /// <returns></returns>
            private IEnumerator RefreshCurVals()
            {
                WWW web = new WWW(urlToUpdate);
                yield return web;

                if (web.error != null)
                {
                    Debug.Log(web.error);
                }
                else
                {
                    ParseUpdateText(web.text);
                }

            }

            /// <summary>
            /// Builds all values from the setup query
            /// </summary>
            /// <returns></returns>
            private IEnumerator BuildValues()
            {
                WWW web = new WWW(urlToQuery);
                yield return web;
                if (web.error != null)
                {
                    Debug.Log(web.error);
                }
                else
                {
                    ParseSetUpText(web.text);
                }
            }
            #endregion //PRIVATE_METHODS

            #region QUERY_HELPER_FUNCTIONS

            /// <summary>
            /// Builds a https Influx query for the given string
            /// </summary>
            /// <param name="query">Influx query to encode</param>
            /// <returns>URL for the query</returns>
            protected string BuildUrl(string query)
            {
                return parent.BuildUrlWithQuery(query);
            }

            /// <summary>
            /// Builds a https Influx query to return a fixed limit of values
            /// </summary>
            /// <param name="limit">Number of values to return</param>
            /// <returns>URL for the query</returns>
            public string BuildUrlWithLimit(int limit)
            {
                return BuildUrl("SELECT type,value FROM " + measure + " WHERE \"building\"=\'" + parent.Building + "\' AND \"room\"=\'" + parent.Room + "\' ORDER BY DESC LIMIT " + limit);

            }


            /// <summary>
            /// Builds a https Influx query to return all values
            /// </summary>
            /// <returns>URL for the query</returns>
            public string BuildUrlWithoutLimit()
            {

                return BuildUrl("SELECT type,value FROM " + measure + " WHERE \"building\"=\'" + parent.Building + "\' AND \"room\"=\'" + parent.Room + "\' ORDER BY DESC");

            }

            /// <summary>
            /// Builds a https Influx query to return a fixed limit of values of a given type
            /// </summary>
            /// <param name="type">Type of value to fetch</param>
            /// <param name="limit">Number of values to return</param>
            /// <returns>URL for the query</returns>
            public string BuildUrlWithLimitSetType(string type, int limit)
            {
                return BuildUrl("SELECT value FROM " + measure + " WHERE \"building\"=\'" + parent.Building + "\' AND \"room\"=\'" + parent.Room + "\' AND \"type\"=\'" + type + "\' ORDER BY DESC LIMIT " + limit);

            }

            /// <summary>
            /// Builds a https Influx query to return all values of a given type
            /// </summary>
            /// <param name="type">Type of value to fetch</param>
            /// <returns>URL for the query</returns>
            public string BuildUrlWithoutLimitSetType(string type)
            {

                return BuildUrl("SELECT value FROM " + measure + " WHERE \"building\"=\'" + parent.Building + "\' AND \"room\"=\'" + parent.Room + "\' AND \"type\"=\'" + type + "\' ORDER BY DESC");

            }

            /// <summary>
            /// Builds a https Influx query to return all tags for a given key
            /// Use for building list of types in a measurement
            /// </summary>
            /// <param name="keyName">Key to fetch tags for</param>
            /// <returns>URL for the query</returns>
            public string BuildUrlTagList(string keyName)
            {
                return BuildUrl("SHOW TAG VALUES FROM " + measure + " WITH KEY = \"" + keyName + "\"");
            }
            #endregion //QUERY_HELPER_FUNCTIONS
        }
    }
}

