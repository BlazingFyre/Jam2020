using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    [Header("Data")]
    public EntityData EntityData;
    public ColorVariable EnergyColor;

    [Header("Images")]
    public Image EnergyIris;

    [Header("Text")]
    public TextMeshProUGUI EnergyValueText;
    public TextMeshProUGUI EnergyGainText;

    public void Start()
    {
        EnergyIris.color = EnergyColor.Value;
    }

    public void Update()
    {
        EnergyValueText.text = EntityData.Energy.Value.ToString();
        EnergyGainText.text = EntityData.EnergyGain.ToString();
    }
}
