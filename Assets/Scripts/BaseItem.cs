using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseItem : MonoBehaviour 
{

    protected SpriteRenderer sr;
    public bool isActive;

    public string itemName;

    public string actionName;

    virtual public void isVisible(bool visibility)
    {
        sr.enabled = visibility;

    }
    public void SetSortingLayer(string name)
    {
        sr.sortingLayerName = name;
    }
    virtual public void UseObject()

    {
        //print(gameObject.name);
    }

    // Use this for initialization

}
