using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public TextMeshProUGUI cardNameDisplay;
    public TextMeshProUGUI cardTextDisplay;
    public TextMeshProUGUI cardDurabilityDisplay;
    // --------------------------------------------------------------------------------------------

    public void Start()
    {
        cardNameDisplay.text = GetComponent<CardHalf>().GetName();
        cardTextDisplay.text = GetComponent<CardHalf>().GetText();
    }

    public void UpdateDisplay()
    {
        cardDurabilityDisplay.text = GetComponent<Half>().GetWhole().GetComponent<CardWhole>().GetDurability().ToString();
    }

}
