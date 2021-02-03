using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sides;
using static Combatants;

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

    public void Start()
    {
        InitBattle();
        AlignBattle();
    }

    private void InitBattle()
    {
        // Place both Spirits under the hierarchy of the Canvas
        playerSpirit.transform.SetParent(battleCanvas.transform);
        enemySpirit.transform.SetParent(battleCanvas.transform);

        // Link the decks/graveyards/hands to the Spirits
        playerSpirit.InitContainers(playerDeck, playerGrave, playerHand);
        enemySpirit.InitContainers(enemyDeck, enemyGrave, enemyHand);

        // Initialize the Spirit's decks using their Base Decks
        playerSpirit.GetDeck().CopyDeck(playerSpirit.GetBaseDeck());
        enemySpirit.GetDeck().CopyDeck(enemySpirit.GetBaseDeck());
    }

    private void AlignBattle()
    {
        GetComponent<BattleAligner>().AlignSpirit(playerSpirit, Combatant.Player);
        GetComponent<BattleAligner>().AlignSpirit(enemySpirit, Combatant.Enemy);
    }

}
