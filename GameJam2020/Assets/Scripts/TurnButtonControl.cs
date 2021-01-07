using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnButtonControl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    private GlobalVariables globalVariables;

    public bool pressed = false;
    public Color pressedColor;

    public void Start()
    {
        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        pressed = true;
        gameObject.GetComponent<Select>().SetDarkened(true);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (globalVariables.GetSelectedObject() == gameObject && pressed)
        {
            globalVariables.EndTurn();
        }

        pressed = false;
        gameObject.GetComponent<Select>().SetDarkened(false);
    }

}
