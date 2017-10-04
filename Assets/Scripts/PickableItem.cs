using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections;

//[System.Serializable]


[RequireComponent(typeof(SpriteRenderer))]
public class PickableItem : BaseItem
{

    Rigidbody2D rb;
    Collider2D[] col;
    
    
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        col = gameObject.GetComponents<Collider2D>();
    }
    override public void UseObject()
    {

    }

    void AddToInventory(Inventory inv)
    {
        inv.AddItem(this.gameObject);
    }
    /// <summary>
    /// ставим видимость объекта для помещения или изъятия его из пула или обычного инвентаря 
    /// </summary>
    override public void isVisible(bool visibility)
    {
        base.isVisible(visibility);
        rb.simulated = visibility;
        
        for (int i = 0; i < col.Length; i++)
        {
            col[i].enabled = visibility;
        }
    }

    public void DropFromInventory(Inventory inv)
    {
        inv.DropItem(this.gameObject);
    }
    public void Destroy()
    {

    }
    public void ReturnToPool()
    {

    }
}
