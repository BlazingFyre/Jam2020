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
    [Space]
    public CardSideData SideA;
    [Space]
    public CardSideData SideB;

    public void Start()
    {
        SideA.Side = Side.A;
        SideB.Side = Side.B;
    }

    public void OnValidate()
    {
        List<Stacktion> NewStacktions = new List<Stacktion>(SideA.StacktionTypes.Count);

        if (SideA.StacktionTypes.Count < SideA.Stacktions.Count)
        {
            for (int i = SideA.StacktionTypes.Count; i < SideA.Stacktions.Count; i++)
            {
                RemoveAsset(SideA.Stacktions[i]);
            }
        }

        for (int i = 0; i < SideA.StacktionTypes.Count; i++)
        {
            if (SideA.Stacktions.Count > i)
            {
                Debug.Log("hello!");

                if (SideA.Stacktions[i].GetType() == null && SideA.StacktionTypes[i].Type == null
                    || SideA.StacktionTypes[i].Type == SideA.Stacktions[i].GetType())
                {
                    NewStacktions.Add(SideA.Stacktions[i]);
                }
                else
                {
                    if (SideA.Stacktions[i] != null)
                    {
                        RemoveAsset(SideA.Stacktions[i]);
                    }

                    if (SideA.StacktionTypes[i].Type != null)
                    {
                        InsertAsset((Stacktion)CreateInstance(SideA.StacktionTypes[i].Type), NewStacktions, i);
                    }
                }
            }
            else if (SideA.StacktionTypes[i].Type != null)
            {
                InsertAsset((Stacktion)CreateInstance(SideA.StacktionTypes[i].Type), NewStacktions, i);
            }
        }

        SideA.Stacktions = NewStacktions;
    }

    private void InsertAsset(Stacktion stacktion, List<Stacktion> newStacktions, int i)
    {
        newStacktions.Add(stacktion);
        AssetDatabase.AddObjectToAsset(stacktion, AssetDatabase.GetAssetPath(this));
        stacktion.name = i + ": " + stacktion.GetType();
    }

    private void RemoveAsset(Stacktion stacktion)
    {
        AssetDatabase.RemoveObjectFromAsset(stacktion);
        DestroyImmediate(stacktion);
    }

}
