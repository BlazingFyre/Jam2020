using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : CardHalf
{

    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<SpiritHalf>() != null;
    }

    public override void CardFunction(GameObject target)
    {
        target.GetComponent<SpiritHalf>().Damage(5);
    }

}
