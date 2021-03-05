using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        target.GetComponent<SpiritHalf>().Tire();
    }

}
