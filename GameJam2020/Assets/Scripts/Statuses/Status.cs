using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionLog;

public class Status : MonoBehaviour
{

    // TODO: Implement Comparable interface and ensure proper Status order processing

    public int amount = 0;

    public ActionLog actionLog;

    public void Start()
    {
        actionLog = GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<ActionLog>();
    }

    public virtual IEnumerator ProcessPreAction(Action action)
    {
        yield break;
    }

    public virtual IEnumerator ProcessPostAction(Action action)
    {
        yield break;
    }

    public int GetAmount()
    {
        return amount;
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

}
