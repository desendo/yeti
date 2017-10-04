using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePartManager  {

    GameObject[] parts;
    Transform[] parts_tr;

    List<GameObject> partsList;
    List<GameObject> notPartsList;
    GameObject creature;

    public CreaturePartManager(GameObject creature)
    {
        this.creature = creature;
        InitParts();
    }
    void InitParts()
    {
        parts_tr = creature.GetComponentsInChildren<Transform>();
        
        partsList = new List<GameObject>();
        notPartsList = new List<GameObject>();
        
        for (int i = 0; i < parts_tr.Length; ++i)
        {
            
            if (parts_tr[i].gameObject.tag == "BodyPart")
            {
                partsList.Add(parts_tr[i].gameObject);
                
            }
            else
            {                
                notPartsList.Add(parts_tr[i].gameObject);
            }
        }
        
        
    }

    /// <summary>
    /// разлагает существо до итемов
    /// </summary>
    public void DecomoseAllToItems()
    {
        if (partsList.Count > 0)
        {

            foreach (GameObject part in partsList)//для всех частей обнуляет родителя, активирует  тв тело,

            {
                PickableItem pickableitem;

                Rigidbody2D rbd = part.GetComponent<Rigidbody2D>();
                if (!rbd)
                {
                    part.AddComponent<Rigidbody2D>();
                    rbd = part.GetComponent<Rigidbody2D>();
                }
                part.transform.parent = null;

                if (!part.GetComponent<PickableItem>())
                {
                    part.AddComponent<PickableItem>().enabled = true;                    

                }
                if (!part.GetComponent<ThrowAndDamage>())
                {
                    part.AddComponent<ThrowAndDamage>().enabled = true;

                }
                pickableitem = part.GetComponent<PickableItem>();
                pickableitem.enabled = true;
                
                rbd.gravityScale = 1;
                rbd.useAutoMass = true;
                rbd.simulated = true;
                rbd.bodyType = RigidbodyType2D.Dynamic;
                foreach (Collider2D col in part.GetComponents<Collider2D>())
                {
                    col.enabled = true;
                    col.isTrigger = false;
                    //col.sharedMaterial = new PhysicsMaterial2D("bp");
                }
                pickableitem.SetSortingLayer("PickableItems");
                part.layer = LayerMask.NameToLayer("Items");
                part.tag = "Untagged";
            }
            partsList.Clear();
            foreach (GameObject notPart in notPartsList)
            {

                GameObject.Destroy(notPart);
            }
            notPartsList.Clear();
            GameObject.Destroy(creature);
        }
    }

}
