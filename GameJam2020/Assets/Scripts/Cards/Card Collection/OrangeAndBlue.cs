using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeAndBlue : CardFunction
{

    public void Start()
    {
        nameA = "Orange";
        nameB = "Blue";
        //descriptionA;
        //descriptionB;
        //artA;
        //artB;
    }

    public override bool IsTargetableA(GameObject target)
    {
        return target.GetComponent<Entity>() != null;
    }

    public override void PlayA(GameObject target)
    {
        target.GetComponent<Entity>().Damage(5);

        DiscardA();
    }

    public override bool IsTargetableB(GameObject target)
    {
        return target.GetComponent<SpiritSideControl>() != null;
    }

    public override void PlayB(GameObject target)
    {
        target.GetComponent<Entity>().Heal(5);
        target.GetComponent<SpiritSideControl>().Tire();

        DiscardB();
    }

}
