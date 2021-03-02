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
    public CardContainer playerDeck;
    public CardContainer playerGrave;
    public CardContainer playerHand;
    public CardContainer enemyDeck;
    public CardContainer enemyGrave;
    public CardContainer enemyHand;

    // Information kept for turns
    public SpiritWhole turnSpirit;
    public Phase turnPhase;

    // Time delay variables (for basic animation)
    public float actionDelay = 0.5f;

    public void Start()
    {
        InitBattle();
        AlignBattle();
    }

    private void InitBattle()
    {
        // Place both Spirits under the hierarchy of the Canvas
        playerSpirit.transform.SetParent(battleCanvas.transform, false);
        enemySpirit.transform.SetParent(battleCanvas.transform, false);

        // Link the decks/graveyards/hands to the Spirits
        playerSpirit.InitContainers(playerDeck, playerGrave, playerHand);
        enemySpirit.InitContainers(enemyDeck, enemyGrave, enemyHand);

        // Initialize the Spirit's decks using their Base Decks
        playerSpirit.GetDeck().CopyFrom(playerSpirit.GetBaseDeck());
        enemySpirit.GetDeck().CopyFrom(enemySpirit.GetBaseDeck());

        // Start the first turn
        GetComponent<ActionLog>().Enter(new ActionLog.PhaseChange(
            playerSpirit, 
            Phase.Start
            ));
    }

    private void AlignBattle()
    {
        GetComponent<BattleAligner>().AlignSpirits();
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
