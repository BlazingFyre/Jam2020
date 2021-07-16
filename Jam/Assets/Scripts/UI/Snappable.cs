using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snappable : MonoBehaviour
{
    public enum SnapRole { Snapper, Snappee }
    public enum SnapCondition { Manual, WhenColliding }
    public enum SnapStyle { SetTransform, NudgeTransform }
    public enum SnapChannel { All }

    [Serializable]
    public class SnapSettings
    {
        [Header("Basic Settings")]
        public SnapRole Role;
        public SnapChannel Channel = SnapChannel.All;

        [Header("Snapper Settings")]
        public SnapCondition Condition;
        public SnapStyle Style;
        public float NudgeSpeed;

        [Header("Optional Settings")]
        public Collider Collider;
    }

    public bool Paused = false;
    public Snappable SnappedObject;

    public SnapSettings Settings;

    void Start()
    {
        if (Settings.Collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Settings.Collider = GetComponent<Collider>();
        }
    }

    void FixedUpdate()
    {
        if (!Paused && SnappedObject != null)
        {
            if (Settings.Style == SnapStyle.SetTransform)
            {
                SnappedObject.transform.position = gameObject.transform.position;
                SnappedObject.transform.rotation = gameObject.transform.rotation;
            }
            else if (Settings.Style == SnapStyle.NudgeTransform)
            {
                //TODO: Make it nudge the snapped object towards this object with the set force
            }
        }
    }

    // For manual snapping

    public void Snap(Snappable target)
    {
        if (SnappedObject != null)
        {
            Unsnap(SnappedObject);
        }

        SnappedObject = target;
    }

    public void Unsnap(Snappable target)
    {
        SnappedObject = null;
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Unpause()
    {
        Paused = false;
    }

    // For automatic (collider-based) snapping

    void OnTriggerEnter(Collider col)
    {
        if (Settings.Condition == SnapCondition.WhenColliding
            && Settings.Role == SnapRole.Snapper
            && col.GetComponent<Snappable>() != null
            && col.GetComponent<Snappable>().Settings.Channel == Settings.Channel
            || col.GetComponent<Snappable>().Settings.Channel == SnapChannel.All)
        {

        }
    }

}
