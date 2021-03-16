using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Sides;
using static SleepStates;

public class HandAligner : CardAligner
{

    public override void UpdateAlignment()
    {
        List<CardWhole> cards = GetComponent<CardContainer>().GetCards();

        if (cards.Count != 0)
        {
            // B: No cost (of code readability) too great. (Fan the cards into a "hand.")
            float cardWidth =
            cards[0].GetComponent<RectTransform>().rect.width
            * cards[0].GetComponent<RectTransform>().localScale.x;
            float handMiddle = gameObject.GetComponent<RectTransform>().position.x;
            float totalHeldCardsWidth = cards.Count * cardWidth;

            float currX = handMiddle - (totalHeldCardsWidth / 2) + (cardWidth / 2);

            foreach (CardWhole c in cards)
            {
                c.GetComponent<RectTransform>().position = new Vector3(currX, gameObject.GetComponent<RectTransform>().position.y, 0);
                currX += cardWidth;
            }

            // Card state changes
            foreach (CardWhole c in cards)
            {
                // Make the waking half interactable, make the dreaming half uninteractable
                c.GetSide(SleepState.Waking).GetComponent<Selectable>().interactable = true;
                c.GetSide(SleepState.Dreaming).GetComponent<Selectable>().interactable = false;

                // Make it active
                c.gameObject.SetActive(true);
            }

        }

    }
}
