using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class WidgetDragInputHandler : MonoBehaviour, IInputClickHandler
{

    [SerializeField]
    private GameObject widget;
    private HandDraggable handDraggable;
    //private bool isDraggable = false;

    // Use this for initialization
    void Start()
    {
        handDraggable = widget.GetComponent<HandDraggable>();

        // Dragging is off by default
        handDraggable.enabled = false;

        // When finished dragging, handDraggable should be disabled
        handDraggable.StoppedDragging += stopDragging;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void stopDragging()
    {
        handDraggable.enabled = false;
    }

    // On air tap, dragging should be enabled
    public void OnInputClicked(InputClickedEventData eventData)
    {
        handDraggable.enabled = true;
    }
}
