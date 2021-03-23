using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Sides;
using static SleepStates;

public class HandAligner : DeckAligner
{

    public override void UpdateAlignment()
    {

        List<CardWhole> cards = GetComponent<CardContainer>().GetCards();

        if (cards.Count != 0)
        {
            // B: No cost (of code readability) too great. (Fan the cards into a "hand.")
            float cardWidth =
            cards[0].GetComponent<RectTransform>().rect.width
            * cards[0].GetComponent<RectTransform>().localScale.x;
            float handMiddle = gameObject.GetComponent<RectTransform>().position.x;
            float totalHeldCardsWidth = cards.Count * cardWidth;

            float currX = handMiddle - (totalHeldCardsWidth / 2) + (cardWidth / 2);

            foreach (CardWhole c in cards)
            {
                c.GetComponent<RectTransform>().position = new Vector3(currX, gameObject.GetComponent<RectTransform>().position.y, 0);
                currX += cardWidth;
            }

            UpdateSleepAlignment();

            // Make all of the cards active
            foreach (CardWhole c in cards)
            {
                c.gameObject.SetActive(true);
            }

        }

    }

    public override void SetCardInteractability(bool interactive)
    {
        foreach (CardWhole c in GetComponent<CardContainer>().GetCards())
        {
            if (interactive)
            {
                c.GetSide(SleepState.Waking).GetComponent<Selectable>().interactable = true;
                c.GetSide(SleepState.Dreaming).GetComponent<Selectable>().interactable = false;
            }
            else
            {
                c.GetComponent<Whole>().GetSide(Side.A).GetComponent<Selectable>().interactable = false;
                c.GetComponent<Whole>().GetSide(Side.B).GetComponent<Selectable>().interactable = false;
            }
        }
    }

    public void UpdateSleepAlignment()
    {
        SetCardInteractability(GameObject.FindGameObjectWithTag("Battle Systems").GetComponent<BattleUI>().IsPlayerInteractionEnabled());
    }

}
