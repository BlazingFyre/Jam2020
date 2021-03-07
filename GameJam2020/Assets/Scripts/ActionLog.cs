using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;
using static SleepStates;

public class ActionLog : MonoBehaviour
{

    //yield return new WaitForSeconds(actionDelay);

    public bool printLog = false;
    private bool processingQueue = false;

    public List<Action> log = new List<Action>();
    public List<Action> queue = new List<Action>();

    public List<Action> GetLog()
    {
        return log;
    }

    public void Enter(Action action)
    {
        queue.Add(action);

        if (!processingQueue)
        {
            processingQueue = true;
            Process(queue[0]);
        }
    }

    private void Process(Action action)
    {
        // These are fizzle checks, to prevent impossible actions from being logged
        if (action == null || action.fizzled || action.WillFizzle()) { return; }

        action.BeforeTriggers();
        if (action.fizzled) { return; }

        if (printLog)
        {
            action.Print();
        }

        action.Run();

        foreach (Action a in action.GetChildren())
        {
            Process(a);
        }

        action.AfterTriggers();

        if (queue.Contains(action))
        {
            queue.Remove(action);

            if (queue.Count != 0)
            {
                Process(queue[0]);
            } 
            else
            {
                processingQueue = false;
            }
        }
    }
    
    // ============================================================================================
    // ==== Action Collection =====================================================================
    // ============================================================================================

    public class Action
    {
        public SpiritWhole source;
        public List<Action> children;
        public bool fizzled = false;

        protected ActionLog actionLog;

        public Action(SpiritWhole source)
        {
            this.source = source;
            this.children = new List<Action>();
            actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
        }
        
        // For entering sub-actions (do NOT use ActionLog.Enter within Run)
        protected void EnterChild(Action action)
        {
            children.Add(action);
        }

        public List<Action> GetChildren()
        {
            return children;
        }

        // Run action through all statuses, moods, etc. to update things like damage values
        public virtual void BeforeTriggers()
        {
            // TODO: All of this crap
        }

        // Run action through all statuses, moods, etc. to trigger conditionals
        public virtual void AfterTriggers()
        {
            // TODO: All of this crap
        }

        // Returns true if the action cannot be performed (and therefore should fizzle)
        public virtual bool WillFizzle() { return false; }

        // Actually perform the action
        public virtual void Run() { }

        // Print a readout of the action for debugging purposes
        public virtual void Print()
        {
            Debug.Log(source + ": " + this);
        }
    }

    // ==== Cards =================================================================================

    public class Cast : Action
    {
        public CardHalf toCast;
        public GameObject target;

        public Cast(SpiritWhole source, CardHalf toCast, GameObject target) : base(source)
        {
            this.toCast = toCast;
            this.target = target;
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toCast + " is cast on " + target);
        }
    }

    // ---- Card Movement -------------------------------------------------------------------------

    public class Move : Action
    {
        // B: For indices, 0 is top of the deck, -1 is bottom.
        public CardWhole toMove;
        public int fromIndex;
        public CardContainer fromContainer;
        public int toIndex;
        public CardContainer toContainer;

        public Move(SpiritWhole source, CardWhole toMove, CardContainer fromContainer, int toIndex, CardContainer toContainer) : base(source)
        {
            this.toMove = toMove;
            this.fromContainer = fromContainer;
            this.toIndex = toIndex;
            this.toContainer = toContainer;
        }

        public Move(SpiritWhole source, int fromIndex, CardContainer fromContainer, int toIndex, CardContainer toContainer) : base(source)
        {
            toMove = null;
            this.fromIndex = fromIndex;
            this.fromContainer = fromContainer;
            this.toIndex = toIndex;
            this.toContainer = toContainer;
        }

        public override bool WillFizzle()
        {
            // If nothing could've been drawn, fizzle.
            return fromContainer.IsEmpty() || (toMove != null && !fromContainer.GetCards().Contains(toMove));
        }

        public override void Run()
        {
            CardWhole drawnCard;
            if (toMove == null)
            {
                drawnCard = fromContainer.DrawIndex(fromIndex);
            }
            else
            {
                drawnCard = fromContainer.DrawTarget(toMove);
            }

            toContainer.PlaceIndex(drawnCard, toIndex);
        }

        public override void Print()
        {
            if (toMove == null)
            {
                Debug.Log(source + ": [i: " + fromIndex + "] " + fromContainer + " moves to [i: " + toIndex + "] " + toContainer);
            }
            else
            {
                Debug.Log(source + ": " + toMove + " from " + fromContainer + " moves to [i: " + toIndex + "] " + toContainer);
            }
        }

    }

    public class Draw : Action
    {
        public SpiritWhole toDraw;
        public int amount;

        public Draw(SpiritWhole source, SpiritWhole toDraw, int amount) : base(source)
        {
            this.toDraw = toDraw;
            this.amount = amount;
        }

        public override bool WillFizzle()
        {
            return toDraw.GetDeck().IsEmpty();
        }

        public override void Run()
        {
            for (int i = 0; i < amount; i++)
            {
                EnterChild(new Move(
                source,
                0,
                toDraw.GetDeck(),
                0,
                toDraw.GetHand()
                ));
            }
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toDraw + " draws " + amount + " cards");
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
            EnterChild(new Move(
                source, 
                target, 
                target.GetCardContainer(), 
                0, 
                target.GetComponent<Use>().GetController().GetGrave()
                ));
        }

        public override void Print()
        {
            Debug.Log(source + ": " + target + " is discarded");
        }
    }

    // ==== Battle Flow ===========================================================================

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
                EnterChild(new RefreshHand(null, turnTaker));
            }
            else if (phase == Phase.Main)
            {

            }
            else if (phase == Phase.End)
            {
                EnterChild(new DiscardHand(null, turnTaker));
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

    public class RefreshHand : Action
    {
        public SpiritWhole toRefresh;
        public int amount;

        public RefreshHand(SpiritWhole source, SpiritWhole toRefresh) : base(source)
        {
            this.toRefresh = toRefresh;
            this.amount = toRefresh.GetTurnStartCards();
        }

        public override void Run()
        {
            EnterChild(new Draw(
                source,
                toRefresh,
                amount
                ));
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toRefresh + " refreshes hand, drawing " + amount + " cards");
        }
    }

    public class DiscardHand : Action
    {
        public SpiritWhole toDiscard;

        public DiscardHand(SpiritWhole source, SpiritWhole toDiscard) : base(source)
        {
            this.toDiscard = toDiscard;
        }

        public override void Run()
        {
            for (int j = toDiscard.GetHand().GetCards().Count - 1; j > -1; j--)
            {
                EnterChild(new Discard(
                    null,
                    toDiscard.GetHand().GetCards()[j]
                    ));
            }
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toDiscard + " discards hand");
        }
    }

    // ==== Spirits ===============================================================================

    public class Damage : Action
    {
        public SpiritHalf target;
        public int amount;

        public Damage(SpiritWhole source, SpiritHalf target, int amount) : base(source)
        {
            this.target = target;
            this.amount = amount;
        }

        public override void Run()
        {
            target.Damage(amount);
        }

        public override void Print()
        {
            Debug.Log(source + ": " + target + " is damaged by " + amount);
        }
    }

    public class SleepChange : Action
    {
        public SpiritHalf toChange;
        public SleepState state;

        public SleepChange(SpiritWhole source, SpiritHalf toChange, SleepState state) : base(source)
        {
            this.toChange = toChange;
            this.state = state;
        }

        public override bool WillFizzle()
        {
            // Fizzle if no change is made
            return toChange.GetSleepState() == state;
        }

        public override void Run()
        {
            toChange.SetSleepState(state);
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toChange + " is now " + state);
        }
    }

}
