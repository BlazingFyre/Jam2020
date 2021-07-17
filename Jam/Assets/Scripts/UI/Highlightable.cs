using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Highlightable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum Sorter { Cards }
    public enum Layering { BringToFront, SendToBack }

    [Serializable]
    public class HightlightSettings
    {
        public Layering LayerSetting = Layering.BringToFront;
        public FloatReference Rescaling;
        public bool HighlightWhileSnapping = true;
    }

    public Sorter DefaultSorting;

    public HightlightSettings Settings;

    private UISorter Highlighter;
    private UISorter DefaultSorter;

    private Vector3 DefaultScale;

    void Start()
    {
        Highlighter = FindObjectOfType<GlobalRefs>().BattleUI.Highlights;

        if (DefaultSorting == Sorter.Cards)
        {
            DefaultSorter = FindObjectOfType<GlobalRefs>().BattleUI.Cards;
        }

        DefaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponent<Snappable>() == null || GetComponent<Snappable>().Snapper == null)
        {
            Unhighlight();
        }
        else if (Settings.HighlightWhileSnapping)
        {
            StartCoroutine(UnhighlightWhenAligned());
        }
    }

    public IEnumerator UnhighlightWhenAligned()
    {
        Snappable snappable = GetComponent<Snappable>();

        yield return new WaitUntil(() => snappable.IsAligned);

        Unhighlight();
    }

    public void Highlight()
    {
        DefaultSorter.RemoveFromSorter(gameObject);
        Highlighter.AddToSorter(gameObject);

        if (Settings.LayerSetting == Layering.BringToFront)
        {
            transform.SetAsFirstSibling();
        }
        else if (Settings.LayerSetting == Layering.SendToBack)
        {
            transform.SetAsLastSibling();
        }

        if (transform.localScale == DefaultScale)
        {
            transform.localScale = transform.localScale * Settings.Rescaling.Value;
        }
    }

    public void Unhighlight()
    {
        Highlighter.RemoveFromSorter(gameObject);
        DefaultSorter.AddToSorter(gameObject);

        transform.localScale = DefaultScale;
    }
}
