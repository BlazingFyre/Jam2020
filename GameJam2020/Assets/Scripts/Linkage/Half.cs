using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;

public class Half : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public Whole whole;
    public Half flip;

    // The Side of this half (either A or B)
    public Side side;
    // --------------------------------------------------------------------------------------------

    public Whole GetWhole()
    {
        return whole;
    }

    public Half GetFlip()
    {
        return flip;
    }

    public Side GetSide()
    {
        return side;
    }

    public void SwitchSide()
    {
        if (side == Side.A)
        {
            side = Side.B;
        }
        else
        {
            side = Side.A;
        }
    }

}
