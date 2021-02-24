using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;

public class CardWhole : MonoBehaviour
{

    public CardContainer container;

    public CardContainer GetCardContainer()
    {
        return container;
    }
    public void SetCardContainer(CardContainer container)
    {
        this.container = container;
    }

    public CardHalf GetActiveSide()
    {
        return GetComponent<Whole>().GetSide(
                GetComponent<Use>().GetController().GetSpirit(SleepState.Waking).GetComponent<Half>().GetSide()
            ).GetComponent<CardHalf>();
    }

    public CardWhole DeepCopy()
    {
        GameObject cardCopy = Instantiate(gameObject);
        cardCopy.transform.SetParent(gameObject.transform.parent);
        return cardCopy.GetComponent<CardWhole>();
    }

}