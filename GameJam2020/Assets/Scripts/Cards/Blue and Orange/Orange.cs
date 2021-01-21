using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : CardHalf
{

    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<Spirit>() != null;
    }

    public override void Play(GameObject target)
    {
        target.GetComponent<Spirit>().Damage(5);

        Discard();
    }

}
