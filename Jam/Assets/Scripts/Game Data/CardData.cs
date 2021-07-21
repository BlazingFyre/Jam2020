using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
using TypeReferences;

[CreateAssetMenu(menuName = "Game Data/CardData")]
public class CardData : ScriptableObject
{
    public enum Side { A, B }

    [Serializable]
    public class CardSideData
    {
        public string Name;
        public string Text;
        public Image Art;

        [Inherits(typeof(Stacktion))]
        public List<TypeReference> StacktionTypes;

        [HideInInspector]
        public Side Side;
        [HideInInspector]
        public List<Stacktion> Stacktions;
    }

    [Header("CTRL + S to see updated Stacktions")]
    
    public CardSideData SideA;
    [Space]
    public CardSideData SideB;

    public void OnValidate()
    {
        SideA.Side = Side.A;
        SideB.Side = Side.B;
        UpdateStacktions(SideA);
        UpdateStacktions(SideB);
    }

    private void UpdateStacktions(CardSideData sideData)
    {
        List<Stacktion> NewStacktions = new List<Stacktion>(sideData.StacktionTypes.Count);

        if (SideA.StacktionTypes.Count < sideData.Stacktions.Count)
        {
            for (int i = sideData.StacktionTypes.Count; i < sideData.Stacktions.Count; i++)
            {
                RemoveAsset(sideData.Side, sideData.Stacktions[i]);
            }
        }

        for (int i = 0; i < sideData.StacktionTypes.Count; i++)
        {
            if (sideData.Stacktions.Count > i)
            {
                if (sideData.Stacktions[i].GetType() == null && sideData.StacktionTypes[i].Type == null
                    || sideData.StacktionTypes[i].Type == sideData.Stacktions[i].GetType())
                {
                    NewStacktions.Add(sideData.Stacktions[i]);
                }
                else
                {
                    if (sideData.Stacktions[i] != null)
                    {
                        RemoveAsset(sideData.Side, sideData.Stacktions[i]);
                    }

                    if (sideData.StacktionTypes[i].Type != null)
                    {
                        InsertAsset(sideData.Side, (Stacktion)CreateInstance(sideData.StacktionTypes[i].Type), NewStacktions, i);
                    }
                }
            }
            else if (sideData.StacktionTypes[i].Type != null)
            {
                InsertAsset(sideData.Side, (Stacktion)CreateInstance(sideData.StacktionTypes[i].Type), NewStacktions, i);
            }
        }

        sideData.Stacktions = NewStacktions;
    }

    private void InsertAsset(Side side, Stacktion stacktion, List<Stacktion> newStacktions, int i)
    {
        newStacktions.Add(stacktion);
        AssetDatabase.AddObjectToAsset(stacktion, AssetDatabase.GetAssetPath(this));
        stacktion.name = side.ToString() + i + ": " + stacktion.GetType();
    }

    private void RemoveAsset(Side side, Stacktion stacktion)
    {
        AssetDatabase.RemoveObjectFromAsset(stacktion);
        DestroyImmediate(stacktion);
    }

}
