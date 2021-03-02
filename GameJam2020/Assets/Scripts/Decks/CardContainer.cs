using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class CardContainer : MonoBehaviour
{

    public int maxCards = int.MaxValue;
    public List<CardWhole> cards = new List<CardWhole>();

    public TextMeshProUGUI cardNumberDisplay;
    public CardAligner cardAligner;

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

    public void Clear()
    {
        cards = new List<CardWhole>();
    }

    public CardWhole DrawTarget(CardWhole target)
    {
        if (cards.Contains(target))
        {
            cards.Remove(target);

            UpdateDeck();

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

            UpdateDeck();

            return poppedCard;
        }
        else
        {
            return null;
        }
    }

    public void PlaceIndex(CardWhole target, int index)
    {
        if (target == null) 
        {
            return;
        }
        else if (IsEmpty())
        {
            cards.Add(target);
        }
        else if (!IsFull())
        {
            // B: Process negative values as moving backwards from the bottom.
            index = (index + cards.Count) % cards.Count;

            cards.Insert(index, target);
        }

        target.SetCardContainer(this);
        target.GetComponent<Use>().SetController(GetComponent<Use>().GetController());

        UpdateDeck();
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

    public void CopyFrom(CardContainer toCopy)
    {
        cards = new List<CardWhole>();
        CardGenerator cardGenerator = GameObject.FindGameObjectWithTag("Battle Canvas").GetComponent<BattleCanvas>().GetCardsObject().GetComponent<CardGenerator>();

        foreach (CardWhole c in toCopy.GetCards())
        {
            CardWhole newCard = cardGenerator.CopyOf(c);
            cards.Add(newCard);
        }

        foreach (CardWhole c in cards)
        {
            c.GetComponent<Use>().InitOwner(GetComponent<Use>().GetOwner());
        }

        UpdateDeck();

    }

    private void UpdateDeck()
    {
        if (cardNumberDisplay != null)
        {
            cardNumberDisplay.text = cards.Count().ToString();
        }
        if (cardAligner != null)
        {
            cardAligner.UpdateAlignment();
        }
    }

}