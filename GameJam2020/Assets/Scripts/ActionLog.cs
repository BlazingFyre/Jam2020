using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;

public class ActionLog : MonoBehaviour
{

    //yield return new WaitForSeconds(actionDelay);

    public bool printLog = false;

    public List<Action> log = new List<Action>();

    public List<Action> GetLog()
    {
        return log;
    }

    public void Enter(Action action)
    {
        // These are fizzle checks. If anything fizzles an action, it won't run.
        if (action.fizzled) { return; }

        action.BeforeTriggers();
        if (action.fizzled) { return; }

        log.Add(action);
        if (printLog)
        {
            action.Print();
        }

        action.Run();
        
        action.AfterTriggers();
    }

    public class Action
    {
        public SpiritWhole source;
        public bool fizzled = false;

        protected ActionLog actionLog;

        public Action(SpiritWhole source)
        {
            this.source = source;

            actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
        }
        
        // Run action through all statuses, moods, etc. to update things like damage values.
        public virtual void BeforeTriggers()
        {
            // TODO: All of this crap
        }

        // Run action through all statuses, moods, etc. to trigger conditionals.
        public virtual void AfterTriggers()
        {
            // TODO: All of this crap
        }

        // Actually perform the action.
        public virtual void Run() { }

        public virtual void Print()
        {
            Debug.Log("Action " + this + " from " + source);
        }
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

        public override void Print()
        {
            Debug.Log(source + " damages " + target + " by " + amount);
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

        public override void Print()
        {
            Debug.Log(source + " discards " + target);
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

        public override void Print()
        {
            Debug.Log(source + " plays " + card + " targeting " + target);
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

            // If nothing could've been drawn, fizzle.
            if (fromContainer.IsEmpty() || !fromContainer.GetCards().Contains(card))
            {
                fizzled = true;
                return;
            }
        }

        public Draw(SpiritWhole source, int fromIndex, CardContainer fromContainer, int toIndex, CardContainer toContainer) : base(source)
        {
            card = null;
            this.fromIndex = fromIndex;
            this.fromContainer = fromContainer;
            this.toIndex = toIndex;
            this.toContainer = toContainer;

            // If nothing could've been drawn, fizzle.
            if (fromContainer.IsEmpty())
            {
                fizzled = true;
                return;
            }
        }

        public override void Run()
        {
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

        public override void Print()
        {
            if (card == null)
            {
                Debug.Log(source + " draws [i: " + fromIndex + "] from " + fromContainer + " and places it into [i: " + toIndex + "] " + toContainer);
            }
            else
            {
                Debug.Log(source + " draws " + card + " from " + fromContainer + " and places it into [i: " + toIndex + "] " + toContainer);
            }
        }

    }

    public class PhaseChange : Action
    {
        SpiritWhole turnTaker;
        Phase phase;

        public PhaseChange(SpiritWhole turnTaker, Phase phase) : base(null)
        {
            this.turnTaker = turnTaker;
            this.phase = phase;
        }

        public override void Run()
        {
            actionLog.GetComponent<BattleSystem>().SetTurnSpirit(turnTaker);
            actionLog.GetComponent<BattleSystem>().SetPhase(phase);

            if (phase == Phase.Start)
            {
                turnTaker.RefreshMana();
                turnTaker.RefreshHand();
            }
            else if (phase == Phase.Main)
            {

            }
            else if (phase == Phase.End)
            {
                turnTaker.DiscardHand();
            }
        }

        public override void AfterTriggers()
        {
            base.AfterTriggers();

            if (phase == Phase.Start)
            {
                actionLog.Enter(new ActionLog.PhaseChange(
                    turnTaker, 
                    Phase.Main
                    ));
            }
            else if (phase == Phase.Main)
            {
                // Temporary! Replace with AI stuff
                if (turnTaker == actionLog.GetComponent<BattleSystem>().GetEnemy())
                {
                    actionLog.Enter(new ActionLog.PhaseChange(
                    turnTaker,
                    Phase.End
                    ));
                }
            }
            else if (phase == Phase.End)
            {
                actionLog.Enter(new ActionLog.PhaseChange(
                    actionLog.GetComponent<BattleSystem>().GetNonturnSpirit(),
                    Phase.Start
                    ));
            }

        }

        public override void Print()
        {
            Debug.Log(turnTaker + " moves to phase " + phase);
        }

    }
}
