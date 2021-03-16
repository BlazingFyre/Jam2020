using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SleepStates;

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

    public Half GetSide(SleepState state)
    {
        Debug.Log(GetComponent<Whole>());
        Debug.Log(GetComponent<Use>());
        Debug.Log(GetComponent<Use>().GetController());
        Debug.Log(GetComponent<Use>().GetController().GetSpirit(state));

        return GetComponent<Whole>().GetSide(GetComponent<Use>().GetController().GetSpirit(state).GetComponent<Half>().GetSide());
    }

    public CardWhole DeepCopy()
    {
        GameObject cardCopy = Instantiate(gameObject);
        cardCopy.transform.SetParent(gameObject.transform.parent);
        return cardCopy.GetComponent<CardWhole>();
    }

}