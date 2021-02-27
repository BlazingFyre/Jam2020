using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Sides;

public class CardAligner : MonoBehaviour
{

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
