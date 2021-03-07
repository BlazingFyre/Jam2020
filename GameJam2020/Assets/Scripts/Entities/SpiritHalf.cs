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
    }

}