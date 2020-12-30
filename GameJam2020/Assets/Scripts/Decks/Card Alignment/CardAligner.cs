using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAligner : MonoBehaviour
{
    public virtual void AlignCards(List<GameObject> cards)
    {
        foreach (GameObject c in cards)
        {
            c.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().position;
        }
    }
}
