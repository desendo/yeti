using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory  {

    public List<GameObject> items;
    private DropableContainerHUD hudContainer;
    public GameObject storedItemsParent;
    Transform storedItemsParentTransform;
    private bool useHUDContainer;

    private void CreateInventory(GameObject parentForItems)
    {
        items = new List<GameObject>();
        this.storedItemsParent = parentForItems;
        storedItemsParentTransform = storedItemsParent.transform;
    }
    public Inventory(DropableContainerHUD dropableContainerHUD, GameObject parentForItems)
    {
        CreateInventory(parentForItems);

        hudContainer = dropableContainerHUD; //backpack for yeti
        useHUDContainer = (hudContainer != null);
        hudContainer.AssignInventory(this);
        

    }
    public Inventory(GameObject parentForItems)
    {
        CreateInventory(parentForItems);
    }
    public void AddItem(GameObject item )
    {
       
        PickableItem pickableItem;

        if ((pickableItem = item.GetComponent<PickableItem>()) && !items.Contains(item))
        {

            
            item.layer = LayerMask.NameToLayer("UI");
            if (useHUDContainer)
            {
                hudContainer.AddItem(item);
            }
            else
            {
                items.Add(item);
                pickableItem.isVisible(false);
                pickableItem.transform.parent = storedItemsParent.transform;
            }

        }
       
    }

    public GameObject DropItem(GameObject item)
    {
        PickableItem pickableItem;
        if ((pickableItem = item.GetComponent<PickableItem>()) && items.Contains(item))
        {

            //Debug.Log(item.name + " " + LayerMask.LayerToName(item.layer));
            pickableItem.SetSortingLayer("PickableItems");
            item.layer = LayerMask.NameToLayer("Items");
            
            if (useHUDContainer)
            {

                hudContainer.SetChanged();

            }
            else
            {
                
                pickableItem.isVisible(true);                

            }
            items.Remove(item);
            item.transform.position = storedItemsParentTransform.position;
            pickableItem.transform.parent = null;
            item.GetComponent<Rigidbody2D>().velocity = //чутка подбрасываем вверхъ
                    Vector2.zero + storedItemsParent.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity * 1.1f + Vector2.up;
            return item;
        }
        
        return null;
    }

    internal GameObject  DropItemAtTop()
    {
        if (items.Count > 0)
            return DropItem(items[0]);
        else
            return null;

    }

    public void RemoveMissing(List<GameObject> items)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] == null)
                items.RemoveAt(i);
        }
    }

}
