using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Phases;

public class BattleUI : MonoBehaviour
{

    // Trackers for what objects are currently being selected/dragged
    public GameObject draggedObject = null;
    public GameObject selectedObject = null;

    // Tracker for whether or not player input via interactables is enabld
    public bool playerInteraction = true;

    // A list of potential targets, collected to highlight them upon initiating a card drag
    public List<GameObject> targetables = new List<GameObject>();

    public void DragObject(GameObject o)
    {
        // If the player is dragging a card, highlight the viable targets
        if (o.GetComponent<CardHalf>() != null)
        {
            foreach (Select s in FindObjectsOfType<Select>())
            {
                if (o.GetComponent<CardHalf>().IsTargetable(s.gameObject) && 
                    s.GetComponent<Selectable>() != null && 
                    s.GetComponent<Selectable>().IsInteractable())
                {
                    targetables.Add(s.gameObject);
                    s.EnableTargetHighlight();
                }
            }
        }

        draggedObject = o;
    }

    public void UndragObject()
    {
        // If the player has dragged the card to a viable target, reset highlights and play it
        if (draggedObject.GetComponent<CardHalf>() != null)
        {
            foreach (GameObject o in targetables)
            {
                o.GetComponent<Select>().DisableTargetHighlight();
            }
            targetables.Clear();

            if (IsSelecting() && draggedObject.GetComponent<CardHalf>().IsTargetable(selectedObject))
            {
                draggedObject.GetComponent<CardHalf>().Play(selectedObject);
            }
        }
        
        draggedObject = null;
    }

    public bool IsDragging()
    {
        return draggedObject != null;
    }

    public void SelectObject(GameObject o)
    {
        selectedObject = o;
    }

    public void UnselectObject(GameObject o)
    {
        if (selectedObject == o)
        {
            selectedObject = null;
        }
    }

    public bool IsSelecting()
    {
        return selectedObject != null;
    }

    public void EndTurnButton()
    {
        SetPlayerInteraction(false);

        GetComponent<ActionLog>().Enter(new ActionLog.PhaseChange(
            GetComponent<BattleSystem>().GetTurnSpirit(),
            Phase.End
            ));
    }

    public void SetPlayerInteraction(bool playerInteraction)
    {
        this.playerInteraction = playerInteraction;

        // Update end turn button
        GetComponent<UIManager>().getEndTurnButton().interactable = playerInteraction;

        // Update decks
        GetComponent<BattleSystem>().GetPlayer().GetDeck().GetComponent<Selectable>().interactable = playerInteraction;
        GetComponent<BattleSystem>().GetPlayer().GetBin().GetComponent<Selectable>().interactable = playerInteraction;
        GetComponent<BattleSystem>().GetEnemy().GetDeck().GetComponent<Selectable>().interactable = playerInteraction;
        GetComponent<BattleSystem>().GetEnemy().GetBin().GetComponent<Selectable>().interactable = playerInteraction;

        // Update cards
        GetComponent<BattleSystem>().GetPlayer().GetHand().GetComponent<DeckAligner>().SetCardInteractability(playerInteraction);
        GetComponent<BattleSystem>().GetEnemy().GetHand().GetComponent<DeckAligner>().SetCardInteractability(playerInteraction);
    }

    public bool IsPlayerInteractionEnabled()
    {
        return playerInteraction;
    }

}
