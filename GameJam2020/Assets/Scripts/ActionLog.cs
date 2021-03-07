using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;
using static SleepStates;

public class ActionLog : MonoBehaviour
{

    public bool printLog = false;
    public float DelayScalar { get; set; } = 1.0f;

    public List<Action> log = new List<Action>();
    public List<Action> queue = new List<Action>();
    private bool processingQueue = false;

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
            StartCoroutine(Process(queue[0]));
        }
    }

    private IEnumerator Process(Action action)
    {
        // These are fizzle checks, to prevent impossible actions from being logged
        if (action == null || action.fizzled || action.WillFizzle()) { yield break; }

        action.BeforeTriggers();
        if (action.fizzled) { yield break; }

        if (printLog)
        {
            action.Print();
        }
        log.Add(action);

        yield return StartCoroutine(action.Run());

        foreach (Action a in action.GetChildren())
        {
            yield return StartCoroutine(Process(a));
        }

        action.AfterTriggers();

        if (queue.Contains(action))
        {
            queue.Remove(action);

            if (queue.Count != 0)
            {
                StartCoroutine(Process(queue[0]));
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
        // The Spirit whose choices this action can be traced back to. Null if there is none
        public SpiritWhole source;
        // The sub-actions that this actions begets
        public List<Action> children;
        // Whether or not the action has "fizzled," or failed to resolve
        public bool fizzled = false;

        // A more convenient way to access the actionLog
        protected ActionLog actionLog;
        // The time delay of each action. Overriden if a delay is used
        protected virtual float actionDelay { get; set; } = 0;

        public Action(SpiritWhole source)
        {
            this.source = source;
            this.children = new List<Action>();
            actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
        }
        
        // For entering sub-actions, which are considered encompassed within the current option
        // Entering an action into the action log would instead queue it for the next available time
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
        public virtual IEnumerator Run() { yield break; }

        // Print a readout of the action for debugging purposes
        public virtual void Print()
        {
            Debug.Log(source + ": " + this);
        }

        // For overriding base action delay times
        public void WithDelay(float actionDelay)
        {
            this.actionDelay = actionDelay;
        }
    }

    // ==== Battle Flow ===========================================================================

    public class PhaseChange : Action
    {
        SpiritWhole turnTaker;
        Phase phase;

        protected override float actionDelay { get; set; } = 1;

        public PhaseChange(SpiritWhole turnTaker, Phase phase) : base(null)
        {
            this.turnTaker = turnTaker;
            this.phase = phase;
        }

        public override IEnumerator Run()
        {
            actionLog.GetComponent<BattleSystem>().SetTurnSpirit(turnTaker);
            actionLog.GetComponent<BattleSystem>().SetPhase(phase);

            if (phase == Phase.Start)
            {
                EnterChild(new RefreshHand(null, turnTaker));
            }
            else if (phase == Phase.End)
            {
                EnterChild(new DiscardHand(null, turnTaker));
            }

            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
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

        public override IEnumerator Run()
        {
            EnterChild(new Draw(
                source,
                toRefresh,
                amount
                ));
            yield break;
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

        public override IEnumerator Run()
        {
            for (int j = toDiscard.GetHand().GetCards().Count - 1; j > -1; j--)
            {
                EnterChild(new Discard(
                    null,
                    toDiscard.GetHand().GetCards()[j]
                    ));
            }
            yield break;
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toDiscard + " discards hand");
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

        protected override float actionDelay { get; set; } = 0.25f;

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

        public override IEnumerator Run()
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

            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
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

        public override IEnumerator Run()
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
            yield break;
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

        public override IEnumerator Run()
        {
            EnterChild(new Move(
                source, 
                target, 
                target.GetCardContainer(), 
                0, 
                target.GetComponent<Use>().GetController().GetGrave()
                ));
            yield break;
        }

        public override void Print()
        {
            Debug.Log(source + ": " + target + " is discarded");
        }
    }

    // ==== Decks =================================================================================

    public class Shuffle : Action
    {
        public CardContainer toShuffle;

        protected override float actionDelay { get; set; } = 0.25f;

        public Shuffle(SpiritWhole source, CardContainer toShuffle) : base(source)
        {
            this.toShuffle = toShuffle;
        }

        public override IEnumerator Run()
        {
            toShuffle.Shuffle();
            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toShuffle + " is shuffled");
        }
    }

    // ==== Spirits ===============================================================================

    public class Damage : Action
    {
        public SpiritHalf target;
        public int amount;

        protected override float actionDelay { get; set; } = 0.25f;

        public Damage(SpiritWhole source, SpiritHalf target, int amount) : base(source)
        {
            this.target = target;
            this.amount = amount;
        }

        public override IEnumerator Run()
        {
            target.Damage(amount);
            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
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

        protected override float actionDelay { get; set; } = 0.25f;

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

        public override IEnumerator Run()
        {
            toChange.SetSleepState(state);

            // Force flip's SleepState to change as well
            EnterChild(new SleepChange(
                source,
                toChange.GetComponent<Half>().GetFlip().GetComponent<SpiritHalf>(),
                (state == SleepState.Dreaming) ? SleepState.Waking : SleepState.Dreaming
                ));

            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toChange + " is now " + state);
        }
    }

}
