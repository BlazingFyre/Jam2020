using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionLog;

public class Pink : CardHalf
{

    // Called when component is added; used to overwrite fields
    public void Reset()
    {
        halfName = "Pink";
        halfText = "Apply 3 Regeneration.";
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
            typeof(HealStatus),
            3
            ));
    }

}
