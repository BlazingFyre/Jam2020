using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionLog;

public class Orange : CardHalf
{

    public void Reset()
    {
        halfName = "Orange";
        halfText = "Deal 5 damage.";
    }

    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<SpiritHalf>() != null;
    }

    public override void CardFunction(GameObject target)
    {
        actionLog.Enter(new Damage(
            GetController(),
            target.GetComponent<SpiritHalf>(),
            5
            ));
    }

}
