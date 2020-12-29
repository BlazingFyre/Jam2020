using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    private GameObject selectedObject = null;
    private GameObject selectedHighlight = null;

    public GameObject getSelectedObject()
    {
        return this.selectedObject;
    }
    public void setSelectedObject(GameObject selectedObject, GameObject highlight)
    {
        this.selectedObject = selectedObject;

        if (selectedHighlight != null)
        {
            selectedHighlight.SetActive(false);
        }

        this.selectedHighlight = highlight;

        if (selectedHighlight != null)
        {
            selectedHighlight.SetActive(true);
        }

    }
}