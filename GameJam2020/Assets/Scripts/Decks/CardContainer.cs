using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class CardContainer : MonoBehaviour
{
    
    public SpiritFunction owner;
    public GameObject cardDisplay = null;
    public CardAligner cardAligner;

    public int maxCards = int.MaxValue;
    public List<GameObject> cards = new List<GameObject>();

    public void Start()
    {
        cardAligner = gameObject.GetComponent<CardAligner>();

        foreach (GameObject c in cards)
        {
            c.GetComponent<CardFunction>().SetCardContainer(this);
        }

        cardAligner.UpdateAlignment();
    }

    public void FixedUpdate()
    {
        if (cardDisplay != null)
        {
            cardDisplay.GetComponent<TextMeshProUGUI>().text = cards.Count().ToString();
        }
    }

    public void UpdateAligner()
    {
        cardAligner.UpdateAlignment();
    }

    public SpiritFunction GetOwner()
    {
        return owner;
    }

    public void SetOwner(SpiritFunction owner)
    {
        this.owner = owner;
        foreach (GameObject c in cards)
        {
            c.GetComponent<CardFunction>().SetController(owner);
        }
    }

    public List<GameObject> GetCards()
    {
        return cards;
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

    public int Size()
    {
        return cards.Count();
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

            UpdateAligner();

            return target;
        } 
        else
        {
            return null;
        }
    }

    public GameObject DrawIndex(int index)
    {
        if (!IsEmpty())
        {
            GameObject poppedObject = cards[index];
            cards.RemoveAt(index);

            UpdateAligner();

            return poppedObject;
        } 
        else
        {
            return null;
        }
    }

    public void PlaceTop(GameObject target)
    {
        PlaceIndex(0, target);
    }

    public void PlaceBottom(GameObject target)
    {
        PlaceIndex(cards.Count() - 1, target);
    }

    public void PlaceIndex(int index, GameObject target)
    {
        if (IsEmpty())
        {
            cards.Add(target);
            target.GetComponent<CardFunction>().SetCardContainer(this);
            target.GetComponent<CardFunction>().SetController(owner);
        }
        else if (!IsFull())
        {
            cards.Insert(index, target);
            target.GetComponent<CardFunction>().SetCardContainer(this);
            target.GetComponent<CardFunction>().SetController(owner);
        }

        UpdateAligner();
    }

    // B: I totally ripped this from StackExchange, but it also totally works, so ¯\_(ツ)_/¯
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
