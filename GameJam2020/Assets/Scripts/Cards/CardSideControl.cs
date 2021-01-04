using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSideControl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public char side;
    public CardFunction cardFunction;
    private GlobalVariables globalVariables;

    public void Start()
    {
        cardFunction = gameObject.GetComponentInParent<CardFunction>();
        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!gameObject.GetComponent<Select>().IsDarkened())
        {

            // Turn on dragged card highlight / keep card from darkening
            this.GetComponent<Select>().EnableHighlightLock();
            this.GetComponent<Select>().EnableHighlight();
            this.GetComponent<Select>().EnableDarkenedLock();

            // Highlight the potential targets within the scene using the Function's IsTargetable method
            Select[] selectors = FindObjectsOfType<Select>();
            List<GameObject> targetables = new List<GameObject>();
            List<GameObject> nontargetables = new List<GameObject>();

            if (side == 'A')
            {
                foreach (Select s in selectors)
                {
                    if (cardFunction.IsTargetableA(s.gameObject))
                    {
                        targetables.Add(s.gameObject);
                    }
                    else
                    {
                        nontargetables.Add(s.gameObject);
                    }
                }
            }
            else if (side == 'B')
            {
                foreach (Select s in selectors)
                {
                    if (cardFunction.IsTargetableB(s.gameObject))
                    {
                        targetables.Add(s.gameObject);
                    }
                    else
                    {
                        nontargetables.Add(s.gameObject);
                    }
                }
            }

            globalVariables.SetPotentialTargets(targetables, nontargetables);

        }
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!gameObject.GetComponent<Select>().IsDarkened())
        {
            // Play the card side if the player selected a potential target
            GameObject selectedObject = globalVariables.GetSelectedObject();

            if (selectedObject != null)
            {
                if (side == 'A')
                {
                    if (cardFunction.IsTargetableA(selectedObject))
                    {
                        cardFunction.PlayA(globalVariables.GetSelectedObject());
                    }
                }
                else if (side == 'B')
                {
                    if (cardFunction.IsTargetableB(selectedObject))
                    {
                        cardFunction.PlayB(globalVariables.GetSelectedObject());
                    }
                }
            }

            // Turn off "dragged" card highlight
            this.GetComponent<Select>().DisableHighlightLock();
            this.GetComponent<Select>().DisableHighlight();
            this.GetComponent<Select>().DisableDarkenedLock();

            // Turn off highlights for potential targets
            globalVariables.SetPotentialTargets(null, null);
        }
    }

}
