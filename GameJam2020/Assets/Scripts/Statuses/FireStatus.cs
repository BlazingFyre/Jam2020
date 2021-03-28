using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionLog;
using static Phases;

public class FireStatus : Status
{

    public override IEnumerator ProcessPostAction(Action action)
    {

        if (action.GetType() == typeof(PhaseChange) 
            && ((PhaseChange) action).phase == Phase.End
            && GetComponent<Half>().GetWhole().GetComponent<SpiritWhole>() == actionLog.GetComponent<BattleSystem>().GetTurnSpirit())
        {
            yield return action.SubProcess(new Damage(
                action.source,
                GetComponent<SpiritHalf>(),
                amount
                ));

            yield return action.SubProcess(new ApplyStatus(
                action.source,
                GetComponent<SpiritHalf>(),
                this.GetType(),
                -1
                ));
        }

    }

}
