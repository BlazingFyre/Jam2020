using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GlobalVariables globalVariables;
    public GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        globalVariables.setSelectedObject(this.gameObject, highlight);
        //highlight.SetActive(true);
        Debug.Log(this.name + " moused over");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        globalVariables.setSelectedObject(null, null);
        //highlight.SetActive(false);
        Debug.Log(this.name + " no longer moused over");
    }

}