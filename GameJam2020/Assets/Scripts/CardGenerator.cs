using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    
    public CardWhole CopyOf(CardWhole card)
    {
        CardWhole newCard = card.DeepCopy();
        newCard.transform.SetParent(gameObject.transform);
        return newCard;
    }

}
