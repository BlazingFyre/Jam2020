using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{

    public SpiritWhole owner = null;
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
