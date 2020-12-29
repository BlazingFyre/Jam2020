using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CardData dataA = new CardData(typeof(A));
        thing test1 = (thing)System.Activator.CreateInstance(dataA.functionality);
        test1.Activate();
        CardData dataB = new CardData(typeof(B));
        thing test2 = (thing)System.Activator.CreateInstance(dataB.functionality);
        test2.Activate();
        // dataA.functionality();
    }
}
