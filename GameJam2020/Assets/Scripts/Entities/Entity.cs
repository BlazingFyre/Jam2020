using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public int health;
    public int maxHealth;

    //TODO: Add statuses

    public Entity() { }
    public Entity(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        this.health = ClampHealth(health);
        CheckHealth();
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

    public void Damage(int amount)
    {
        //Assure the health does not exceed maxHealth or go below 0
        health = ClampHealth(health - amount);
        CheckHealth();
    }

    public void Heal(int amount)
    {
        //Assure the health does not exceed maxHealth or go below 0
        health = ClampHealth(health + amount);
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