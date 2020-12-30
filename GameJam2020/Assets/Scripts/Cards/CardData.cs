using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardData : MonoBehaviour
{

    public string name;
    public string text;
    public Sprite art;
    public Type function;
    // For manually entering in card functions
    public string functionName = null;
    

    public CardData(string name, string text, Sprite art, Type function)
    {
        this.name = name;
        this.text = text;
        this.art = art;
        this.function = function;
    }

    public void Start()
    {
        if (functionName != null)
        {
            function = Type.GetType(functionName);
        }
    }

    public string GetName()
    {
        return name;
    }
    public string GetText()
    {
        return text;
    }
    public Sprite GetArt()
    {
        return art;
    }
    public Type GetFunction()
    {
        return function;
    }

}
