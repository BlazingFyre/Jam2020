using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;

public class SpiritHalf : Entity
{

    public SleepState state;

    public SleepState GetSleepState()
    {
        return state;
    }

    public void SetSleepState(SleepState state)
    {
        this.state = state;

        SpiritHalf flip = GetComponent<Half>().GetFlip().GetComponent<SpiritHalf>();

        // If they are both the same SleepState, force the flip to change
        if (flip.GetSleepState() == state)
        {
            GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>().Enter(new ActionLog.SleepChange(
                GetComponent<Half>().GetWhole().GetComponent<SpiritWhole>(),
                flip,
                (state == SleepState.Waking) ? SleepState.Dreaming : SleepState.Waking
                ));
        }
    }

}