using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public Selectable endTurnButton;
    // --------------------------------------------------------------------------------------------

    public Selectable getEndTurnButton()
    {
        return endTurnButton;
    }

}
