using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;

    public GameObject turnTaker;

    public bool isTargeting = false;

    // The amount of seconds to wait between each time a card is drawn
    public float cardDrawDelay = 0.1f;

    public GameObject selectedObject = null;
    public List<GameObject> potentialTargets = null;
    public List<GameObject> nonpotentialTargets = null;

    public void Start()
    {
        turnTaker = player;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
    public GameObject GetEnemy()
    {
        return enemy;
    }
    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
    }

    public void EndTurn()
    {
        //TODO: end of turn effects

        if (turnTaker == player)
        {
            turnTaker = enemy;
        }
        else if (turnTaker == enemy)
        {
            turnTaker = player;
        }

        //TODO: some sort of visual

        StartTurn();

    }

    public void StartTurn()
    {

        SpiritFunction turnSpirit = turnTaker.GetComponent<SpiritFunction>();

        //TODO: start of turn effects
        //TODO: turnTaker: refresh mana

        // Refresh the Spirit's hand
        StartCoroutine(HandRefresh(turnSpirit));

        if (turnTaker == enemy)
        {
            //TODO: enemy.act
        }
    }

    private IEnumerator HandRefresh(SpiritFunction turnSpirit)
    {
        // Discard the cards in the Spirit's hand
        while (!turnSpirit.GetHand().IsEmpty())
        {
            yield return new WaitForSeconds(cardDrawDelay);

            GameObject drawnCard = turnSpirit.GetHand().DrawTop();
            turnSpirit.GetGraveyard().PlaceBottom(drawnCard);
        }

        // For however many cards the Spirit draws,
        for (int i = 0; i < turnSpirit.GetTurnStartCards(); i++)
        {
            yield return new WaitForSeconds(cardDrawDelay);

            // If at any point the deck is empty, attempt to shuffle the graveyard into the deck
            if (turnSpirit.GetDeck().IsEmpty())
            {
                // If both decks are empty, stop drawing
                if (turnSpirit.GetGraveyard().IsEmpty())
                {
                    break;
                }
                else
                {
                    while (!turnSpirit.GetGraveyard().IsEmpty())
                    {
                        yield return new WaitForSeconds(cardDrawDelay / 2);

                        GameObject currCard = turnSpirit.GetGraveyard().DrawBottom();
                        turnSpirit.GetDeck().PlaceBottom(currCard);
                    }
                    turnSpirit.GetDeck().Shuffle();
                }
            }

            // Place the card into the Spirit's hand
            GameObject drawnCard = turnSpirit.GetDeck().DrawTop();
            turnSpirit.GetHand().PlaceBottom(drawnCard);
        }
    }

    public GameObject GetSelectedObject()
    {
        return this.selectedObject;
    }
    public void SetSelectedObject(GameObject selectedObject)
    {
        if (this.selectedObject != null)
        {
            this.selectedObject.GetComponent<Select>().DisableHighlight();
            
            if (this.selectedObject.tag == "Card Side")
            {
                this.selectedObject.GetComponent<Select>().DisableUpscaling();
            }
        }

        if (selectedObject != null)
        {
            selectedObject.GetComponent<Select>().EnableHighlight();

            if (selectedObject.tag == "Card Side")
            {
                selectedObject.GetComponent<Select>().EnableUpscaling();
            }
        }

        this.selectedObject = selectedObject;
    }

    public void SetPotentialTargets(List<GameObject> potentialTargets, List<GameObject> nonpotentialTargets)
    {
        if (this.nonpotentialTargets != null)
        {
            StopTargeting();
        }
        if (nonpotentialTargets != null)
        {
            StartTargeting(potentialTargets, nonpotentialTargets);
        }

        this.potentialTargets = potentialTargets;
        this.nonpotentialTargets = nonpotentialTargets;
    }

    public bool IsTargeting()
    {
        return isTargeting;
    }

    private void StartTargeting(List<GameObject> potentialTargets, List<GameObject> nonpotentialTargets)
    {
        isTargeting = true;

        foreach (GameObject o in nonpotentialTargets)
        {
            o.GetComponent<Select>().EnableDarkened();
        }
    }

    private void StopTargeting()
    {
        isTargeting = false;

        // Relight all non-card selectable objects (the cards are handled by CardAligner)
        foreach (GameObject o in this.nonpotentialTargets)
        {
            if (o.GetComponent<CardSideControl>() == null)
            {
                o.GetComponent<Select>().DisableDarkened();
            }
        }
        
    }
}