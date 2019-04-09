using ARDesign.Widgets;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetReceiver : InteractionReceiver
{
    [SerializeField]
    private GameObject widgetparent;
    [SerializeField]
    private GameObject widgetobj;

    #region PRIVATE_MEMBER_VARIABLES
    private DataWidgetHandler handler;
    private DataWidget wid;
    private HandDraggable handDraggable;
    private bool isDraggable = false;
    #endregion //PRIVATE_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    private void Start()
    {
        wid = widgetobj.GetComponent <DataWidget>();
        handler = widgetparent.GetComponent<DataWidgetHandler>();

        handDraggable = widgetobj.GetComponent<HandDraggable>();

        // Dragging is off by default
        handDraggable.enabled = isDraggable;

        // When finished dragging, handDraggable should be disabled
        handDraggable.StoppedDragging += stopDragging;
    }

    protected override void InputDown(GameObject obj, InputEventData eventData)
    {
        Debug.Log(obj.name);
        switch (obj.name)
        {
            case "PinButton":
                PinAction();
                break;
            case "StorageButton":
                StorageAction();
                break;
            case "DragButton":
                DragAction();
                break;
            case "ResizeButton":
                ResizeAction();
                break;
            case "CloseButton":
                CloseAction();
                break;
            default:
                base.InputDown(obj, eventData);
                break;
        }

    }
    #endregion //UNITY_MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS
    private void ResizeAction()
    {
        throw new NotImplementedException();
    }

    private void StorageAction()
    {
        throw new NotImplementedException();
    }

    private void PinAction()
    {
        throw new NotImplementedException();
    }

    private void stopDragging()
    {
        handDraggable.enabled = false;
    }

    private void CloseAction()
    {
        handler.EnableWidget(false);
    }
    private void DragAction()
    {
        handDraggable.enabled = !isDraggable;
        isDraggable = !isDraggable;
    }
    #endregion //PRIVATE_METHODS


}

