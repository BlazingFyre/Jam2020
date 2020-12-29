using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardData : MonoBehaviour
{

    string name;
    string text;
    string art;
    Type function;

    public CardData(string name, string text, string art, Type function)
    {
        this.name = name;
        this.text = text;
        this.art = art;
        this.function = function;
    }

    public string getName()
    {
        return name;
    }
    public string getText()
    {
        return text;
    }
    public string getArt()
    {
        return art;
    }
    public Type getFunction()
    {
        return function;
    }

}
