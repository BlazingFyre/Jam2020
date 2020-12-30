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
      //cardData.function = typeof(Function2);
      Type functionType = cardData.GetFunction();

      Debug.Log(functionType);

      //We then create an instance of that type:
      Function cardFunctionInstance = (Function) System.Activator.CreateInstance(functionType);
      //And then call the Play() function of cardFunctionInstance
      cardFunctionInstance.Play(target);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (globalVariables.GetSelectedObject() != null)
        {
            PlayCard(globalVariables.GetSelectedObject());
        }

        // Turn off "dragged" card highlight
        this.GetComponent<Select>().DisableTargetableHighlight();

        // Turn off highlights for potential targets
        globalVariables.SetPotentialTargets(null);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        // Turn on dragged card highlight
        this.GetComponent<Select>().EnableTargetableHighlight();

        // Highlight the potential targets within the scene using the Function's IsTargetable method
        Select[] selectors = FindObjectsOfType<Select>();
        
        Type functionType = cardData.GetFunction();
        Function cardFunctionInstance = (Function)System.Activator.CreateInstance(functionType);

        List<GameObject> targetables = new List<GameObject>();

        foreach (Select s in selectors)
        {
            if (cardFunctionInstance.IsTargetable(s.gameObject))
            {
                targetables.Add(s.gameObject);
            }
        }

        globalVariables.SetPotentialTargets(targetables);

    }

}
