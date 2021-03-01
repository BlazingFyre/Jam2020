using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHalfTemplate : CardHalf
{

    public override bool IsTargetable(GameObject target)
    {
        return false;
    }

    public override void CardFunction(GameObject target)
    {

    }

}
