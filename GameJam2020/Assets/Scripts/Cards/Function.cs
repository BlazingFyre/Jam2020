using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function : MonoBehaviour
{
    public virtual void Play(GameObject cardSide, GameObject target)
    {
        // Do nothing.
        Discard(cardSide.transform.parent.gameObject);
    }

    public virtual void Discard(GameObject card)
    {
        Spirit owner = card.GetComponent<Owner>().GetOwner();

        // wtf
        if (owner.GetHand().GetComponent<CardContainer>().Contains(card))
        {
            owner.GetHand().GetComponent<CardContainer>().DrawTarget(card);
        } 
        else if (owner.GetDeck().GetComponent<CardContainer>().Contains(card))
        {
            owner.GetDeck().GetComponent<CardContainer>().DrawTarget(card);
        }

        owner.GetGraveyard().GetComponent<CardContainer>().PlaceTop(card);
    }

    public virtual bool IsTargetable(GameObject target)
    {
        return false;
    }

}
