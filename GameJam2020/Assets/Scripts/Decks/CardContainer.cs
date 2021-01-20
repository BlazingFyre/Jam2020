using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class CardContainer : MonoBehaviour
{

    public int maxCards = int.MaxValue;
    public List<CardWhole> cards = new List<CardWhole>();

    public TextMeshProUGUI cardNumberDisplay = null;

    public void InitContainer(SpiritWhole owner)
    {
        gameObject.AddComponent<Use>();
        gameObject.GetComponent<Use>().InitOwner(owner);

        foreach (CardWhole c in cards)
        {
            c.InitCard(this);
        }
    }

    public void FixedUpdate()
    {
        if (cardNumberDisplay != null)
        {
            cardNumberDisplay.text = cards.Count().ToString();
        }
    }

    public List<CardWhole> GetCards()
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

    public bool Contains(CardWhole target)
    {
        return cards.Contains(target);
    }

    public int Size()
    {
        return cards.Count();
    }

    public void Clear()
    {
        cards = new List<CardWhole>();
    }

    public CardWhole DrawTop()
    {
        return DrawIndex(0);
    }

    public CardWhole DrawBottom()
    {
        return DrawIndex(cards.Count() - 1);
    }

    public CardWhole DrawTarget(CardWhole target)
    {
        if (Contains(target))
        {
            cards.Remove(target);

            return target;
        }
        else
        {
            return null;
        }
    }

    public CardWhole DrawIndex(int index)
    {
        if (!IsEmpty())
        {
            CardWhole poppedCard = cards[index];
            cards.RemoveAt(index);

            return poppedCard;
        }
        else
        {
            return null;
        }
    }

    public void PlaceTop(CardWhole target)
    {
        PlaceIndex(target, 0);
    }

    public void PlaceBottom(CardWhole target)
    {
        PlaceIndex(target, cards.Count() - 1);
    }

    public void PlaceIndex(CardWhole target, int index)
    {
        if (IsEmpty())
        {
            cards.Add(target);
            target.SetCardContainer(this);
            target.GetComponent<Use>().SetController(GetComponent<Use>().GetController());
        }
        else if (!IsFull())
        {
            cards.Insert(index, target);
            target.SetCardContainer(this);
            target.GetComponent<Use>().SetController(GetComponent<Use>().GetController());
        }

    }

    // B: I totally ripped this from StackExchange, but it also totally works, so ¯\_(ツ)_/¯
    public void Shuffle()
    {
        var count = cards.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var temp = cards[i];
            cards[i] = cards[r];
            cards[r] = temp;
        }
    }

    public void CopyDeck(CardContainer toCopy)
    {
        cards = new List<CardWhole>();

        foreach (CardWhole c in toCopy.GetCards())
        {
            cards.Add(c); // This needs to be a dedicated copying function from CardWhole!
        }

        foreach (CardWhole c in cards)
        {
            c.GetComponent<Use>().InitOwner(GetComponent<Use>().GetOwner());
        }
    }

}