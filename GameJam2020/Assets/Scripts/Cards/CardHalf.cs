using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardHalf : MonoBehaviour
{

    public virtual string HalfName { get; set; } = "Default Name";
    public virtual string HalfText { get; set; } = "Default text.";
    public virtual Sprite HalfArt { get; set; } = null;

    public Type DiscardAction { get; set; } = typeof(ActionLog.Discard);

    protected ActionLog actionLog;

    public void Start()
    {
         actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
    }

    public virtual bool IsTargetable(GameObject target)
    {
        return false;
    }

    public virtual void CardFunction(GameObject target)
    {
        
    }

    public void Play(GameObject target)
    {
        // Log "Play" action
        actionLog.Enter(new ActionLog.Cast(
                GetComponent<Half>().GetWhole().GetComponent<Use>().GetController(), 
                this, 
                target
                ));

        // Log card's actual text
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

}
