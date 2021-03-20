using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    // ---- Preloaded -----------------------------------------------------------------------------
    public GameObject canvas;
    public GameObject alignersObject;
    public GameObject UIObject;
    public GameObject decksObject;
    public GameObject cardsObject;
    // --------------------------------------------------------------------------------------------

    public GameObject GetCanvas()
    {
        return canvas;
    }

    public GameObject GetAlignersObject()
    {
        return alignersObject;
    }

    public GameObject GetUIObject()
    {
        return UIObject;
    }

    public GameObject GetDecksObject()
    {
        return decksObject;
    }

    public GameObject GetCardsObject()
    {
        return cardsObject;
    }

}
