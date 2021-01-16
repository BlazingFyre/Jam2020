using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
  public GameObject cardPrefab;
  private CardLibrary cardLibrary;

  //We use Awake instead of Start here because the code in GlobalVariables has to run before certain code in Start() for other classes.
  void Awake()
  {
    TextAsset cardDataJSON = Resources.Load<TextAsset>("Data/CardData");
    cardLibrary = JsonUtility.FromJson<CardLibrary>(cardDataJSON.text);
  }

  public CardData GetCardData(string cardName)
  {
    foreach(CardData cardData in cardLibrary.allCards)
    {
      if(cardData.cardName == cardName)
      {
        return cardData;
      }
    }
    return null;
  }
}

class CardLibrary
{
  public CardData[] allCards;
}
