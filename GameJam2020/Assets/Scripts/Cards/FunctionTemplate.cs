using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTemplate : CardFunction
{

    public void Start()
    {
        nameA = "";
        nameB = "";
        descriptionA = "";
        descriptionB = "";
        //artA;
        //artB;
    }

    public override bool IsTargetableA(GameObject target)
    {
        return false;
    }

    public override void PlayA(GameObject target)
    {
        DiscardA();
    }

    public override void DiscardA()
    {
        container.DrawTarget(gameObject);
        controller.GetGraveyard().PlaceTop(gameObject);
    }

    public override bool IsTargetableB(GameObject target)
    {
        return false;
    }

    public override void PlayB(GameObject target)
    {
        DiscardB();
    }

    public override void DiscardB()
    {
        container.DrawTarget(gameObject);
        controller.GetGraveyard().PlaceTop(gameObject);
    }

}
