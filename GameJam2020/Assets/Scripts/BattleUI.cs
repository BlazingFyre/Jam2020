using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;

public class BattleUI : MonoBehaviour
{
    public bool dragging = false;
    public CardWhole draggedCard = null;

    public void DragCard(CardWhole card)
    {
        dragging = true;
        draggedCard = card;
    }

    public void UndragCard()
    {
        dragging = false;
        draggedCard = null;
    }

    public bool IsDragging()
    {
        return dragging;
    }


    public void EndTurnButton()
    {
        GetComponent<ActionLog>().Enter(new ActionLog.PhaseChange(
            GetComponent<BattleSystem>().GetTurnSpirit(),
            Phase.End
            ));
    }

}
