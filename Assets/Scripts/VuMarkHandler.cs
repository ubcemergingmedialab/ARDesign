/*===============================================================================
Copyright (c) 2016 PTC Inc. All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;


/// <summary>
/// A custom handler which uses the vuMarkManager.
/// </summary>
public class VuMarkHandler : MonoBehaviour
{
    public Origin child;
    public string ID;

    #region PRIVATE_MEMBER_VARIABLES

    private VuMarkManager mVuMarkManager;
    private VuMarkTarget mClosestVuMark;
    private VuMarkTarget mCurrentVuMark;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    { 
        // register callbacks to VuMark Manager
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        mVuMarkManager.RegisterVuMarkDetectedCallback(OnVuMarkDetected);
        mVuMarkManager.RegisterVuMarkLostCallback(OnVuMarkLost);
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        // unregister callbacks from VuMark Manager
        mVuMarkManager.UnregisterVuMarkDetectedCallback(OnVuMarkDetected);
        mVuMarkManager.UnregisterVuMarkLostCallback(OnVuMarkLost);
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

    /// <summary>
    /// This method will be called whenever a new VuMark is detected
    /// </summary>
    public void OnVuMarkDetected(VuMarkTarget target)
    {
        Debug.Log("New VuMark: " + GetVuMarkID(target) + " name:" + target.Name);
        ID = GetVuMarkID(target);
        // assign ID to VuMark's child GameObject
        child = gameObject.AddComponent<Origin>();
        child.Init(ID); 
    }

    /// <summary>
    /// This method will be called whenever a tracked VuMark is lost
    /// </summary>
    public void OnVuMarkLost(VuMarkTarget target)
    {
        Debug.Log("Lost VuMark: " + GetVuMarkID(target));
    }

    #endregion // PUBLIC_METHODS



    #region PRIVATE_METHODS
     

    private string GetVuMarkID(VuMarkTarget vumark)
    {
        return vumark.InstanceId.StringValue;
    }

    #endregion // PRIVATE_METHODS
}