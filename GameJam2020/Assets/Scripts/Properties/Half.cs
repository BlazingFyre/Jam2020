using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;

public class Half : MonoBehaviour
{

    public Whole whole;
    public Half flip;

    public Side side;

    public void InitVars(Whole whole, Half flip, Side side)
    {
        this.whole = whole;
        this.flip = flip;
        this.side = side;
    }

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

    public void Mirror()
    {
        this.SwitchSide();
        flip.SwitchSide();
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
