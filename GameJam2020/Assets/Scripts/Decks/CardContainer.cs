using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardContainer : Owner
{
    public int maxCards;
    public List<GameObject> cards = new List<GameObject>();
    public CardAligner aligner = null;

    public void Start()
    {
        foreach (GameObject c in cards)
        {
            c.GetComponent<Owner>().SetOwner(owner);
        }
    }

    public void FixedUpdate()
    {
        if (!IsEmpty())
        {
            aligner.AlignCards(cards);
        }
    }

    public bool IsFull()
    {
        return cards.Count() >= maxCards;
    }

    public bool IsEmpty()
    {
        return cards.Count() == 0;
    }

    public bool Contains(GameObject target)
    {
        return cards.Contains(target);
    }

    public GameObject DrawTop()
    {
        return DrawIndex(0);
    }

    public GameObject DrawBottom()
    {
        return DrawIndex(cards.Count() - 1);
    }

    public GameObject DrawTarget(GameObject target)
    {
        if (Contains(target))
        {
            cards.Remove(target);
            return target;
        }

        return null;
    }

    public GameObject DrawIndex(int index)
    {
        if (!IsEmpty())
        {
            GameObject poppedObject = cards[index];
            cards.RemoveAt(index);
            return poppedObject;
        }

        return null;
    }

    public void PlaceTop(GameObject target)
    {
        if (!IsFull())
        {
            target.GetComponent<Owner>().SetOwner(owner);
            cards.Insert(0, target);
        }
    }

    public void PlaceBottom(GameObject target)
    {
        if (!IsFull())
        {
            target.GetComponent<Owner>().SetOwner(owner);
            cards.Add(target);
        }
    }

    public void PlaceIndex(int index, GameObject target)
    {
        if (!IsFull())
        {
            target.GetComponent<Owner>().SetOwner(owner);
            cards.Insert(index, target);
        }
    }

    // Not entirely sure if this works lol
    public void Shuffle()
    {
        var count = cards.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = cards[i];
            cards[i] = cards[r];
            cards[r] = tmp;
        }
    }

}
