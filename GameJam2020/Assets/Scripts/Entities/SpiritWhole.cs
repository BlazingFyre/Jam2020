using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static SleepStates;

public class SpiritWhole : MonoBehaviour
{

    public CardContainer baseDeck;
    public CardContainer deck;
    public CardContainer grave;
    public CardContainer hand;

    public int turnStartCards = 4;
    public int turnStartMana = 4;
    public int mana = 4;

    private ActionLog actionLog;

    public void Start()
    {
        actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
    }

    public void InitContainers(CardContainer deck, CardContainer grave, CardContainer hand)
    {
        this.deck = deck;
        this.grave = grave;
        this.hand = hand;

        deck.GetComponent<Use>().InitOwner(this);
        grave.GetComponent<Use>().InitOwner(this);
        hand.GetComponent<Use>().InitOwner(this);
    }

    private void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>().Enter(new ActionLog.Draw(
                this,
                0,
                deck,
                0,
                hand
                ));
        }
    }

    public void RefreshHand()
    {
        DrawCards(turnStartCards);
    }

    public void DiscardHand()
    {
        if (!hand.IsEmpty())
        {
            for (int j = hand.GetCards().Count - 1; j > -1; j--)
            {
                hand.GetCards()[j].GetComponent<Whole>().GetSide(SleepState.Waking).GetComponent<CardHalf>().Discard();
            }
        }
    }

    public CardContainer GetBaseDeck()
    {
        return baseDeck;
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

    public void AddMana(int amount)
    {
        mana = mana + amount;
    }

    public void RefreshMana()
    {
        mana = turnStartMana;
    }

}