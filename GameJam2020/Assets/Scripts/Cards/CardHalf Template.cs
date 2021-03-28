using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionLog;

public class CardHalfTemplate : CardHalf
{

    // Called when component is added; used to overwrite fields
    public void Reset()
    {
        halfName = "Name";
        halfText = "Card text";
    }

    // Return true if "target" is a valid target for this card
    public override bool IsTargetable(GameObject target)
    {
        return false;
    }

    // The card's meat, bread, and butter. Usually just entering Actions
    public override void CardFunction(GameObject target)
    {
        
    }

}
