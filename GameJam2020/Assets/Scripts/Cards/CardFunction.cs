using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFunction : MonoBehaviour
{

    public string nameA;
    public string nameB;
    public string descriptionA;
    public string descriptionB;
    public Sprite artA;
    public Sprite artB;

    public GameObject sideA;
    public GameObject sideB;

    public CardContainer container;
    public SpiritFunction controller;

    public virtual bool IsTargetableA(GameObject target)
    {
        return false;
    }

    public virtual void PlayA(GameObject target)
    {
        DiscardA();
    }

    public virtual void DiscardA()
    {
        container.DrawTarget(gameObject);
        controller.GetGraveyard().PlaceTop(gameObject);
    }

    public virtual bool IsTargetableB(GameObject target)
    {
        return false;
    }

    public virtual void PlayB(GameObject target)
    {
        DiscardB();
    }

    public virtual void DiscardB()
    {
        container.DrawTarget(gameObject);
        controller.GetGraveyard().PlaceTop(gameObject);
    }

    public virtual string GetNameA()
    {
        return nameA;
    }
    public virtual string GetNameB()
    {
        return nameB;
    }
    public virtual string GetDescA()
    {
        return descriptionA;
    }
    public virtual string GetDescB()
    {
        return descriptionB;
    }
    public virtual Sprite GetArtA()
    {
        return artA;
    }
    public virtual Sprite GetArtB()
    {
        return artB;
    }

    public virtual CardSideControl GetSideA()
    {
        return sideA.GetComponent<CardSideControl>();
    }
    public virtual CardSideControl GetSideB()
    {
        return sideB.GetComponent<CardSideControl>();
    }

    public virtual CardContainer GetCardContainer()
    {
        return container;
    }
    public virtual void SetCardContainer(CardContainer container)
    {
        this.container = container;
    }
    public virtual SpiritFunction GetController()
    {
        return controller;
    }
    public virtual void SetController(SpiritFunction controller)
    {
        this.controller = controller;
    }

}
