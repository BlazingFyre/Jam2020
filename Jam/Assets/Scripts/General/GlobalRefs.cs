using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalRefs : MonoBehaviour
{
    [Header("Battle Canvas")]
    public Canvas BattleCanvas;

    [Serializable]
    public class BattleUIManagers
    {
        public CardUI Cards;
        public HighlightUI Highlights;
    }
    public BattleUIManagers BattleUI;
    
}
