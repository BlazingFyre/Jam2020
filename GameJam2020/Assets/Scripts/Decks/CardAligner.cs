using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAligner : MonoBehaviour
{

    protected GlobalVariables globalVariables;
    public CardContainer container;

    public void Start()
    {
        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
        container = gameObject.GetComponent<CardContainer>();
    }

    public void UpdateAlignment()
    {
        if (!container.IsEmpty())
        {
            AlignCards();
        }
    }

    public virtual void AlignCards()
    {
        foreach (GameObject c in container.GetCards())
        {
            c.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().position;
            c.SetActive(false);
        }
    }

}
