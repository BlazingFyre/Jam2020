using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health = 10;
    public int maxHealth = 10;
    //TODO: add statuses

    public Entity() { }
    public Entity(int maxHealth, int health)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public Entity(int maxHealth)
    {
        this.health = maxHealth;
        this.maxHealth = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetHealth(int amount)
    {
        if (health <= 0)
        {
            Destroy();
        }
    }

    public void Damage(int amount)
    {
        //TODO: apply status effects to amount(?)
        health = Mathf.Min(Mathf.Max(health - amount, 0), maxHealth);

        print(this.name + " now has " + health + " HP after being dealt " + amount + " damage.");

        if (health <= 0)
        {
            Destroy();
        }
    }

    public void Heal(int amount)
    {
        //TODO: apply status effects to amount(?)
        health = Mathf.Min(Mathf.Max(health - amount, 0), maxHealth);

        // This check should be here just in case there is some negative healing status
        if (health <= 0)
        {
            Destroy();
        }
    }

    

    public void Destroy()
    {
        print(this.name + " was destroyed!");
    }

}
