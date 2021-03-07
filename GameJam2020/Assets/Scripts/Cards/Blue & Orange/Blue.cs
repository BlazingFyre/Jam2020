using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;

public class Blue : CardHalf
{

    public override string HalfName { get; set; } = "Blue";
    public override string HalfText { get; set; } = "Sleep.";

    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<SpiritHalf>() != null && target.GetComponent<SpiritHalf>().GetSleepState() != SleepState.Dreaming;
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
