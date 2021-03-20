using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardHalf : MonoBehaviour
{

    // Card data, overwritten by subclasses (see CardHalf Template)
    public string halfName = "Default Name";
    public string halfText = "Default text.";
    public Sprite halfArt = null;

    // The action to be logged at the end of the turn (typically discards to the deck)
    public Type DiscardAction { get; set; } = typeof(ActionLog.Discard);

    protected ActionLog actionLog;

    public void Start()
    {
         actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
    }

    public virtual bool IsTargetable(GameObject target) { return false; }

    public virtual void CardFunction(GameObject target) { }

    public void Play(GameObject target)
    {
        // Log "Play" action
        actionLog.Enter(new ActionLog.Cast(
                GetComponent<Half>().GetWhole().GetComponent<Use>().GetController(), 
                this, 
                target
                ));

        // Perform card's actual text
        CardFunction(target);

        // Log DiscardAction action
        actionLog.Enter((ActionLog.Action) System.Activator.CreateInstance(
            DiscardAction,
            GetComponent<Half>().GetWhole().GetComponent<Use>().GetController(),
            GetComponent<Half>().GetWhole().GetComponent<CardWhole>()
            ));
    }

    public SpiritWhole GetController()
    {
        return GetComponent<Half>().GetWhole().GetComponent<Use>().GetController();
    }

    public string GetName()
    {
        return halfName;
    }

    public string GetText()
    {
        return halfText;
    }

    public Sprite GetArt()
    {
        return halfArt;
    }

}
