using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{

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
