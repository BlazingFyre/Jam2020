using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;

public class BattleSystem : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public SpiritWhole playerSpirit;
    public SpiritWhole enemySpirit;

    public CardContainer playerHand;
    public CardContainer enemyHand;
    // --------------------------------------------------------------------------------------------

    // Information kept for turns
    public SpiritWhole turnSpirit;
    public Phase turnPhase;

    public void Start()
    {
        AlignBattle();
        StartBattle(playerSpirit, enemySpirit);
    }

    public void StartBattle(SpiritWhole playerSpirit, SpiritWhole enemySpirit)
    {
        this.playerSpirit = playerSpirit;
        this.enemySpirit = enemySpirit;

        // Link the Hands to each Spirit
        playerSpirit.SetHand(playerHand);
        enemySpirit.SetHand(enemyHand);
        playerHand.GetComponent<Use>().InitOwner(playerSpirit);
        enemyHand.GetComponent<Use>().InitOwner(enemySpirit);

        // Link cards to their Decks/Spirits
        playerSpirit.GetDeck().InitializeCardConnections();
        playerSpirit.GetBin().InitializeCardConnections();
        enemySpirit.GetDeck().InitializeCardConnections();
        enemySpirit.GetBin().InitializeCardConnections();

        // Clear the log
        GetComponent<ActionLog>().ClearLog();

        // Start the first turn
        GetComponent<ActionLog>().Enter(new ActionLog.BattleStart());
    }

    public void EndBattle()
    {

    }

    private void AlignBattle()
    {
        GetComponent<BattleAligner>().AlignSpirits();
        GetComponent<BattleAligner>().AlignDecks();
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
