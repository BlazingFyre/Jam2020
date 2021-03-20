using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static SleepStates;

public class Whole : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public Half side1;
    public Half side2;
    // --------------------------------------------------------------------------------------------

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

    public void SwapHalves()
    {
        // Swap side variables
        Half temp = side1;
        side1 = side2;
        side2 = temp;

        // Change each Half's side
        side1.SwitchSide();
        side2.SwitchSide();
    }

}
