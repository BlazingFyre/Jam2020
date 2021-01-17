using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public Item(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

}