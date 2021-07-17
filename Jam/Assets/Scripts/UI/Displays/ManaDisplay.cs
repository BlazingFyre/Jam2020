using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManaDisplay : MonoBehaviour
{
    [Header("Data")]
    public EntityData EntityData;
    public ColorVariable ManaColor;

    [Header("Images")]
    public Image ManaIris;

    [Header("Text")]
    public TextMeshProUGUI ManaValueText;
    public TextMeshProUGUI ManaGainText;

    public void Start()
    {
        ManaIris.color = ManaColor.Value;
    }

    public void Update()
    {
        ManaValueText.text = EntityData.Mana.Value.ToString();
        ManaGainText.text = EntityData.ManaGain.ToString();
    }
}
