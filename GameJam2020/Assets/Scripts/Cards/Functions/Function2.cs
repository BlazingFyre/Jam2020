using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function2 : Function
{
    public override void Play(GameObject cardSide, GameObject target)
    {
        Entity targetComponent = target.GetComponent<Entity>();

        if (targetComponent != null)
        {
            targetComponent.Damage(10);
        }

        Discard(cardSide.transform.parent.gameObject);
    }

    public override bool IsTargetable(GameObject target)
    {
        return target.GetComponent<Entity>() != null;
    }

}
