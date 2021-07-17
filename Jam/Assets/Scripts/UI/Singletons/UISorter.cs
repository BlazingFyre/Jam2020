using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISorter : MonoBehaviour
{
    public List<GameObject> SortingList;

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                AddToSorter(child.gameObject);
            }
        }
    }

    public void AddToSorter(GameObject obj)
    {
        SortingList.Add(obj);
        obj.transform.SetParent(transform);
    }

    public void RemoveFromSorter(GameObject obj)
    {
        SortingList.Remove(obj);
    }

    public virtual void AddOperation(GameObject obj) { }
    public virtual void RemoveOperation(GameObject obj) { }

}
