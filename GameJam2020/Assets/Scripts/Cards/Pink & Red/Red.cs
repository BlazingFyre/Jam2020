using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionLog;

public class Red : CardHalf
{

    // Called when component is added; used to overwrite fields
    public void Reset()
    {
        halfName = "Red";
        halfText = "Apply 3 Fire.";
    }

    // Return true if "target" is a valid target for this card
    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<SpiritHalf>() != null;
    }

    // The card's meat, bread, and butter. Usually just entering Actions
    public override void CardFunction(GameObject target)
    {
        actionLog.Enter(new ApplyStatus(
            GetController(),
            target.GetComponent<SpiritHalf>(),
            typeof(FireStatus),
            3
            ));
    }

}
