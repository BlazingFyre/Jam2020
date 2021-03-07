using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public TextMeshProUGUI cardNameDisplay;
    public TextMeshProUGUI cardTextDisplay;

    public void Start()
    {
        cardNameDisplay.text = GetComponent<CardHalf>().HalfName;
        cardTextDisplay.text = GetComponent<CardHalf>().HalfText;
    }

}
