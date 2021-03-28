using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Entity : MonoBehaviour
{

    // ---- Preloaded (optional) ------------------------------------------------------------------
    public TextMeshProUGUI healthDisplay;
    // --------------------------------------------------------------------------------------------

    public int health = 50;
    public int maxHealth = 50;

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        // Clamp health between 0 and the maximum durability
        this.health = Mathf.Min(Mathf.Max(0, health), maxHealth);

        if (healthDisplay != null)
        {
            healthDisplay.text = health.ToString() + " / " + maxHealth.ToString();
        }
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;

        // Assure health value is updated accordingly
        SetHealth(health);
    }

}