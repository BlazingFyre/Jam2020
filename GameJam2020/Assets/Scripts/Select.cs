using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public bool highlightLock = false;
    public bool highlighted = false;
    public bool darkenLock = false;
    public bool darkened = false;
    public bool upscaled = false;
    public bool selectable = true;

    public Color selectedColor = new Color(0f, 0.75f, 1f);
    public Color litColor = new Color(1f, 1f, 1f);
    public Color darkenedColor = new Color(0.5f, 0.5f, 0.5f);
    public float selectUpscaling = 1.2f;

    private GlobalVariables globalVariables;
    public GameObject highlight;
    public GameObject front;

    // Start is called before the first frame update
    void Start()
    {
        // Temporary
        selectUpscaling = 1.0f;

        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();

        highlight = gameObject.transform.Find("Highlight").gameObject;
        front = gameObject.transform.Find("Front").gameObject;

        highlight.GetComponent<Image>().color = selectedColor;
        front.GetComponent<Image>().color = litColor;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (selectable)
        {
            globalVariables.SetSelectedObject(this.gameObject);

            front.GetComponent<Image>().color = litColor;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (selectable)
        {
            globalVariables.SetSelectedObject(null);

            if (darkened)
            {
                front.GetComponent<Image>().color = darkenedColor;
            }
            else
            {
                front.GetComponent<Image>().color = litColor;
            }
        }
    }

    public bool IsSelectable()
    {
        return selectable;
    }

    public void SetSelectable(bool selectable)
    {
        this.selectable = selectable;
    }

    public bool IsUpscaled()
    {
        return upscaled;
    }

    public void SetUpscaled(bool upscaled)
    {
        this.upscaled = upscaled;

        if (upscaled)
        {
            gameObject.transform.parent.SetSiblingIndex(int.MaxValue);
            gameObject.GetComponent<RectTransform>().localScale = new Vector3(selectUpscaling, selectUpscaling, 1);
        }
        else
        {
            gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }

    public bool IsHighlighted()
    {
        return highlighted;
    }

    public void SetHighlighted(bool highlighted)
    {
        if (!highlightLock)
        {
            this.highlighted = highlighted;
            highlight.SetActive(highlighted);
        }
    }

    public void SetHighlightColor(Color color)
    {
        selectedColor = color;
        highlight.GetComponent<Image>().color = selectedColor;
    }

    public bool IsHighlightLocked()
    {
        return highlightLock;
    }

    public void SetHighlightLock(bool highlightLock)
    {
        this.highlightLock = highlightLock;
    }

    public bool IsDarkened()
    {
        return darkened;
    }

    public void SetDarkened(bool darkened)
    {
        if (!darkenLock)
        {
            this.darkened = darkened;
            if (darkened)
            {
                front.GetComponent<Image>().color = darkenedColor;
            }
            else
            {
                front.GetComponent<Image>().color = litColor;
            }
        }
    }

    public bool IsDarkenLocked()
    {
        return darkenLock;
    }

    public void SetDarkenLock(bool darkenLock)
    {
        this.darkenLock = darkenLock;
    }

}