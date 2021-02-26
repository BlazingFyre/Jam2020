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
    //TODO: add compatibility for starting items?

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
        StartTurn(playerSpirit);
    }

    public void StartTurn(SpiritWhole spirit)
    {
        StartCoroutine(StartTurnRoutine(spirit));
    }

    public IEnumerator StartTurnRoutine(SpiritWhole spirit)
    {
        Debug.Log("turn start: " + spirit);

        turnSpirit = spirit;

        // Start Phase
        turnPhase = Phase.Start;

        // Upkeep Phase
        turnPhase = Phase.Upkeep;
        turnSpirit.RefreshMana();
        yield return new WaitForSeconds(actionDelay);
        turnSpirit.RefreshHand();
        yield return new WaitForSeconds(actionDelay);

        turnPhase = Phase.Main;

        // TODO: Actually make an enemy AI system instead of this automatic turn-ender.

        if (turnSpirit == enemySpirit)
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnRoutine());
    }

    public IEnumerator EndTurnRoutine()
    {
        Debug.Log("turn end: " + turnSpirit);

        // Downkeep Phase
        turnPhase = Phase.Downkeep;
        turnSpirit.DiscardHand();
        yield return new WaitForSeconds(actionDelay);

        // End Phase
        turnPhase = Phase.End;

        // Start next turn
        // B: Perhaps adding "turn queue" to support multiple turns in a row is a good idea.
        // B: But for now, this suffices.

        if (turnSpirit == playerSpirit)
        {
            StartTurn(enemySpirit);
        }
        else
        {
            StartTurn(playerSpirit);
        }

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

    public SpiritWhole getTurnSpirit()
    {
        return turnSpirit;
    }

}
