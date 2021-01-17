using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static SleepStates;

public class SpiritWhole : MonoBehaviour
{

    public CardContainer deck;
    public CardContainer graveyard;
    public CardContainer hand;

    public int turnStartCards = 4;

    public void InitDecks()
    {
        deck.InitContainer(this);
        graveyard.InitContainer(this);
        hand.InitContainer(this);
    }

    public CardContainer GetDeck()
    {
        return deck;
    }

    public CardContainer GetGraveyard()
    {
        return graveyard;
    }

    public CardContainer GetHand()
    {
        return hand;
    }

    public int GetTurnStartCards()
    {
        return turnStartCards;
    }

    public Spirit GetSpirit(Side side)
    {
        return GetComponent<Whole>().GetSide(side).GetComponent<Spirit>();
    }

    public Spirit GetSpirit(SleepState state)
    {
        if (GetComponent<Whole>().GetSide(Side.A).GetComponent<Spirit>().GetSleepState() == state)
        {
            return GetComponent<Whole>().GetSide(Side.A).GetComponent<Spirit>();
        }
        else
        {
            return GetComponent<Whole>().GetSide(Side.B).GetComponent<Spirit>();
        }
    }

}