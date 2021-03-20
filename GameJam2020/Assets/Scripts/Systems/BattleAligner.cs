using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;

public class BattleAligner : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public Transform playerSideA;
    public Transform playerSideB;
    public Transform enemySideA;
    public Transform enemySideB;

    public Transform playerDeck;
    public Transform playerBin;
    public Transform enemyDeck;
    public Transform enemyBin;
    // --------------------------------------------------------------------------------------------

    public void AlignSpirits()
    {
        Whole playerWhole = GetComponent<BattleSystem>().GetPlayer().GetComponent<Whole>();
        Whole enemyWhole = GetComponent<BattleSystem>().GetEnemy().GetComponent<Whole>();

        // Place both Spirits under the hierarchy of the Canvas
        playerWhole.transform.SetParent(GetComponent<CanvasManager>().GetCanvas().transform, false);
        enemyWhole.transform.SetParent(GetComponent<CanvasManager>().GetCanvas().transform, false);

        // Align their SpiritHalves
        playerWhole.GetSide(Side.A).transform.position = playerSideA.position;
        playerWhole.GetSide(Side.B).transform.position = playerSideB.position;
        enemyWhole.GetSide(Side.A).transform.position = enemySideA.position;
        enemyWhole.GetSide(Side.B).transform.position = enemySideB.position;
    }

    public void AlignDecks()
    {
        SpiritWhole playerSpirit = GetComponent<BattleSystem>().GetPlayer();
        SpiritWhole enemySpirit = GetComponent<BattleSystem>().GetEnemy();

        // Place the decks under the hierarchy of Canvas > Decks
        playerSpirit.GetDeck().transform.SetParent(GetComponent<CanvasManager>().GetDecksObject().transform, false);
        playerSpirit.GetBin().transform.SetParent(GetComponent<CanvasManager>().GetDecksObject().transform, false);
        enemySpirit.GetDeck().transform.SetParent(GetComponent<CanvasManager>().GetDecksObject().transform, false);
        enemySpirit.GetBin().transform.SetParent(GetComponent<CanvasManager>().GetDecksObject().transform, false);

        // Align the decks
        playerSpirit.GetDeck().transform.position = playerDeck.position;
        playerSpirit.GetBin().transform.position = playerBin.position;
        enemySpirit.GetDeck().transform.position = enemyDeck.position;
        enemySpirit.GetBin().transform.position = enemyBin.position;

        // Place the cards under the hierarch of Canvas > Cards
        playerSpirit.GetDeck().GetComponent<DeckAligner>().InitializeCardAlignment();
        playerSpirit.GetBin().GetComponent<DeckAligner>().InitializeCardAlignment();
        enemySpirit.GetDeck().GetComponent<DeckAligner>().InitializeCardAlignment();
        enemySpirit.GetBin().GetComponent<DeckAligner>().InitializeCardAlignment();
    }

}
