using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandAligner : CardAligner
{
    public override void AlignCards(List<GameObject> cards)
    {
        float cardWidth = cards[0].GetComponent<RectTransform>().rect.width;
        float currX = gameObject.GetComponent<RectTransform>().position.x
            - ((cards.Count() * cardWidth) / 2)
            + (cardWidth);

        foreach (GameObject c in cards)
        {
            c.GetComponent<RectTransform>().position = new Vector3(currX, gameObject.GetComponent<RectTransform>().position.y);
            currX += cardWidth / 2;
        }
    }
}
