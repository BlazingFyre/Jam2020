using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
  private GlobalVariables globalVariables;
  private CardData cardData;
  public void Start()
  {
    globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
    cardData = gameObject.GetComponent<CardData>();
  }

    public CardController() { }

    void FixedUpdate()
    {

    }

    public void PlayCard(GameObject target)
    {
      //The following section is dedicated to calling the function that should be called upon playing this card, stored in CardData.
      //We need to get the Type of the function:
      cardData.function = typeof(Function1);
      Type functionType = cardData.getFunction();

      Debug.Log(functionType);

      //We then create an instance of that type:
      Function cardFunctionInstance = (Function) System.Activator.CreateInstance(functionType);
      //And then call the Play() function of cardFunctionInstance
      cardFunctionInstance.Play(target);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (globalVariables.getSelectedObject() != null)
        {
            PlayCard(globalVariables.getSelectedObject());
        }

        this.GetComponent<Select>().DisableHighlightLock();
        this.GetComponent<Select>().DisableHighlight();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        this.GetComponent<Select>().EnableHighlightLock();
    }

}
