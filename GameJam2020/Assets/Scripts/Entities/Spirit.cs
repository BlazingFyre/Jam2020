using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;

public class Spirit : Entity
{

    public SleepState state;

    public SleepState GetSleepState()
    {
        return state;
    }

    public void Awaken()
    {
        /*
        if (!IsAwake() && flip.IsAwake())
        {
            SetAwake(true);
            flip.SetAwake(false);

            spiritFunction.GetHand().GetComponent<HandAligner>().UpdateAlignment();
        }
        */
    }

    public void Tire()
    {
        /*
        if (IsAwake() && !flip.IsAwake())
        {
            SetAwake(false);
            flip.SetAwake(true);

            spiritFunction.GetHand().GetComponent<HandAligner>().UpdateAlignment();
        }
        */
    }

}