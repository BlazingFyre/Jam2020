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

    // TODO: Shouldn't we make it so only cards in a hand be discarded? 
    // TODO: And if so, what should the use-case be if Discard() is called on a card elsewhere?
    public virtual void Discard()
    {
        // Place this into its controller's graveyard ...
        GetComponent<Half>().GetWhole().GetComponent<Use>().GetController().GetGrave().PlaceTop(
            // ... after drawing it from its respective container
            GetComponent<Half>().GetWhole().GetComponent<CardWhole>().GetCardContainer().DrawTarget(
                GetComponent<Half>().GetWhole().GetComponent<CardWhole>()
            )
        );
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
