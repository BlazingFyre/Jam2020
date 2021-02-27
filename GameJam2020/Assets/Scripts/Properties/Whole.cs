using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static SleepStates;

public class Whole : MonoBehaviour
{

    public Half side1;
    public Half side2;

    public void InitSides(GameObject objectA, GameObject objectB)
    {
        /*
        objectA.AddComponent<Half>();
        objectB.AddComponent<Half>();

        side1 = objectA.GetComponent<Half>();
        side2 = objectB.GetComponent<Half>();

        side1.InitVars(this, side2, Side.A);
        side2.InitVars(this, side1, Side.B);
        */
    }

    public Half GetSide(Side side)
    {
        if (side1.GetSide() == side)
        {
            return side1;
        }
        else
        {
            return side2;
        }
    }

    public Half GetSide(SleepState state)
    {
        return GetSide(GetComponent<Use>().GetController().GetSpirit(state).GetComponent<Half>().GetSide());
    }

}
