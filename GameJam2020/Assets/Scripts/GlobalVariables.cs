using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;

    public GameObject turnTaker;
    public GameObject turnButton;

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
        // Make sure the end turn button cannot be touched until next turn
        turnButton.GetComponent<Select>().SetSelectable(false);
        turnButton.GetComponent<Select>().SetHighlighted(false);
        turnButton.GetComponent<Select>().SetDarkened(true);
        turnButton.GetComponent<Select>().SetDarkenLock(true);

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

        // Re-enable the end turn button
        turnButton.GetComponent<Select>().SetDarkenLock(false);
        turnButton.GetComponent<Select>().SetDarkened(false);
        turnButton.GetComponent<Select>().SetSelectable(true);
    }

    public GameObject GetSelectedObject()
    {
        return this.selectedObject;
    }
    public void SetSelectedObject(GameObject selectedObject)
    {
        if (this.selectedObject != null)
        {
            this.selectedObject.GetComponent<Select>().SetHighlighted(false);

            // If the previously selected object was a card (and the player isn't targeting something with another card), downscale it
            if (this.selectedObject.GetComponent<CardSideControl>() != null && !isTargeting)
            {
                this.selectedObject.GetComponent<Select>().SetUpscaled(false);
            }
        }

        if (selectedObject != null)
        {
            selectedObject.GetComponent<Select>().SetHighlighted(true);

            // If the selected object is a card (and the player isn't targeting something with another card), upscale it
            if (selectedObject.GetComponent<CardSideControl>() != null && !isTargeting)
            {
                selectedObject.GetComponent<Select>().SetUpscaled(true);
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
            o.GetComponent<Select>().SetSelectable(false);
            o.GetComponent<Select>().SetDarkened(true);
        }
    }

    private void StopTargeting()
    {
        isTargeting = false;

        foreach (GameObject o in this.nonpotentialTargets)
        {
            if (o.GetComponent<CardSideControl>() == null)
            {
                o.GetComponent<Select>().SetSelectable(true);
                o.GetComponent<Select>().SetDarkened(false);
            }

            player.GetComponent<SpiritFunction>().GetHand().GetComponent<CardAligner>().UpdateAlignment();
        }
        
    }
}