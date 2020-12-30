using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{

    public Item(int maxHealth, int health)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public Item(int maxHealth)
    {
        this.health = maxHealth;
        this.maxHealth = maxHealth;
    }

}
