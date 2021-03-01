using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLog : MonoBehaviour
{

    List<Action> actionLog;

    public List<Action> GetLog()
    {
        return actionLog;
    }

    public void Enter(Action action)
    {
        action.CheckTriggers();
        action.Run();
        actionLog.Add(action);
    }

    public class Action
    {
        public GameObject source;

        public Action(GameObject source)
        {
            this.source = source;
        }
        
        // Run action through all statuses, moods, etc. to update things like damage values and activate triggers.
        public void CheckTriggers()
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

        public Damage(GameObject source, SpiritWhole target, int amount) : base(source)
        {
            this.target = target;
            this.amount = amount;
        }
    }

    public class Discard : Action
    {
        public CardWhole target;

        public Discard(GameObject source, CardWhole target) : base(source)
        {
            this.target = target;
        }
    }

}
