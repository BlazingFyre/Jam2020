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

    public void Start()
    {
        InitBattle();
        AlignBattle();
    }

    private void InitBattle()
    {
        // Place both Spirits under the hierarchy of the Canvas
        playerSpirit.transform.parent = battleCanvas.transform;
        enemySpirit.transform.parent = battleCanvas.transform;

        // Link the decks/graveyards/hands to the Spirits
        playerSpirit.InitContainers(playerDeck, playerGrave, playerHand);
        enemySpirit.InitContainers(enemyDeck, enemyGrave, enemyHand);

        // Initialize the Spirit's decks using their Base Decks
        playerSpirit.GetDeck().CopyFrom(playerSpirit.GetBaseDeck());
        enemySpirit.GetDeck().CopyFrom(enemySpirit.GetBaseDeck());

    }

    private void StartTurn(SpiritWhole spirit)
    {
        turnSpirit = spirit;

        // Start Phase
        turnPhase = Phase.Start;

        // Upkeep Phase
        turnPhase = Phase.Upkeep;
        turnSpirit.RefreshMana();
        turnSpirit.RefreshHand();

        turnPhase = Phase.Main;
    }

    private void EndTurn()
    {
        // Downkeep Phase
        turnPhase = Phase.Downkeep;
        turnSpirit.DiscardHand();

        // End Phase
        turnPhase = Phase.End;
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
