using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{

    // Data supplied before battle starts
    public GameObject playerSpirit;
    public GameObject enemySpirit;
    //TODO: add compatibility for starting items?

    // Unchanging variables for instantiating objects
    public static GameObject deckPrefab;

    public void Awake()
    {
        
    }

}
