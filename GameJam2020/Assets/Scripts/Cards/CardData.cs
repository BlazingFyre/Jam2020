using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardData : MonoBehaviour
{

    string name;
    string text;
    string art;
    public Type function;

    public CardData(string name, string text, string art, Type function)
    {
        this.name = name;
        this.text = text;
        this.art = art;
        this.function = function;
    }

    public void Start()
    {
        function = typeof(Function2);
    }

    public string GetName()
    {
        return name;
    }
    public string GetText()
    {
        return text;
    }
    public string GetArt()
    {
        return art;
    }
    public Type GetFunction()
    {
        return function;
    }

}
