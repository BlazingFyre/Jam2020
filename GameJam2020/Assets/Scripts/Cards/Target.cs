using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Target : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
  private GlobalVariables globalVariables;
  public void Start()
  {
    globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
  }
  public void OnPointerUp(PointerEventData pointerEventData)
  {

  }

  public void OnPointerDown(PointerEventData pointerEventData)
  {
  }
public void OnPointerEnter(PointerEventData pointerEventData)
{
    globalVariables.setSelectedObject(this);
    Debug.Log(this.name + "moused over");
}

//Detect when Cursor leaves the GameObject
public void OnPointerExit(PointerEventData pointerEventData)
{
    globalVariables.setSelectedObject(null);
    Debug.Log(this.name + "no longer moused over");
}
}
