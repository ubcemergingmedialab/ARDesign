using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetActivateInputHandler : MonoBehaviour, IInputClickHandler
{

    private bool isEnable = false;
    [SerializeField]
    private WidgetHandler parent;


    // Responds to taps from user: enables or disables widget depending on current state
    public void OnInputClicked(InputClickedEventData eventData)
    {
        parent.EnableWidget(!isEnable);
        isEnable = !isEnable;
    }
}
