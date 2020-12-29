using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    private GameObject selectedObject = null;

    public GameObject getSelectedObject()
    {
        return this.selectedObject;
    }
    public void setSelectedObject(GameObject selectedObject)
    {

        if (this.selectedObject != null)
        {
            this.selectedObject.GetComponent<Select>().DisableHighlight();
        }

        if (selectedObject != null)
        {
            selectedObject.GetComponent<Select>().EnableHighlight();
        }

        this.selectedObject = selectedObject;
        
    }
}