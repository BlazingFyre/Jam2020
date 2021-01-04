using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFunction : MonoBehaviour
{

    public GameObject deck;
    public GameObject graveyard;
    public GameObject hand;

    public GameObject spiritA;
    public GameObject spiritB;

    public int turnStartCards = 4;

    public void Start()
    {
        deck.GetComponent<CardContainer>().SetOwner(this);
        graveyard.GetComponent<CardContainer>().SetOwner(this);
        hand.GetComponent<CardContainer>().SetOwner(this);

        deck.GetComponent<CardContainer>().Start();
        graveyard.GetComponent<CardContainer>().Start();
        hand.GetComponent<CardContainer>().Start();

        spiritA = gameObject.transform.Find("Side A").gameObject;
        spiritB = gameObject.transform.Find("Side B").gameObject;

        spiritA.GetComponent<SpiritSideControl>().SetFlip(spiritB.GetComponent<SpiritSideControl>());
        spiritB.GetComponent<SpiritSideControl>().SetFlip(spiritA.GetComponent<SpiritSideControl>());
    }

    public CardContainer GetDeck()
    {
        return deck.GetComponent<CardContainer>();
    }

    public CardContainer GetGraveyard()
    {
        return graveyard.GetComponent<CardContainer>();
    }

    public CardContainer GetHand()
    {
        return hand.GetComponent<CardContainer>();
    }

    public int GetTurnStartCards()
    {
        return turnStartCards;
    }

    public SpiritSideControl GetWakingSpirit()
    {
        if (spiritA.GetComponent<SpiritSideControl>().IsAwake())
        {
            return spiritA.GetComponent<SpiritSideControl>();
        }
        else
        {
            return spiritB.GetComponent<SpiritSideControl>();
        }
    }

    public SpiritSideControl GetDreamingSpirit()
    {
        if (!spiritA.GetComponent<SpiritSideControl>().IsAwake())
        {
            return spiritA.GetComponent<SpiritSideControl>();
        }
        else
        {
            return spiritB.GetComponent<SpiritSideControl>();
        }
    }

}
