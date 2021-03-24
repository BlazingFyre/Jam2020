using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour
{

    // ---- Preloaded (optional) ------------------------------------------------------------------
    public TextMeshProUGUI healthDisplay;
    // --------------------------------------------------------------------------------------------

    public int health = 50;
    public int maxHealth = 50;

    //TODO: Add statuses

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        this.health = ClampHealth(health);
        CheckHealth();

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

        this.health = ClampHealth(health);
        CheckHealth();
    }

    // Consolidate Heal/Damage into actions
    public void Damage(int amount)
    {
        //Assure the health does not exceed maxHealth or go below 0
        SetHealth(health - amount);
        CheckHealth();
    }

    public void Heal(int amount)
    {
        //Assure the health does not exceed maxHealth or go below 0
        SetHealth(health + amount);
        // This check should be here just in case there is some negative healing status
        CheckHealth();
    }

    // Destroys the Entity if health is 0
    private void CheckHealth()
    {
        if (health <= 0)
        {
            Destroy();
        }
    }

    // Returns a valid health value (between 0 and maxHealth)
    private int ClampHealth(int healthValue)
    {
        return Mathf.Min(Mathf.Max(healthValue, 0), maxHealth);
    }

    // TODO: Replace with a Destroy Action!
    public void Destroy()
    {
        print(this.name + " was destroyed!");
        // B: Very temporary
        Object.Destroy(gameObject);
    }

}