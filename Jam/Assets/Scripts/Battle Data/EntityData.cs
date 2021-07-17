using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Data/EntityData")]
public class EntityData : ScriptableObject
{
    [Header("HP Eye")]
    public FloatClamped Health;
    public FloatClamped Grace;
    public FloatClamped Grit;

    [Header("Mana Eye")]
    public FloatClamped Mana;
    public float ManaGain;
}
