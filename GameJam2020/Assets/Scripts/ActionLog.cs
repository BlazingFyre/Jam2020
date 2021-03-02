using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLog : MonoBehaviour
{

    List<Action> actionLog = new List<Action>();

    public List<Action> GetLog()
    {
        return actionLog;
    }

    public void Enter(Action action)
    {
        // These are fizzle checks. If anything prevents an action, it will deconstruct itself.
        if (action.fizzled) { return; }

        action.BeforeTriggers();
        if (action.fizzled) { return; }
        action.Run();
        if (action.fizzled) { return; }

        actionLog.Add(action);
        action.AfterTriggers();
    }

    public class Action
    {
        public SpiritWhole source;
        public bool fizzled = false;

        public Action(SpiritWhole source)
        {
            this.source = source;
        }
        
        // Run action through all statuses, moods, etc. to update things like damage values.
        public void BeforeTriggers()
        {
            // TODO: All of this crap
        }

        // Run action through all statuses, moods, etc. to trigger conditionals.
        public void AfterTriggers()
        {
            // TODO: All of this crap
        }

        // Actually perform the action.
        public virtual void Run() { }
    }

    public class Damage : Action
    {
        public SpiritWhole target;
        public int amount;

        public Damage(SpiritWhole source, SpiritWhole target, int amount) : base(source)
        {
            this.target = target;
            this.amount = amount;
        }
    }

    public class Discard : Action
    {
        public CardWhole target;

        public Discard(SpiritWhole source, CardWhole target) : base(source)
        {
            this.target = target;
        }

        public override void Run()
        {
            // Place this into its controller's graveyard ...
            target.GetComponent<Use>().GetController().GetGrave().PlaceIndex(
                // ... after drawing it from its respective container
                target.GetCardContainer().DrawTarget(target), 0
            );
        }
    }

    public class Play : Action
    {
        public CardHalf card;
        public GameObject target;

        public Play(SpiritWhole source, CardHalf card, GameObject target) : base(source)
        {
            this.card = card;
            this.target = target;
        }
    }

    public class Draw : Action
    {
        // B: For indices, 0 is top of the deck, -1 is bottom.
        public CardWhole card;
        public int fromIndex;
        public CardContainer fromContainer;
        public int toIndex;
        public CardContainer toContainer;

        public Draw(SpiritWhole source, CardWhole card, CardContainer fromContainer, int toIndex, CardContainer toContainer) : base(source)
        {
            this.card = card;
            this.fromContainer = fromContainer;
            this.toIndex = toIndex;
            this.toContainer = toContainer;
        }

        public Draw(SpiritWhole source, int fromIndex, CardContainer fromContainer, int toIndex, CardContainer toContainer) : base(source)
        {
            card = null;
            this.fromIndex = fromIndex;
            this.fromContainer = fromContainer;
            this.toIndex = toIndex;
            this.toContainer = toContainer;
        }

        public override void Run()
        {
            // If nothing could've been drawn, fizzle.
            if (fromContainer.IsEmpty())
            {
                fizzled = true;
                return;
            }

            CardWhole drawnCard;
            if (card == null)
            {
                drawnCard = fromContainer.DrawIndex(fromIndex);
            } 
            else
            {
                drawnCard = fromContainer.DrawTarget(card);
            }

            toContainer.PlaceIndex(drawnCard, toIndex);

        }

    }

}
