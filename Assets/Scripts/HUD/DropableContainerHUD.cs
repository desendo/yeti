using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DropableContainerHUD : MonoBehaviour
{

    public LayerMask whatIsItems;
    Collider2D[] colls;
    public bool changed;
    PickableItem temp_item;


    public List<GameObject> ItemsToDrop;
    private Inventory inv;
    private bool isHasInv;

    internal void AssignInventory(Inventory inventory)
    {
        inv = inventory;
        isHasInv = true;
    }



    void Start()
    {
        if (whatIsItems == 0)
        {
            Debug.LogError(gameObject.name + ": Установите маску объектов \"UI\"");
        }

    }

    public void AddItem(GameObject item)
    {
        
            inv.items.Add(item);

            item.transform.parent = gameObject.transform;
            item.transform.localPosition = new Vector3(0, 1f, -0.1f);
            item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            changed = true;
        
    }

    internal void SetChanged()
    {
        

        
        changed = true;

    }


    private List<GameObject> SortByHeight(List<GameObject> itemsToSort)
    {
        List<GameObject> SortedList = itemsToSort.OrderBy(o => o.transform.position.y).ToList();
        SortedList.Reverse();
        return SortedList;
    }

    void Update()
    {

        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.05F, 0.86F, 465));
        
    }

    void FixedUpdate()
    {
        if (isHasInv)
        {
            CheckEscape();
            if (changed)
            {
                CheckItemsIn();
                inv.items = SortByHeight(inv.items);
                changed = false;
            }
        }
    }



    void CheckEscape()
    {
        inv.RemoveMissing(inv.items);
        if (inv.items.Count > 0)
            for (int i = 0; i < inv.items.Count; i++)
            {
                if (inv.items[i].transform.localPosition.y < -0.2F)
                {
                    ItemsToDrop.Add(inv.items[i]);
                    changed = true;
                }
            }
        if (ItemsToDrop.Count > 0)
        {
            for (int i = 0; i < ItemsToDrop.Count; i++)
            {
                inv.DropItem(ItemsToDrop[i]);
            }
            ItemsToDrop.Clear();

        }


    }


    void CheckItemsIn()
    {
        
            inv.items.Clear();
            colls = Physics2D.OverlapAreaAll(
                new Vector2(transform.position.x - 0.5f, transform.position.y - 0.6f),
                new Vector2(transform.position.x + 0.5f, transform.position.y + 1.1f), whatIsItems);
            foreach (Collider2D coll in colls)
            {
                if (coll.gameObject.name != gameObject.name)
                    inv.items.Add(coll.gameObject);
            }

            inv.items = inv.items.Distinct().ToList();
        
    }


}
