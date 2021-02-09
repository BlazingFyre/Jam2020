using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static Combatants;

public class BattleAligner : MonoBehaviour
{

    public Transform playerSideA;
    public Transform playerSideB;
    public Transform enemySideA;
    public Transform enemySideB;

    public void AlignSpirits()
    {

        Whole playerWhole = GetComponent<BattleSystem>().GetPlayer().GetComponent<Whole>();
        Whole enemyWhole = GetComponent<BattleSystem>().GetEnemy().GetComponent<Whole>();

        playerWhole.GetSide(Side.A).transform.position = playerSideA.position;
        playerWhole.GetSide(Side.B).transform.position = playerSideB.position;

        enemyWhole.GetSide(Side.A).transform.position = enemySideA.position;
        enemyWhole.GetSide(Side.B).transform.position = enemySideB.position;

    }

}
