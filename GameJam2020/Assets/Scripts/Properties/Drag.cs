using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private BattleUI battleUI;

    public void Start()
    {
        battleUI = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<BattleUI>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (GetComponent<Selectable>().IsInteractable())
        {
            battleUI.DragObject(gameObject);
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (GetComponent<Selectable>().IsInteractable())
        {
            battleUI.UndragObject();
        }
    }

}
