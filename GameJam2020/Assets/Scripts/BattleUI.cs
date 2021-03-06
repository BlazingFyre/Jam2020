using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;

public class BattleUI : MonoBehaviour
{
    public bool dragging = false;
    public GameObject draggedObject = null;

    public bool selecting = false;
    public GameObject selectedObject = null;

    public void DragObject(GameObject o)
    {
        dragging = true;
        draggedObject = o;
    }

    public void UndragObject()
    {
        dragging = false;
        draggedObject = null;
    }

    public bool IsDragging()
    {
        return dragging;
    }

    public void SelectObject(GameObject o)
    {
        selecting = true;
        selectedObject = o;
    }

    public void UnselectObject(GameObject o)
    {
        if (selectedObject == o)
        {
            selecting = false;
            selectedObject = null;
        }
    }

    public bool IsSelecting()
    {
        return selecting;
    }

    public void EndTurnButton()
    {
        GetComponent<ActionLog>().Enter(new ActionLog.PhaseChange(
            GetComponent<BattleSystem>().GetTurnSpirit(),
            Phase.End
            ));
    }

}
