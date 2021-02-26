using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAligner : CardAligner
{

    public override void UpdateAlignment()
    {

        List<CardWhole> cards = GetComponent<CardContainer>().GetCards();

        if (cards.Count != 0)
        {
            // B: No cost (of code quality) too great.
            float cardWidth =
            cards[0].GetComponent<RectTransform>().rect.width
            * cards[0].GetComponent<RectTransform>().localScale.x;
            float handMiddle = gameObject.GetComponent<RectTransform>().position.x;
            float totalHeldCardsWidth = cards.Count * cardWidth;

            float currX = handMiddle - (totalHeldCardsWidth / 2) + (cardWidth / 2);

            foreach (CardWhole c in cards)
            {
                c.GetComponent<RectTransform>().position = new Vector3(currX, gameObject.GetComponent<RectTransform>().position.y);
                currX += cardWidth;
            }
        }

    }
}
