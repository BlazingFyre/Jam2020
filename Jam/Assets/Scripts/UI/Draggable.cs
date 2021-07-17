using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class DragSettings
    {
        public bool OverridesSnapping = true;
    }

    private bool IsBeingDragged = false;

    public DragSettings Settings;

    private Snappable Snappable;

    public void Start()
    {
        Snappable = GetComponent<Snappable>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsBeingDragged = true;

        if (Snappable != null
            && Settings.OverridesSnapping)
        {
            Snappable.Pause();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsBeingDragged = false;

        if (Snappable != null
            && Settings.OverridesSnapping)
        {
            Snappable.Unpause();
        }
    }

    public void Update()
    {
        if (IsBeingDragged)
        {
            if (GetComponent<Snappable>() == null || Settings.OverridesSnapping)
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
}
