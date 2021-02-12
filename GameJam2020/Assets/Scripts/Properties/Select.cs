using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    
    public bool highlighted = false;

    void OnMouseOver()
    {
        Debug.Log(gameObject);
        highlighted = true;
    }

    void OnMouseExit()
    {
        highlighted = false;
    }

    void Enter()
    {

    }

}
