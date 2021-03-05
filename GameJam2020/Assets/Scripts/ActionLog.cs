using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Phases;
using static SleepStates;

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
            Debug.Log(source + ": " + toCast + " is cast " + " targeting " + target);
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

            // If nothing could've been drawn, fizzle.
            if (fromContainer.IsEmpty() || !fromContainer.GetCards().Contains(toMove))
            {
                fizzled = true;
                return;
            }
        }

        public Move(SpiritWhole source, int fromIndex, CardContainer fromContainer, int toIndex, CardContainer toContainer) : base(source)
        {
            toMove = null;
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

        public override void Run()
        {
            for (int i = 0; i < amount; i++)
            {
                actionLog.Enter(new Move(
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
            actionLog.Enter(new Move(
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
                actionLog.Enter(new RefreshHand(null, turnTaker));
            }
            else if (phase == Phase.Main)
            {

            }
            else if (phase == Phase.End)
            {
                actionLog.Enter(new DiscardHand(null, turnTaker));
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
            actionLog.Enter(new Draw(
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
                actionLog.Enter(new Discard(
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

}
