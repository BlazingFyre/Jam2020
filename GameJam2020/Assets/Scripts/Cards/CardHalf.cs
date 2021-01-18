using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHalf : MonoBehaviour
{

    public string halfName;
    public string halfText;
    public Sprite halfArt;

    public virtual bool IsTargetable(GameObject target)
    {
        return false;
    }

    public virtual void Play(GameObject target)
    {
        Discard();
    }

    public virtual void Discard()
    {
        
    }

    public virtual string GetHalfName()
    {
        return halfName;
    }

    public virtual string GetHalfText()
    {
        return halfText;
    }

    public virtual Sprite GetHalfArt()
    {
        return halfArt;
    }

}
