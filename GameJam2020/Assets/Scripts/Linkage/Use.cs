using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use : MonoBehaviour
{
    // The original owner of this object. Initialized at the start of a battle
    public SpiritWhole owner = null;
    // The current controller of this object. Initialized at the start of a battle
    public SpiritWhole controller = null;

    public SpiritWhole GetOwner()
    {
        return owner;
    }

    public void InitOwner(SpiritWhole owner)
    {
        this.owner = owner;
        this.controller = owner;
    }

    public SpiritWhole GetController()
    {
        return controller;
    }

    public void SetController(SpiritWhole controller)
    {
        this.controller = controller;
    }

}
