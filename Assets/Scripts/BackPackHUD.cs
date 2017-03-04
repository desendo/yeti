using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class BackPackHUD : MonoBehaviour {

    //public static BackPackHUD hud;
    public float height;
    public float scale = 50;

    public LayerMask whatIsItems;
    Collider2D[] colls;
    public bool changed;
    Item temp_item;
    public List<GameObject> Items;
    public List<GameObject> ItemsToDrop;
    private UnityAction itemListener;
    Text health;
    GameObject currentHealthBar;
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject); // позволяет переходить объекту без изменений из уровня в уровень
        if (FindObjectsOfType(GetType()).Length > 1) //уничтожает клон если таковой появится при телепортации. 
            Destroy(gameObject);
        //itemListener = new UnityAction(Func);
        health = gameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();
        currentHealthBar = gameObject.transform.GetChild(1).GetChild(0).gameObject;

    }
    public void UpdateHealth(int cH, int mH)
    {
        //gameObject.Get

        health.text = cH.ToString() + "/" + mH.ToString();
        if (cH < 0) cH = 0;
        currentHealthBar.transform.localScale = new Vector3(100 * cH / mH, currentHealthBar.transform.localScale.y, currentHealthBar.transform.localScale.z);
        currentHealthBar.transform.localPosition = new Vector3((float)-((100 - 100 * (float)cH / (float)mH) / 200), 0, 0.26F);


    }

    public void AddItemTo(GameObject item)
    {
        Items.Add(item);
    }
    // Use this for initialization
    void Start() {
        //Debug.Log(gameObject.GetComponents<Collider2D>()[1].bounds.min);
        //Debug.Log(gameObject.GetComponents<Collider2D>()[1].bounds.max);
        //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + new Vector3(50,50,0), Color.red);


    }
    private List<GameObject> SortByHeight(List<GameObject> itemsToSort)
    {
        List<GameObject> SortedList = itemsToSort.OrderBy(o => o.transform.position.y).ToList();
       // Debug.Log("Sorted");
        return SortedList;
    }
	// Update is called once per frame
	void Update () {
        if (changed)
        {
            Items = SortByHeight(Items);
         //   Debug.Log("изменено");
            changed = false;
        }

        
        height = Camera.main.orthographicSize * 2;
         
        transform.localScale = new Vector3(height / scale,  height / scale, 1);
        
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.05F, 0.87F, 465));
        


    }

    void FixedUpdate()
    {

        CheckEscape();
        

        /* if (changed)
         {

             CheckItemsIn();
             changed = false;
         }
         */
    }
    void CheckEscape() {
        RemoveMissing(Items);
        if (Items.Count>0)
        foreach (GameObject item in Items)
        //for (int i = 0; i < Items.Count;i++)
        {

            if (item.transform.localPosition.y <-0.5F )
                {

                 //   Debug.Log("Сброс");
                ItemsToDrop.Add(item);
                //temp_item = (Item)Items[i].GetComponent(typeof(Item));
                //temp_item.DropToGround();
                changed = true;
                }
        }
        if (ItemsToDrop.Count > 0)
        {
            foreach (GameObject item in ItemsToDrop)
            {
                Items.Remove(item);
                temp_item = (Item)item.GetComponent(typeof(Item));
                temp_item.DropToGround();
            }
            ItemsToDrop.Clear();

        }
     

    }

    private void RemoveMissing(List<GameObject> items)
    {
        for (int i = items.Count - 1; i > -1; i--)
        {
            if (items[i] == null)
                items.RemoveAt(i);
        }
    }
    
    void CheckItemsIn()
    {
        //if (Items != null) Items.Clear();
        Items.Clear();
        colls = Physics2D.OverlapAreaAll( new Vector2(transform.position.x-8 , transform.position.y-8), new Vector2(transform.position.x + 8, transform.position.y + 8), whatIsItems);
       // Debug.Log(colls.Length);
        foreach (Collider2D coll in colls)
        {
            if(coll.gameObject.name !=gameObject.name)
            Items.Add(coll.gameObject);
        }
    }


    void OnEnable()
    {
        EvntMngr.StartListening("drop", itemListener);
        //Debug.Log("Начал послушание " + name1);

    }

    public void DropLast()
    {
       // Debug.Log("Acted drop");
        if (Items.Count >0)
        Items[Items.Count-1].GetComponent<Item>().DropToGround();
        CheckItemsIn();
    }
    void OnDisable()
    {
        EvntMngr.StopListening("drop", itemListener);
        //  Debug.Log("Окончил послушание "+ name1);

    }
}
