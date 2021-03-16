using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static Combatants;
using static Phases;

public class BattleSystem : MonoBehaviour
{

    // Data supplied before battle starts
    public SpiritWhole playerSpirit;
    public SpiritWhole enemySpirit;

    // Objects that are always loaded and connected to BattleSystem
    public GameObject battleCanvas;
    public GameObject alignersObject;
    public GameObject UIObject;
    public GameObject decksObject;
    public GameObject cardsObject;

    public CardContainer playerHand;
    public CardContainer enemyHand;

    // Information kept for turns
    public SpiritWhole turnSpirit;
    public Phase turnPhase;

    // Time delay variables (for basic animation)
    public float actionDelay = 0.5f;

    public void Start()
    {
        AlignBattle();
        InitBattle();
    }

    private void InitBattle()
    {
        // Link the Hands to each Spirit
        playerSpirit.SetHand(playerHand);
        enemySpirit.SetHand(enemyHand);
        playerHand.GetComponent<Use>().InitOwner(playerSpirit);
        enemyHand.GetComponent<Use>().InitOwner(enemySpirit);

        // Link cards to their Decks/Spirits
        playerSpirit.GetDeck().InitializeCardConnections();
        playerSpirit.GetGrave().InitializeCardConnections();
        enemySpirit.GetDeck().InitializeCardConnections();
        enemySpirit.GetGrave().InitializeCardConnections();

        // Start the first turn
        GetComponent<ActionLog>().Enter(new ActionLog.PhaseChange(
            playerSpirit, 
            Phase.Start
            ));
    }

    private void AlignBattle()
    {
        GetComponent<BattleAligner>().AlignSpirits();
        GetComponent<BattleAligner>().AlignDecks();
    }

    public GameObject GetCardsObject()
    {
        return cardsObject;
    }

    public GameObject GetDecksObject()
    {
        return decksObject;
}

    public GameObject GetCanvas()
    {
        return battleCanvas;
    }

    public SpiritWhole GetPlayer()
    {
        return playerSpirit;
    }

    public SpiritWhole GetEnemy()
    {
        return enemySpirit;
    }

    public SpiritWhole GetTurnSpirit()
    {
        return turnSpirit;
    }

    public SpiritWhole GetNonturnSpirit()
    {
        if (turnSpirit == playerSpirit)
        {
            return enemySpirit;
        } 
        else
        {
            return playerSpirit;
        }
    }

    public void SetTurnSpirit(SpiritWhole spirit)
    {
        this.turnSpirit = spirit;
    }

    public void SetPhase(Phase phase)
    {
        turnPhase = phase;
    }

}
