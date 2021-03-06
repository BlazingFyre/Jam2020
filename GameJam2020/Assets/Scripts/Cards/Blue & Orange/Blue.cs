using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;

public class Blue : CardHalf
{

    public override string HalfName { get; set; } = "Blue";
    public override string HalfText { get; set; } = "Tire target.";

    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<SpiritHalf>() != null;
    }

    public override void CardFunction(GameObject target)
    {
        actionLog.Enter(new ActionLog.SleepChange(
            GetController(),
            target.GetComponent<SpiritHalf>(),
            SleepState.Dreaming
            ));
    }

}
