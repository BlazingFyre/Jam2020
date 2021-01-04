﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandAligner : CardAligner
{

    public override void AlignCards()
    {
        float cardWidth = container.GetCards()[0].GetComponent<RectTransform>().rect.width * container.GetCards()[0].GetComponent<RectTransform>().localScale.x;
        float handMiddle = gameObject.GetComponent<RectTransform>().position.x;
        float totalHeldCardsWidth = container.GetCards().Count() * cardWidth;

        float currX = handMiddle - (totalHeldCardsWidth / 2) + (cardWidth / 2);

        foreach (GameObject c in container.GetCards())
        {
            c.GetComponent<RectTransform>().position = new Vector3(currX, gameObject.GetComponent<RectTransform>().position.y);
            currX += cardWidth;
            c.SetActive(true);

        }

        if (!globalVariables.IsTargeting())
        {
            DarkenCards();
        }
    }

    public void DarkenCards()
    {
        if (container.GetOwner().GetWakingSpirit().GetSide() == 'A')
        {
            foreach (GameObject c in container.GetCards())
            {
                if (c.GetComponent<CardFunction>().GetSideA() != null)
                {
                    c.GetComponent<CardFunction>().GetSideA().gameObject.GetComponent<Select>().DisableDarkened();
                    c.GetComponent<CardFunction>().GetSideB().gameObject.GetComponent<Select>().EnableDarkened();
                }
            }
        }
        else
        {
            foreach (GameObject c in container.GetCards())
            {
                if (c.GetComponent<CardFunction>().GetSideA() != null)
                {
                    c.GetComponent<CardFunction>().GetSideA().GetComponent<Select>().EnableDarkened();
                    c.GetComponent<CardFunction>().GetSideB().GetComponent<Select>().DisableDarkened();
                }
            }
        }
    }

}
