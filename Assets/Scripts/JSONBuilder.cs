using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using ARDesign.Widgets;
using ARDesign.Serialize;
using ARDesign.Influx;

namespace ARDesign
{
    /// <summary>
    /// Singleton object for handling scene loading and widget creation
    /// </summary>
    public class JSONBuilder : MonoBehaviour
    {
        public static JSONBuilder instance = null;

        /// <summary>
        /// Prefab to be used for building widgets
        /// </summary>
        [SerializeField]
        private GameObject widgetPrefab;

        /// <summary>
        /// Object shown on successful webrequest
        /// </summary>
        [SerializeField]
        private GameObject success;
        /// <summary>
        /// Object shown on unsuccessful webrequest
        /// </summary>
        [SerializeField]
        private GameObject webError;
        /// <summary>
        /// Object shown during webrequest
        /// </summary>
        [SerializeField]
        private GameObject serverCommunication;

        #region TEST_FIELDS
        [SerializeField]
        private bool test = false;
        [SerializeField]
        private GameObject testSceneObj; // something like this is needed ofc, but it should be initialized with Vuforia, not placed in the scene as it is here. 
        [SerializeField]
        private string fileName = "test.json";
        #endregion //TEST_FIELDS

        #region PRIVATE_MEMBER_VARIABLES
        // List of all present widgets 
        private List<InfluxReader> widgets;
        private List<InfluxReader> tempWidgets;

        private bool isRunning = false;
        #endregion //PRIVATE_MEMBER_VARIABLES

        #region UNITY_MONOBEHAVIOUR_METHODS
        // Use this for initialization
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
            tempWidgets = new List<InfluxReader>();
            widgets = new List<InfluxReader>();


            if (test) Test();

        }
        #endregion //UNITY_MONOBEHAVIOUR_METHODS

        #region PUBLIC_METHODS
        
        /// <summary>
        /// Deserializes a scene configuration and builds it on the origin location
        /// </summary>
        /// <param name="json">Scene configuration to build</param>
        /// <param name="origin">Parent object to build scene on - should be vumark location</param>
        public void InitializeScene(string json, GameObject origin)
        {
            DBScene scene = Serialize.Utility.CreateFromJSON(json);
            BuildScene(scene, origin);
            StartCoroutine(SetUpWidgets());
            Debug.Log("Finish init");
        }

        /// <summary>
        /// Adds widgets to list of active widgets
        /// </summary>
        /// <typeparam name="T">Widget types</typeparam>
        /// <param name="toAdd">List of widgets to add</param>
        public void AddWidget<T>(IList<T> toAdd) where T : InfluxReader
        {
            if (isRunning)
            {
                tempWidgets.AddRange(toAdd.Cast<InfluxReader>());
            }
            else
            {
                widgets.AddRange(toAdd.Cast<InfluxReader>());
            }

        }

        /// <summary>
        /// Adds widgets to list of active widgets
        /// </summary>
        /// <typeparam name="T">Widget types</typeparam>
        /// <param name="toAdd">Widget to add</param>
        public void AddWidget<T>(T toAdd) where T : InfluxReader
        {
            if (isRunning)
            {
                tempWidgets.Add(toAdd);
            }
            else
            {
                widgets.Add(toAdd);
            }
        }

        
        /// <summary>
        /// Starts get request to fetch config for a given ID
        /// </summary>
        /// <param name="id">ID of scene config to load</param>
        /// <returns>Get request for ID</returns>
        public IEnumerator GetText(string id)
        {
            // create web request object with HTTP verb GET and URL 
            id = id.Substring(0, 24); // needs to be changed depending on id string length
            UnityWebRequest www = Receiver.GetFromId(id);
            Debug.Log("Created Web Request object for: " + www.url);

            www.downloadHandler = new DownloadHandlerBuffer();
            serverCommunication.SetActive(true);

            yield return www.SendWebRequest();

            Debug.Log("Response Code: " + www.responseCode);
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                webError.SetActive(true);
                serverCommunication.SetActive(false);
            }
            else
            {
                // retrieve results
                string results = www.downloadHandler.text;
                Debug.Log("Printing results...");
                Debug.Log(results);
                yield return www.downloadHandler.text;
                success.SetActive(true);
                serverCommunication.SetActive(false);
            }
        }

        
        /// <summary>
        /// Updates all widgets that are set to update
        /// </summary>
        public void UpdateWidgets()
        {
            foreach (InfluxReader r in widgets)
            {
                if (r.toUpdate)
                {
                    r.UpdateVals();
                }
            }
        }
        #endregion //PUBLIC_METHODS

        #region PRIVATE_METHODS
        /// <summary>
        /// Builds an influxSetup component on the given object, and populates it with the configuration specified in conf
        /// passes scene object that contains all widget location info
        /// </summary>
        /// <param name="conf">Scene to build</param>
        /// <param name="sceneObj">Object to parent scene config</param>
        private void BuildScene(DBScene conf, GameObject sceneObj)
        {

            InfluxSetup setup = sceneObj.AddComponent<InfluxSetup>();
            setup.Setup(conf);
            foreach (DBWidget wid in conf.Widgets)
            {
                CreateWidgets(wid, sceneObj);
            }
        }


        /// <summary>
        /// Creates the widget objects specified in wid as children of the sceneObj
        /// This shouldn't do any computationally expensive tasks - save those for the setupwidget coroutine
        /// </summary>
        /// <param name="wid">Widget to create</param>
        /// <param name="sceneObj">Parent of widgets - sceneObj should have an InfluxSetup component</param>
        private void CreateWidgets(DBWidget wid, GameObject sceneObj)
        {
            GameObject newWidget = Instantiate(widgetPrefab);
            newWidget.transform.parent = sceneObj.transform;
            newWidget.transform.localPosition = wid.Position;
            InfluxReader r = newWidget.GetComponent<InfluxReader>();
            r.SetDBVals(wid.Measure);
            widgets.Add(r);
        }

        
        /// <summary>
        /// Loads data on all widgets
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator SetUpWidgets()
        {
            //Stop thread when all widgets are build - maybe consider wrapping in a while true loop later?
            while (!AllWidgetsSetup())
            {
                isRunning = true;
                //Set up current widgets
                foreach (InfluxReader r in widgets)
                {
                    if (!r.isSetup)
                    {
                        yield return null;
                        r.SetVals();
                    }

                }
                isRunning = false;
                //Check if new widgets need to be added
                if (tempWidgets.Count != 0)
                {
                    widgets.AddRange(tempWidgets);
                    tempWidgets.Clear();
                }
                yield return null;
            }

        }

        /// <summary>
        /// Checks if all widgets are setup
        /// </summary>
        /// <returns>True if all widgets have been set up</returns>
        private bool AllWidgetsSetup()
        {
            foreach (InfluxReader r in widgets)
            {
                if (!r.isSetup)
                    return false;
            }
            return true;
        }

        
        /// <summary>
        /// Builds a scene with a static json and editor specified object
        /// </summary>
        private void Test()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            if (File.Exists(filePath))
            {

                InitializeScene(File.ReadAllText(filePath), testSceneObj);
            }
        }
        #endregion //PRIVATE_METHODS
    }
}



