using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : CardHalf
{

    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<Spirit>() != null;
    }

    public override void Play(GameObject target)
    {
        target.GetComponent<Spirit>().Tire();

        Discard();
    }

}
