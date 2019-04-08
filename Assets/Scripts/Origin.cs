using System.Collections; 
using UnityEngine;
using UnityEngine.Networking;

public class Origin : MonoBehaviour
{
    public string results;

    #region PRIVATE_MEMBER_VARIABLES 

    #endregion // PRIVATE_MEMBER_VARIABLES

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary> This method associates the parent VuMarkId to Origin 
    ///     child game object using the given string Id then starts
    ///     a new coroutine that sends a GET request for JSON data 
    ///     based on the given Id. </summary> 
    /// <param><c>VuMarkId</c> is the tracked VuMark's Id.</param>
    public void Init(string VuMarkId)
    { 
        string originid = VuMarkId;
        Debug.Log("New Origin w/ ID " + originid);
        StartCoroutine(JSONBuilder.instance.GetText(originid));
    }
    

}
