using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandAligner : CardAligner
{
    public override void AlignCards(List<GameObject> cards)
    {
        float cardWidth = cards[0].GetComponent<RectTransform>().rect.width * cards[0].GetComponent<RectTransform>().localScale.x;
        float handMiddle = gameObject.GetComponent<RectTransform>().position.x;
        float totalHeldCardsWidth = cards.Count() * cardWidth;

        float currX = handMiddle - (totalHeldCardsWidth / 2) + (cardWidth / 2);

        foreach (GameObject c in cards)
        {
            c.GetComponent<RectTransform>().position = new Vector3(currX, gameObject.GetComponent<RectTransform>().position.y);
            currX += cardWidth;
        }
    }
}
