using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static Combatants;

public class BattleAligner : MonoBehaviour
{

    float verticalChange = 200;
    float horizontalChange = 375;

    public void AlignSpirit(SpiritWhole spiritWhole, Combatant combatant)
    {
        Debug.Log("hello");
        spiritWhole.transform.localPosition = new Vector3(0, 0, 0);

        float newHorizontalChange = (combatant == Combatant.Player) ? horizontalChange : -horizontalChange;

        spiritWhole.GetSpirit(Side.A).transform.localPosition = new Vector3(newHorizontalChange, verticalChange, 0);
        spiritWhole.GetSpirit(Side.B).transform.localPosition = new Vector3(newHorizontalChange, -verticalChange, 0);
    }

}
