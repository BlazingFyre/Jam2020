using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAligner : CardAligner
{

    public override void UpdateAlignment()
    {
        CardContainer cardContainer = GetComponent<CardContainer>();

        float cardWidth = 
            cardContainer.GetCards()[0].GetComponent<RectTransform>().rect.width 
            * cardContainer.GetCards()[0].GetComponent<RectTransform>().localScale.x;
        float handMiddle = gameObject.GetComponent<RectTransform>().position.x;
        float totalHeldCardsWidth = cardContainer.GetCards().Count * cardWidth;

        float currX = handMiddle - (totalHeldCardsWidth / 2) + (cardWidth / 2);

        foreach (CardWhole c in cardContainer.GetCards())
        {
            c.GetComponent<RectTransform>().position = new Vector3(currX, gameObject.GetComponent<RectTransform>().position.y);
            currX += cardWidth;
        }

    }
}
