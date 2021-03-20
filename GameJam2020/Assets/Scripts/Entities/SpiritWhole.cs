using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static SleepStates;

public class SpiritWhole : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public CardContainer deck;
    public CardContainer grave;
    // --------------------------------------------------------------------------------------------

    // The hand for a spirit. Initialized at the start of a battle
    public CardContainer hand;

    // The number of cards drawn at the start of this spirit's turn
    public int turnStartCards = 4;

    private ActionLog actionLog;

    public void Start()
    {
        actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
    }

    public CardContainer GetDeck()
    {
        return deck;
    }

    public CardContainer GetGrave()
    {
        return grave;
    }

    public CardContainer GetHand()
    {
        return hand;
    }

    public void SetHand(CardContainer hand)
    {
        this.hand = hand;
    }

    public SpiritHalf GetSpirit(Side side)
    {
        return GetComponent<Whole>().GetSide(side).GetComponent<SpiritHalf>();
    }

    public SpiritHalf GetSpirit(SleepState state)
    {
        if (GetComponent<Whole>().GetSide(Side.A).GetComponent<SpiritHalf>().GetSleepState() == state)
        {
            return GetComponent<Whole>().GetSide(Side.A).GetComponent<SpiritHalf>();
        }
        else
        {
            return GetComponent<Whole>().GetSide(Side.B).GetComponent<SpiritHalf>();
        }
    }

    public int GetTurnStartCards()
    {
        return turnStartCards;
    }

}