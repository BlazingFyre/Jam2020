using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;
using TMPro;

public class SpiritHalf : Entity
{

    public SleepState state;

    public TextMeshProUGUI sleepDisplay;

    public SleepState GetSleepState()
    {
        return state;
    }

    public void SetSleepState(SleepState state)
    {
        this.state = state;

        // Temporary. Displays the SleepState of this Spirit.
        sleepDisplay.text = (state == SleepState.Waking) ? "+" : "-";
    }

}