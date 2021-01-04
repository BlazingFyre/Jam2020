using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GlobalVariables globalVariables;
    public bool highlightLock = false;
    public bool selected = false;
    public bool darkenedLock = false;
    public bool darkened = false;

    public Color selectedColor = new Color(0f, 0.75f, 1f);
    private Color originalSelectedColor;

    public Color litColor = new Color(1f, 1f, 1f);
    public Color darkenedColor = new Color(0.5f, 0.5f, 0.5f);

    public float selectUpscaling = 1.3f;

    public GameObject highlight;
    public GameObject front;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
        SetColorSelected();

        highlight = gameObject.transform.Find("Highlight").gameObject;
        front = gameObject.transform.Find("Front").gameObject;

        originalSelectedColor = selectedColor;
        litColor = front.GetComponent<Image>().color;
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

    public void EnableUpscaling()
    {
        gameObject.transform.parent.SetSiblingIndex(int.MaxValue);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(selectUpscaling, selectUpscaling, 1);
    }

    public void DisableUpscaling()
    {
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void EnableHighlight()
    {
        if (!highlightLock)
        {
            selected = true;
            if (!darkened)
            {
                UpdateHighlight();
            }
        }
    }

    public void DisableHighlight()
    {
        if (!highlightLock)
        {
            selected = false;
            if (!darkened)
            {
                UpdateHighlight();
            }
        }
    }

    private void SetColorSelected()
    {
        highlight.GetComponent<Image>().color = selectedColor;
    }

    private void UpdateHighlight()
    {
        highlight.SetActive(selected);
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

    public void SetHighlightColor(Color color)
    {
        selectedColor = color;
        SetColorSelected();
    }

    public void RevertHighlightColor()
    {
        selectedColor = originalSelectedColor;
        SetColorSelected();
    }

    public bool IsDarkened()
    {
        return darkened;
    }

    public void EnableDarkened()
    {
        if (!darkenedLock)
        {
            darkened = true;
            UpdateDarkened();
        }
    }
    public void DisableDarkened()
    {
        if (!darkenedLock)
        {
            darkened = false;
            UpdateDarkened();
        }
    }

    public void EnableDarkenedLock()
    {
        darkenedLock = true;
    }

    public void DisableDarkenedLock()
    {
        darkenedLock = false;
        UpdateDarkened();
    }

    private void UpdateDarkened()
    {
        if (darkened)
        {
            front.GetComponent<Image>().color = darkenedColor;
        }
        else
        {
            front.GetComponent<Image>().color = litColor;
            UpdateHighlight();
        }
    }
}