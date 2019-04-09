using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ARDesign.Serialize;
using ARDesign.Influx;

namespace ARDesign.Widgets
{
    // This ensures type agnostic functions can be run on InfluxReaders - all widgets should inherit from THIS, NOT InfluxReader!
    public abstract class InfluxType<T> : InfluxReader
    {
        protected T dataVals;
        protected abstract void CastWidget();

        // Use this for initialization
        void Awake()
        {
            widget = this.GetComponent<WidgetHandler>();
            //Debug.Log("Start called on " + widget.name);
            CastWidget();
        }

        public T GetData()
        {
            return dataVals;
        }

    }

    public abstract class InfluxReader : MonoBehaviour
    {

        //determines if widget needs to be updated or is static
        public bool toUpdate = false;

        protected string measure;
        protected string building;
        protected string room;
        private InfluxSetup parent = null;

        private string urlToQuery;
        private string urlToUpdate;

        protected WidgetHandler widget;

        [NonSerialized]
        public bool isDataBuilt = false;
        [NonSerialized]
        public bool isSetup = false;


        // Sets the base database values - should be called before anything else!
        public void SetDBVals(string m, string b, string r)
        {
            parent = gameObject.GetComponentInParent<InfluxSetup>();
            measure = m;
            building = b;
            room = r;

            urlToQuery = SetQueryUrl();
            urlToUpdate = SetQueryUrl();
        }

        //Sets the string for querying the DB
        protected abstract string SetQueryUrl();

        //Sets the update string
        protected abstract string SetUpdateUrl();

        // Update the values in the widget, by querying the database
        public void UpdateVals()
        {
            StartCoroutine(RefreshCurVals());
        }

        // Set the values in the widget, by querying the database
        public void SetVals()
        {
            StartCoroutine(BuildValues());
            isSetup = true;
        }

        //Call back function for updating the widget - webResult is the raw JSON from the webrequest
        protected abstract void ParseUpdateText(string webResult);

        IEnumerator RefreshCurVals()
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

        IEnumerator BuildValues()
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

        protected abstract void ParseSetUpText(string webReturn);


        // QUERY HELPER FUNCTIONS


        protected string BuildUrl(string query)
        {
            return parent.BuildUrlWithQuery(query);
        }

        // Builds URLs for widgets to query with a fixed limit on values
        public string BuildUrlWithLimit(int limit)
        {
            return BuildUrl("SELECT type,value FROM " + measure + " WHERE \"building\"=\'" + building + "\' AND \"room\"=\'" + room + "\' ORDER BY DESC LIMIT " + limit);

        }

        // Builds URLs for widgets to query with out a fixed limit on values
        public string BuildUrlWithoutLimit()
        {

            return BuildUrl("SELECT type,value FROM " + measure + " WHERE \"building\"=\'" + building + "\' AND \"room\"=\'" + room + "\' ORDER BY DESC");

        }

        // Builds URLs for widgets to query with a fixed limit on values
        public string BuildUrlWithLimitSetType(string type, int limit)
        {
            return BuildUrl("SELECT value FROM " + measure + " WHERE \"building\"=\'" + building + "\' AND \"room\"=\'" + room + "\' AND \"type\"=\'" + type + "\' ORDER BY DESC LIMIT " + limit);

        }

        // Builds URLs for widgets to query with out a fixed limit on values with set type
        public string BuildUrlWithoutLimitSetType(string type)
        {

            return BuildUrl("SELECT value FROM " + measure + " WHERE \"building\"=\'" + building + "\' AND \"room\"=\'" + room + "\' AND \"type\"=\'" + type + "\' ORDER BY DESC");

        }

        // Builds list of unique tags - to use for getting labels
        public string BuildUrlTagList(string keyName)
        {
            return BuildUrl("SHOW TAG VALUES FROM " + measure + " WITH KEY = \"" + keyName + "\"");
        }
    }
}

