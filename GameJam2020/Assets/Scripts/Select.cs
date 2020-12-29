using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GlobalVariables globalVariables;
    private bool highlightLock = false;

    public Color selectedColor = new Color(0f, 0.75f, 1f);
    public Color lockedColor = new Color(1f, 0.5f, 0f);

    public GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
        highlight.GetComponent<Image>().color = selectedColor;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        globalVariables.setSelectedObject(this.gameObject);
        //Debug.Log(this.name + " moused over");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        globalVariables.setSelectedObject(null);
        //Debug.Log(this.name + " no longer moused over");
    }

    public void EnableHighlight()
    {
        if (!highlightLock)
        {
            highlight.SetActive(true);
        }
    }

    public void DisableHighlight()
    {
        if (!highlightLock)
        {
            highlight.SetActive(false);
        }
    }

    public void EnableHighlightLock()
    {
        highlightLock = true;
        highlight.GetComponent<Image>().color = lockedColor;
    }

    public void DisableHighlightLock()
    {
        highlightLock = false;
        highlight.GetComponent<Image>().color = selectedColor;
    }

    public void UpdateHighlight()
    {
        if (!highlightLock)
        {
            if (globalVariables.getSelectedObject() != this)
            {
                highlight.SetActive(false);
            }
            else
            {
                highlight.SetActive(true);
            }
        }
        
    }

}