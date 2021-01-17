using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWhole : MonoBehaviour
{

    public GameObject cardSideA;
    public GameObject cardSideB;

    public CardContainer container;

    public void InitCard(CardContainer container)
    {
        SetCardContainer(container);

        gameObject.AddComponent<Whole>();
        gameObject.GetComponent<Whole>().InitSides(cardSideA, cardSideB);

        gameObject.AddComponent<User>();
        gameObject.GetComponent<User>().InitOwner(container.GetComponent<User>().GetOwner());
    }

    public CardContainer GetCardContainer()
    {
        return container;
    }
    public void SetCardContainer(CardContainer container)
    {
        this.container = container;
    }

}