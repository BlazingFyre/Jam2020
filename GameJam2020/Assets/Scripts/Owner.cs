using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
    public Spirit owner;

    public virtual Spirit GetOwner()
    {
        return owner;
    }
    public virtual void SetOwner(Spirit owner)
    {
        this.owner = owner;
    }

}
