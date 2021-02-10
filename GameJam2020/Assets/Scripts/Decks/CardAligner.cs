using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAligner : MonoBehaviour
{

    public virtual void UpdateAlignment()
    {
        CardContainer cardContainer = GetComponent<CardContainer>();

        foreach (CardWhole c in cardContainer.GetCards())
        {
            c.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().position;
        }

    }

}
