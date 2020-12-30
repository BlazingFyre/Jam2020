using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    private GameObject selectedObject = null;
    private List<GameObject> potentialTargets = null;

    public GameObject GetSelectedObject()
    {
        return this.selectedObject;
    }
    public void SetSelectedObject(GameObject selectedObject)
    {
        if (this.selectedObject != null)
        {
            this.selectedObject.GetComponent<Select>().DisableSelectedHighlight();
            
            if (this.selectedObject.tag == "Card Side")
            {
                this.selectedObject.GetComponent<Select>().DisableSelectedUpscaling();
            }
        }

        if (selectedObject != null)
        {
            selectedObject.GetComponent<Select>().EnableSelectedHighlight();

            if (selectedObject.tag == "Card Side")
            {
                selectedObject.GetComponent<Select>().EnableSelectedUpscaling();
            }
        }

        this.selectedObject = selectedObject;
    }

    public List<GameObject> GetPotentialTargets()
    {
        return this.potentialTargets;
    }

    public void SetPotentialTargets(List<GameObject> potentialTargets)
    {
        if (this.potentialTargets != null)
        {
            foreach (GameObject o in this.potentialTargets)
            {
                o.GetComponent<Select>().DisableTargetableHighlight();
            }
        }

        if (potentialTargets != null)
        {
            foreach (GameObject o in potentialTargets)
            {
                o.GetComponent<Select>().EnableTargetableHighlight();
            }
        }

        this.potentialTargets = potentialTargets;
    }
}