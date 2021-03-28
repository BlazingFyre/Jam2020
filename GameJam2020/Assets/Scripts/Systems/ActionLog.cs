using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;
using static SleepStates;
using static Sides;
using System;

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
    public bool processingQueue = false;

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
        if (action != null && !action.WillFizzle() && !action.HasFizzled())
        {

            yield return StartCoroutine(action.BeforeTriggers());

            if (!action.HasFizzled())
            {

                if (printLog)
                {
                    action.Print();
                }
                log.Add(action);

                yield return StartCoroutine(action.Run());

                yield return StartCoroutine(action.AfterTriggers());

            }
        }

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
        protected ActionLog actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
        // The time delay of each action. Overriden if a delay is used
        protected virtual float actionDelay { get; set; } = 0;

        public Action(SpiritWhole source)
        {
            this.source = source;
            this.children = new List<Action>();
        }

        // Process an action immediately instead of entering it into the queue
        // Used for sub-actions
        public IEnumerator SubProcess(Action action)
        {
            yield return actionLog.StartCoroutine(actionLog.Process(action));
        }

        // Run action through all statuses, moods, etc. to update things like damage values
        public virtual IEnumerator BeforeTriggers()
        {
            yield return ProcessTriggers(true);
        }

        // Run action through all statuses, moods, etc. to trigger conditionals
        public virtual IEnumerator AfterTriggers()
        {
            yield return ProcessTriggers(false);
        }

        private IEnumerator ProcessTriggers(bool preAction)
        {
            // Process action through all statuses
            // TODO: Fix ordering
            Status[] statuses = FindObjectsOfType<Status>();

            Debug.Log("statuses: " + statuses);

            foreach (Status s in statuses)
            {
                if (preAction)
                {
                    yield return s.ProcessPreAction(this);
                }
                else
                {
                    yield return s.ProcessPostAction(this);
                }
            }

            // TODO: Implement moods
            yield break;
        }

        // Returns true if the action cannot be performed (and therefore should fizzle)
        public virtual bool WillFizzle() { return false; }

        // Returns the current status of the action, for counterspell-like effects
        public bool HasFizzled() { return fizzled; }

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

    public class BattleStart : Action
    {

        public BattleStart() : base(null) { }

        public override IEnumerator Run()
        {
            actionLog.Enter(new PhaseChange(
                actionLog.GetComponent<BattleSystem>().GetPlayer(),
                Phase.Start
                ));

            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
        }

        public override void Print()
        {
            Debug.Log("The battle begins!");
        }

    }

    public class BattleEnd : Action
    {

        public SpiritWhole winner;

        public BattleEnd(SpiritWhole winner) : base(null)
        {
            this.winner = winner;
        }

        public override IEnumerator Run()
        {
            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);

            actionLog.GetComponent<BattleSystem>().EndBattle();
        }

        public override void Print()
        {
            Debug.Log(winner + " has won the battle!");
        }

    }

    public class PhaseChange : Action
    {
        public SpiritWhole turnTaker;
        public Phase phase;
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

        public override IEnumerator AfterTriggers()
        {

            yield return actionLog.StartCoroutine(base.AfterTriggers());

            if (phase == Phase.Start)
            {
                actionLog.Enter(new PhaseChange(
                    turnTaker,
                    Phase.Main
                    ));
            }
            else if (phase == Phase.Main)
            {
                // Temporary! Replace with AI stuff
                if (turnTaker == actionLog.GetComponent<BattleSystem>().GetEnemy())
                {
                    actionLog.Enter(new PhaseChange(
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
                actionLog.Enter(new PhaseChange(
                    actionLog.GetComponent<BattleSystem>().GetNonturnSpirit(),
                    Phase.Start
                    ));
            }

            yield break;
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
            Debug.Log(source + ": " + toRefresh + " refreshes their hand");
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
            // If nothing could've been drawn or the target container is full, fizzle.
            return fromContainer.IsEmpty() || toContainer.IsFull() || (toMove != null && !fromContainer.GetCards().Contains(toMove));
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
            int drawAttempts = amount;

            for (int i = 0; i < drawAttempts; i++)
            {
                Action moveAction = new Move(
                    source,
                    0,
                    toDraw.GetDeck(),
                    0,
                    toDraw.GetHand()
                    );

                // Assure that the number of cards counted as "drawn" in the overall Draw action is correct
                // B: This will not show up on the actionLog readouts, unfortunately

                if (moveAction.WillFizzle())
                {
                    amount--;
                }

                yield return SubProcess(moveAction);
            }
        }

        public override void Print()
        {
            Debug.Log(source + ": " + toDraw + " attempts to draw " + amount + " cards");
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

    // ---- Health Manipulation -------------------------------------------------------------------
    public class HealthChange : Action
    {
        public SpiritHalf target;
        // Positive values increase health, negative values decrease health
        public int amount;
        protected override float actionDelay { get; set; } = 0.25f;

        public HealthChange(SpiritWhole source, SpiritHalf target, int amount) : base(source)
        {
            this.target = target;
            this.amount = amount;
        }

        public override IEnumerator Run()
        {
            target.SetHealth(target.GetHealth() + amount);
            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);

            if (target.GetHealth() <= 0)
            {
                actionLog.Enter(new BattleEnd(
                    (target == actionLog.GetComponent<BattleSystem>().GetPlayer()) 
                        ? actionLog.GetComponent<BattleSystem>().GetEnemy() 
                        : actionLog.GetComponent<BattleSystem>().GetPlayer()
                    ));
            }
        }

        public override void Print()
        {
            Debug.Log(source + ": " + target + "'s health is changed by " + amount);
        }
    }

    public class Damage : Action
    {
        public SpiritHalf target;
        public int amount;

        public Damage(SpiritWhole source, SpiritHalf target, int amount) : base(source)
        {
            this.target = target;
            this.amount = amount;
        }

        public override IEnumerator Run()
        {
            yield return SubProcess(new HealthChange(
                source,
                target,
                amount * -1
                ));
        }

        public override void Print()
        {
            Debug.Log(source + ": " + target + " is damaged by " + amount);
        }
    }

    public class Heal : Action
    {
        public SpiritHalf target;
        public int amount;

        public Heal(SpiritWhole source, SpiritHalf target, int amount) : base(source)
        {
            this.target = target;
            this.amount = amount;
        }

        public override IEnumerator Run()
        {
            yield return SubProcess(new HealthChange(
                source,
                target,
                amount
                ));
        }

        public override void Print()
        {
            Debug.Log(source + ": " + target + " is healed by " + amount);
        }
    }

    // ---- Status Manipulation -------------------------------------------------------------------

    public class ApplyStatus : Action
    {

        public SpiritHalf target;
        public Type statusType;
        public int amount;

        protected override float actionDelay { get; set; } = 0.25f;

        public ApplyStatus(SpiritWhole source, SpiritHalf target, Type statusType, int amount) : base(source)
        {
            this.target = target;
            this.statusType = statusType;
            this.amount = amount;
        }

        public override IEnumerator Run()
        {

            Status targetStatus = (Status) target.GetComponent(statusType);

            if (targetStatus == null)
            {
                target.gameObject.AddComponent(statusType);
                targetStatus = (Status) target.GetComponent(statusType);
                targetStatus.SetAmount(amount);
            } 
            else
            {
                targetStatus.SetAmount(targetStatus.GetAmount() + amount);
            }

            if (targetStatus.GetAmount() == 0)
            {
                yield return SubProcess(new RemoveStatus(
                    source,
                    target,
                    statusType
                    ));
            } 
            else
            {
                yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
            }
            
        }

        public override void Print()
        {
            Debug.Log(source + ": " + amount + " " + statusType.Name + " is applied to " + target);
        }

    }

    public class RemoveStatus : Action
    {

        public SpiritHalf target;
        public Type statusType;

        protected override float actionDelay { get; set; } = 0.25f;

        public RemoveStatus(SpiritWhole source, SpiritHalf target, Type statusType) : base(source)
        {
            this.target = target;
            this.statusType = statusType;
        }

        public override IEnumerator Run()
        {
            GameObject.Destroy((Status) target.GetComponent(statusType));

            yield return new WaitForSeconds(actionLog.DelayScalar * actionDelay);
        }

        public override void Print()
        {
            Debug.Log(source + ": all " + statusType.Name + " is removed from " + target);
        }
    }


    // ---- Sleep Manipulation --------------------------------------------------------------------
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
