using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatClamped
{
    public float Value;
    public float Max;
    public float Min;

    public void Add(float number)
    {
        Value = Math.Min(Max, Math.Max(Min, Value + number));
    }
}
