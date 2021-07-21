using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snappable : MonoBehaviour
{
    public enum SnapRole { Snapper, Snappee }
    public enum SnapCondition { Manual }
    public enum SnapStyle { NudgeTransform, SetTransform }
    public enum SnapChannel { All }

    [Serializable]
    public class SnapSettings
    {
        [Header("Basic Settings")]
        public SnapRole Role = SnapRole.Snapper;
        public SnapChannel Channel = SnapChannel.All;

        [Header("Snapping Settings")]
        public SnapCondition Condition = SnapCondition.Manual;
        public SnapStyle Style = SnapStyle.NudgeTransform;
        public FloatReference NudgeSpeed;
    }

    public bool Paused = false;
    public Snappable Snapper;

    public SnapSettings Settings;

    //[HideInInspector]
    public bool IsAligned = false;

    public void FixedUpdate()
    {
        if (Snapper != null)
        {
            if (!Paused && !Snapper.Paused)
            {
                if (Settings.Style == SnapStyle.SetTransform)
                {
                    transform.position = Snapper.transform.position;
                }
                else if (Settings.Style == SnapStyle.NudgeTransform)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        Snapper.transform.position,
                        Settings.NudgeSpeed.Value
                        );
                }
            }

            IsAligned = (Snapper.transform.position.x == transform.position.x)
                && (Snapper.transform.position.y == transform.position.y);
        }
    }

    // For manual snapping

    public void Snap(Snappable snapper)
    {
        if (Snapper != null)
        {
            Unsnap(Snapper);
        }

        Snapper = snapper;
    }

    public void Unsnap(Snappable snapper)
    {
        Snapper = null;
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Unpause()
    {
        Paused = false;
    }

}
