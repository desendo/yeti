using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class InteractionField  {

    public GameObject[] itemsNearArray;
    private int itemsNearMax=50;


    /// <summary>
    /// ближайший предмет. заполняется при вызове CheckItemsNear
    /// </summary>
    public GameObject nearestObject;



    LayerMask whatIsInteractible;
    GameObject whoIsChecking;
    private float checkRange = 0.3f;



    /// <summary>
    /// создаем поле для взаимодействия. Обнаруживает ближайшие объекты при вызове CheckItemsNear. 
    /// </summary>
    /// <param name="whoIsChecking">источник поля</param>
    /// <param name="whatIsInteractible">что искать? </param>
    public InteractionField(GameObject whoIsChecking, LayerMask whatIsInteractible)
    {
        this.whoIsChecking = whoIsChecking;
        this.whatIsInteractible = whatIsInteractible;

       // itemsNear = new List<GameObject>();
        itemsNearArray = new GameObject[itemsNearMax];


    }



    /// <summary>
    /// обнаруживает ближайший предмет. заполняет поле nearestObject
    /// </summary>
    public void CheckItemsNear()
    {
               
        Collider2D[] nearColliders = Physics2D.OverlapAreaAll(
            new Vector2(whoIsChecking.transform.position.x - checkRange, whoIsChecking.transform.position.y - checkRange),
            new Vector2(whoIsChecking.transform.position.x + checkRange, whoIsChecking.transform.position.y + checkRange), 
            whatIsInteractible);


        Array.Clear(itemsNearArray, 0, itemsNearMax);
        int matchedItemIndex = 0;
        for (int i=0; i< nearColliders.Length;i++)
        {
            try
            {

                if (nearColliders[i].gameObject.GetComponent<BaseItem>()!=null && nearColliders[i].gameObject.GetComponent<BaseItem>().isActive)
                {
                    
                    itemsNearArray[matchedItemIndex] = nearColliders[i].gameObject;
                    matchedItemIndex++;

                }
            }
            catch
            {
                Debug.LogError(nearColliders[i].gameObject.name);

            }
        }

        //List<GameObject> SortedList = itemsNear.OrderBy(o => Mathf.Abs(o.transform.position.x - whoIsChecking.transform.position.x)).ToList();
        FindNearestObject();

    }
    void FindNearestObject()
    {
        float minDist;
        float dist;
        int itemsNearCount = itemsNearArray.Count(s => s != null);
        if (itemsNearCount > 0)
        {
            minDist = Mathf.Abs(itemsNearArray[0].transform.position.x - whoIsChecking.transform.position.x);
            nearestObject = itemsNearArray[0];
            for (int i = 0; i < itemsNearCount; i++)
            {
                dist = Mathf.Abs(itemsNearArray[i].transform.position.x - whoIsChecking.transform.position.x);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearestObject = itemsNearArray[i];

                }
            }
            
            EventManager.NearestObjectAppear(nearestObject);
        }
        else
        {
            nearestObject = null;
            EventManager.NearestObjectDisappear();
        }
      
    }
}
