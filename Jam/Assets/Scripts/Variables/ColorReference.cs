using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ColorReference
{
    public bool UseConstant = false;
    public Color ConstantValue;
    public ColorVariable Variable;

    public Color Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
