using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Select : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private BattleUI battleUI;

    public void Start()
    {
        battleUI = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<BattleUI>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (GetComponent<Selectable>().IsInteractable())
        {
            battleUI.SelectObject(gameObject);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (GetComponent<Selectable>().IsInteractable())
        {
            battleUI.UnselectObject(gameObject);
        }
    }

}
