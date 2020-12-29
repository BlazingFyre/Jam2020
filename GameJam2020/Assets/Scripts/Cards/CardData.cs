using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{

    string name;
    string text;
    string art;
    string function;

    public CardData(string name, string text, string art, string function)
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
    public string getFunction()
    {
        return function;
    }

}
