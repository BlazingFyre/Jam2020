using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
  private GlobalVariables globalVariables;
  public void Start()
  {
    globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
  }

    public CardController() { }

    void FixedUpdate()
    {

    }

    public void PlayTest(GameObject target)
    {
        print(target.name);
        // Grab function and Play() it
    }

    public void ToggleHighlight()
    {

    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(globalVariables.getSelectedObject().name);

    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {

    }

}
