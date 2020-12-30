using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GlobalVariables globalVariables;
    private bool highlightLock = false;
    private bool selected = false;
    private bool targetable = false;

    public Color selectedColor = new Color(0f, 0.75f, 1f);
    public Color targetableColor = new Color(1f, 0.5f, 0f);

    public GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
        SetColorSelected();
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        globalVariables.SetSelectedObject(this.gameObject);
        //Debug.Log(this.name + " moused over");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        globalVariables.SetSelectedObject(null);
        //Debug.Log(this.name + " no longer moused over");
    }

    public void EnableSelectedHighlight()
    {
        if (!highlightLock)
        {
            selected = true;
            UpdateHighlight();
        }
    }

    public void DisableSelectedHighlight()
    {
        if (!highlightLock)
        {
            selected = false;
            UpdateHighlight();
        }
    }

    public void EnableTargetableHighlight()
    {
        if (!highlightLock)
        {
            targetable = true;
            UpdateHighlight();
        }
    }

    public void DisableTargetableHighlight()
    {
        if (!highlightLock)
        {
            targetable = false;
            UpdateHighlight();
        }
    }

    private void SetColorSelected()
    {
        highlight.GetComponent<Image>().color = selectedColor;
    }

    private void SetColorTargetable()
    {
        highlight.GetComponent<Image>().color = targetableColor;
    }

    private void UpdateHighlight()
    {
        if (!selected && !targetable)
        {
            highlight.SetActive(false);
        }
        else
        {
            if (selected)
            {
                SetColorSelected();
            } 
            else if (targetable)
            {
                SetColorTargetable();
            }
            highlight.SetActive(true);
        }
    }

    public void EnableHighlightLock()
    {
        highlightLock = true;
    }

    public void DisableHighlightLock()
    {
        highlightLock = false;
        UpdateHighlight();
    }

}