using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionLog;
using static Phases;

public class StatusTemplate : Status
{

    // For acting upon actions; typically changes action fields like 'damage'
    public override IEnumerator ProcessPreAction(Action action)
    {
        yield break;
    }

    // For reacting to actions; typically adds subprocesses onto appropriate actions
    public override IEnumerator ProcessPostAction(Action action)
    {
        yield break;
    }

}
