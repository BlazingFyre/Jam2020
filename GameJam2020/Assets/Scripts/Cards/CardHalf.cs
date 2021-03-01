using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHalf : MonoBehaviour
{

    public virtual string HalfName { get; set; } = "";
    public virtual string HalfText { get; set; } = "";
    public virtual Sprite HalfArt { get; set; } = null;

    public ActionLog.Action discardAction; // TODO: = default

    public virtual bool IsTargetable(GameObject target)
    {
        return false;
    }

    public virtual void CardFunction(GameObject target)
    {
        
    }

    public void Play(GameObject target)
    {
        // Log "Play" action
        CardFunction(target);
        // Log respective Discarding action
        // Temporary:
        Discard();
    }

    

    // TODO: Shouldn't we make it so only cards in a hand be discarded? 
    // TODO: And if so, what should the use-case be if Discard() is called on a card elsewhere?
    public void Discard()
    {
        // Place this into its controller's graveyard ...
        GetComponent<Half>().GetWhole().GetComponent<Use>().GetController().GetGrave().PlaceTop(
            // ... after drawing it from its respective container
            GetComponent<Half>().GetWhole().GetComponent<CardWhole>().GetCardContainer().DrawTarget(
                GetComponent<Half>().GetWhole().GetComponent<CardWhole>()
            )
        );
    }

}
