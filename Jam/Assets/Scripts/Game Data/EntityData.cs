using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/EntityData")]
public class EntityData : ScriptableObject
{
    [Header("Persistent Data")]
    public CardContainer BaseDeck;
    public CardContainer BaseBin;
    [Space]
    public FloatClamped Health;
    public float EnergyGain;

    [Header("Battle Data")]
    public CardContainer Deck;
    public CardContainer Bin;
    public CardContainer Hand;
    [Space]
    public FloatClamped Grace;
    public FloatClamped Grit;
    public FloatClamped Energy;
    
    public void OnValidate()
    {

    }
}
