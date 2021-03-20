using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Sides;

public class DeckAligner : MonoBehaviour
{

    // Called at the start of a battle
    public void InitializeCardAlignment()
    {
        List<CardWhole> cards = GetComponent<CardContainer>().GetCards();
        CanvasManager battleCanvas = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<CanvasManager>();

        foreach (CardWhole c in cards)
        {
            // Place it under the hierarchy of the Cards object
            c.transform.SetParent(battleCanvas.GetCardsObject().transform, false);
        }
    }

    // Called every time a container loses/gains a card
    public virtual void UpdateAlignment()
    {
        List<CardWhole> cards = GetComponent<CardContainer>().GetCards();

        foreach (CardWhole c in cards)
        {
            // Make it uninteractable
            c.GetComponent<Whole>().GetSide(Side.A).GetComponent<Selectable>().interactable = false;
            c.GetComponent<Whole>().GetSide(Side.B).GetComponent<Selectable>().interactable = false;

            // Make it inactive
            c.gameObject.SetActive(false);

            // Move the card to the deck's position
            c.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        }
    }

}
