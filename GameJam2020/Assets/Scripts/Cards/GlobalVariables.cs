using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
  private Target selectedObject = null;

  public Target getSelectedObject()
  {
    return this.selectedObject;
  }
  public void setSelectedObject(Target selectedObject_)
  {
    this.selectedObject = selectedObject_;
  }
}
