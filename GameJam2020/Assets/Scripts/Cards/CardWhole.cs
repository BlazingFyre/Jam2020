using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWhole : MonoBehaviour
{

    public CardContainer container;

    public CardContainer GetCardContainer()
    {
        return container;
    }
    public void SetCardContainer(CardContainer container)
    {
        this.container = container;
    }

    public CardWhole DeepCopy()
    {
        GameObject cardCopy = Instantiate(gameObject);
        cardCopy.transform.SetParent(gameObject.transform.parent);
        return cardCopy.GetComponent<CardWhole>();
    }

}