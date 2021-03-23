using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;
using TMPro;

public class SpiritHalf : Entity
{

    // ---- Preloaded -----------------------------------------------------------------------------
    // The state of a spirit (either Waking or Dreaming)
    public SleepState state;
    // ---- Preloaded (optional) ------------------------------------------------------------------
    public TextMeshProUGUI sleepDisplay;
    // --------------------------------------------------------------------------------------------

    public SleepState GetSleepState()
    {
        return state;
    }

    public void SetSleepState(SleepState state)
    {
        this.state = state;

        // Displays the SleepState of this Spirit
        if (sleepDisplay != null)
        {
            sleepDisplay.text = (state == SleepState.Waking) ? "+" : "-";
        }

        // Update the hand according to sleep state changes
        GetComponent<Half>().GetWhole().GetComponent<SpiritWhole>().GetHand().GetComponent<HandAligner>().UpdateSleepAlignment();
    }

}