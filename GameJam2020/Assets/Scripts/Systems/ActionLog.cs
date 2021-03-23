using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;
using static SleepStates;
using static Sides;

public class ActionLog : MonoBehaviour
{

    // Enable to get a live readout of the action log
    public bool printLog = false;
    // A scalar applied to all time delays in actions. For adjusting overall animation speed later
    public float DelayScalar { get; set; } = 0.5f;

    // The log of all actions within a battle
    public List<Action> log = new List<Action>();
    // The queue of actions. Item 0 is first in line
    public List<Action> queue = new List<Action>();
    // Whether or not the queue is currently being processed
    private bool processingQueue = false;

    public void ClearLog()
    {
        log.Clear();
    }

    public List<Action> GetLog()
    {
        return log;
    }

    // For "entering" actions into the log to be performed
    public void Enter(Action action)
    {
        queue.Add(action);

        if (!processingQueue)
        {
            processingQueue = true;
            if (GetComponent<BattleSystem>().GetTurnSpirit() == GetComponent<BattleSystem>().GetPlayer())
            {
                GetComponent<BattleUI>().SetPlayerInteraction(false);
            }
            StartCoroutine(Process(queue[0]));
        }
    }

    // Processes the action, including pre-triggers and post-triggers
    public IEnumerator Process(Action action)
    {
        // These are fizzle checks, to prevent impossible actions
        if (action == null || action.fizzled || action.WillFizzle()) { yield break; }

        action.BeforeTriggers();
        if (action.fizzled) { yield break; }

        if (printLog)
        {
            action.Print();
        }
        log.Add(action);

        yield return StartCoroutine(action.Run());

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
                if (GetComponent<BattleSystem>().GetTurnSpirit() == GetComponent<BattleSystem>().GetPlayer())
                {
                    GetComponent<BattleUI>().SetPlayerInteraction(true);
                }
            }
        }
    }

    // ============================================================================================
    // ==== Action Collection =====================================================================
    // ============================================================================================

    // All actions inherit from this
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

        // Process an action immediately instead of entering it into the queue
        // Used for sub-actions
        protected IEnumerator SubProcess(Action action)
        {
            yield return actionLog.StartCoroutine(actionLog.Process(action));
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
        public Action WithDelay(float actionDelay)
        {
            this.actionDelay = actionDelay;
            return this;
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

            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);

            if (phase == Phase.Start)
            {
                yield return SubProcess(new RefreshHand(
                    null, 
                    turnTaker
                    ));
            }
            else if (phase == Phase.End)
            {
                yield return SubProcess(new DiscardHand(
                    null, 
                    turnTaker
                    ));
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
                else if (turnTaker == actionLog.GetComponent<BattleSystem>().GetPlayer())
                {
                    actionLog.GetComponent<BattleUI>().SetPlayerInteraction(true);
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
            yield return actionLog.StartCoroutine(actionLog.Process(new Draw(
                source,
                toRefresh,
                amount
                )));
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
                yield return SubProcess((ActionLog.Action) System.Activator.CreateInstance(
                    toDiscard.GetHand().GetCards()[j].GetSide(SleepState.Waking).GetComponent<CardHalf>().EndTurnAction,
                    source,
                    toDiscard.GetHand().GetCards()[j]
                    ));
            }
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

    // ---- Card Manipulation ---------------------------------------------------------------------

    public class Mirror : Action
    {
        public Whole toMirror;
        protected override float actionDelay { get; set; } = 0.25f;

        public Mirror(SpiritWhole source, Whole toMirror) : base(source)
        {
            this.toMirror = toMirror;
        }

        public override IEnumerator Run()
        {
            toMirror.SwapHalves();
            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toMirror + " is mirrored");
        }
    }

    public class Crack : Action
    {

        public CardWhole toCrack;

        protected override float actionDelay { get; set; } = 0.25f;

        public Crack(SpiritWhole source, CardWhole toCrack) : base(source)
        {
            this.toCrack = toCrack;
        }

        public override IEnumerator Run()
        {
            toCrack.SetDurability(toCrack.GetDurability() - 1);
            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);

            if (toCrack.GetDurability() == 0)
            {
                yield return SubProcess(new Shatter(
                    source,
                    toCrack
                    ));
            }
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toCrack + " cracks");
        }

    }

    public class Shatter : Action
    {

        public CardWhole toShatter;

        protected override float actionDelay { get; set; } = 0.25f;

        public Shatter(SpiritWhole source, CardWhole toShatter) : base(source)
        {
            this.toShatter = toShatter;
        }

        public override IEnumerator Run()
        {
            toShatter.SetDurability(0);
            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);

            yield return SubProcess(new Discard(
                source,
                toShatter
                ));
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toShatter + " shatters");
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
            return toDraw.GetDeck().IsEmpty() && toDraw.GetBin().IsEmpty();
        }

        public override IEnumerator Run()
        {
            for (int i = 0; i < amount; i++)
            {

                // Removed for now

                /*
                // If the deck is empty during an attempted draw, shuffle the bin into the deck
                if (toDraw.GetDeck().IsEmpty() && !toDraw.GetBin().IsEmpty())
                {
                    yield return SubProcess(new MoveDeck(
                        source,
                        toDraw.GetBin(),
                        toDraw.GetDeck()
                        ));

                    yield return SubProcess(new Shuffle(
                        source,
                        toDraw.GetDeck()
                        ));
                }
                */

                yield return SubProcess(new Move(
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
        public CardWhole toDiscard;

        public Discard(SpiritWhole source, CardWhole toDiscard) : base(source)
        {
            this.toDiscard = toDiscard;
        }

        public override IEnumerator Run()
        {
            yield return SubProcess(new Move(
                source,
                toDiscard,
                toDiscard.GetCardContainer(), 
                0,
                toDiscard.GetComponent<Use>().GetController().GetBin()
                ));
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toDiscard + " is discarded");
        }
    }

    public class Recycle : Action
    {
        public CardWhole toRecycle;

        public Recycle(SpiritWhole source, CardWhole toRecycle) : base(source)
        {
            this.toRecycle = toRecycle;
        }

        public override IEnumerator Run()
        {
            yield return SubProcess(new Move(
                source,
                toRecycle,
                toRecycle.GetCardContainer(),
                -1,
                toRecycle.GetComponent<Use>().GetController().GetDeck()
                ));
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toRecycle + " is replaced into the deck");
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

        public override bool WillFizzle()
        {
            return !toShuffle.IsEmpty();
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

    public class MoveDeck : Action
    {
        public CardContainer fromContainer;
        public CardContainer toContainer;
        protected override float actionDelay { get; set; } = 0.25f;

        public MoveDeck(SpiritWhole source, CardContainer fromContainer, CardContainer toContainer) : base(source)
        {
            this.fromContainer = fromContainer;
            this.toContainer = toContainer;
        }

        public override bool WillFizzle()
        {
            return fromContainer.IsEmpty();
        }

        public override IEnumerator Run()
        {
            int amountOfCards = fromContainer.GetCards().Count;

            for (int i = amountOfCards - 1; i > -1; i--)
            {
                yield return SubProcess(new Move(
                    source,
                    i,
                    fromContainer,
                    0,
                    toContainer
                    ).WithDelay(actionDelay / amountOfCards));
            }
        }

        public override void Print()
        {
            Debug.Log(source + ": " + fromContainer + " is placed into " + toContainer);
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
            yield return SubProcess(new SleepChange(
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
