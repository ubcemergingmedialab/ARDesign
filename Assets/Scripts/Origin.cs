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

    /// <summary>
    /// This method associates the parent VuMarkId to Origin
    /// </summary> 
    public void Init(string VuMarkId)
    { 
        string originid = VuMarkId;
        Debug.Log("New Origin w/ ID " + originid);
        StartCoroutine(JSONBuilder.instance.GetText(originid));
    }
    

}
