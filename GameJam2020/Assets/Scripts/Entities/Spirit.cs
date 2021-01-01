using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : Entity
{

    public GameObject deck;
    public GameObject graveyard;
    public GameObject hand;

    public void Start()
    {
        deck.GetComponent<CardContainer>().SetOwner(this);
        graveyard.GetComponent<CardContainer>().SetOwner(this);
        hand.GetComponent<CardContainer>().SetOwner(this);
    }

    public GameObject GetDeck()
    {
        return deck;
    }

    public GameObject GetGraveyard()
    {
        return graveyard;
    }

    public GameObject GetHand()
    {
        return hand;
    }

}
