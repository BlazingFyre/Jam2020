using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;

public class CardWhole : MonoBehaviour
{

    // The container holding the card(s). Initialized when each deck boots up
    public CardContainer container;
    // The maximum durability of the card
    public int maxDurability = 4;
    // The durability of the card
    public int durability = 4;

    public CardContainer GetCardContainer()
    {
        return container;
    }
    public void SetCardContainer(CardContainer container)
    {
        this.container = container;
    }

    public int GetDurability()
    {
        return durability;
    }

    public void SetDurability(int durability)
    {
        // Clamp the durability between 0 and the maximum durability
        this.durability = Math.Min(Math.Max(0, durability), maxDurability);

        // Show the new durability
        GetSide(SleepState.Waking).GetComponent<CardDisplay>().UpdateDisplay();
        GetSide(SleepState.Dreaming).GetComponent<CardDisplay>().UpdateDisplay();
    }

    public Half GetSide(SleepState state)
    {
        return GetComponent<Whole>().GetSide(GetComponent<Use>().GetController().GetSpirit(state).GetComponent<Half>().GetSide());
    }

    public CardWhole DeepCopy()
    {
        GameObject cardCopy = Instantiate(gameObject);
        cardCopy.transform.SetParent(gameObject.transform.parent);
        return cardCopy.GetComponent<CardWhole>();
    }

}