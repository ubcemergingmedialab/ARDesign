using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web;
using UnityEngine.Networking;
using System.Security.Policy;

public class JSONBuilder : MonoBehaviour {

    public static JSONBuilder instance = null;

    [SerializeField] // Prefab to be used for building widgets
    private GameObject widgetPrefab;
    [SerializeField]
    private string requestUrl = "https://ardesign-config.herokuapp.com/api/scenes/";

    [SerializeField]
    private GameObject success;
    [SerializeField]
    private GameObject webError;
    [SerializeField]
    private GameObject serverCommunication;

    // List of all present widgets 
    private List<InfluxReader> widgets;
    private List<InfluxReader> tempWidgets;

    [SerializeField]
    private bool test = false;

    bool isRunning = false;

    // TO BE REMOVED LATER
    [SerializeField]
    private GameObject testSceneObj; // something like this is needed ofc, but it should be initialized with Vuforia, not placed in the scene as it is here. 
    [SerializeField]
    private string fileName = "test.json";

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
    // TEST function - uses constant file and given obj for scene creation
    private void Test()
    {


        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {


            InitializeScene(File.ReadAllText(filePath), testSceneObj);
        }
    }

    //Called on QR detection
    public void InitializeScene(string json, GameObject origin)
    {
        DBScene scene = JSONHelper.CreateFromJSON(json);
        BuildScene(scene, origin);
        StartCoroutine(SetUpWidgets());
        Debug.Log("Finish init");
    }


	
	public void AddWidget<T>(IList<T> toAdd) where T: InfluxReader
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

    // Builds an influxSetup component on the given object, and populates it with the configuration specified in conf
    // passes scene object that contains all widget location info
    void BuildScene(DBScene conf, GameObject sceneObj)
    {

        InfluxSetup setup = sceneObj.AddComponent<InfluxSetup>();
        setup.Setup(conf);
        foreach (DBWidget wid in conf.Widgets)
        {
            CreateWidgets(wid, sceneObj);
        }
    }

    // Creates the widget objects specified in wid as children of the sceneObj - sceneObj should have an InfluxSetup component. This shouldn't do any computationally expensive tasks - save those for the setupwidget coroutine
    private void CreateWidgets(DBWidget wid, GameObject sceneObj)
    {
        GameObject newWidget = Instantiate(widgetPrefab);
        newWidget.transform.parent = sceneObj.transform;
        newWidget.transform.localPosition = wid.Position;
        InfluxReader r = newWidget.GetComponent<InfluxReader>();
        r.SetDBVals(wid.Measure, wid.Building, wid.Room);
        widgets.Add(r);
    }

    // Updates the values in all widgets
    IEnumerator SetUpWidgets()
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

    // Updates the values in all widgets
    public void UpdateWidgets()
    {
        foreach(InfluxReader r in widgets)
        {
            if (r.toUpdate) {
                r.UpdateVals();
            }
        }
    }

    private bool AllWidgetsSetup()
    {
        foreach (InfluxReader r in widgets)
        {
            if (!r.isSetup)
                return false;
        }
        return true;
    }

    /// This method makes an HTTP GET request to HTTP_URL and stores results as string
    public IEnumerator GetText(string id)
    {
        // create web request object with HTTP verb GET and URL 
        id = id.Substring(0, 24); // needs to be changed depending on id string length
        UnityWebRequest www = UnityWebRequest.Get(requestUrl + id);
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
}




