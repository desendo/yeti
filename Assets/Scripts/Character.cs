using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Creature {



    public InteractionField interactionField;
    public LayerMask whatIsItems;


    public List<GameObject> debug_INVENTORY;
    public Inventory inventory;


    public GameObject aimingPoint; // точка прицела
    
    public float throwForce;
    Vector3 pos, dir;

    /// <summary>
    /// Точка выхода объектов инвентаря и родительский объект при неотображаемом хранении (без hud).
    /// </summary>
    private GameObject parentForStoredItems;

    public GameObject backpack;//объект содержащий худ контейнер
    
    private void Awake()
    {

        InitComponents();
        InitChecks();
        facingRight = true;
        interactionField = new InteractionField(this.gameObject, whatIsItems);
        InitInventory(ref inventory, backpack);

    }




    void Start () {
          
    }

    private void FixedUpdate()
    {
        CheckGround();
        CheckSides();
    }


    

    
    void Update () {
        SetAnimatorParameters();
        interactionField.CheckItemsNear();
        debug_INVENTORY = inventory.items;


        //вертит башкой 
        if (true)
        {
            pos = Camera.main.WorldToScreenPoint(aimingPoint.transform.position);
            dir = Input.mousePosition - pos;
            dir.Normalize();
            
            float angle=0;
            if (facingRight) { 

                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            }
            else
                angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, -30f, 30f);
            aimingPoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //
        }

    }
    /// <summary>
    /// инициализация инвентаря. привязка к HUD контейнера или создание невидимого игрового хранилища
    /// </summary>
    /// <param name="inventory">Экземпляр инвентаря. Хранит предметы и методы взаимодействия с предметами в инвентаре</param>  
    /// <param name="backpack">HUD объект контейнера или рюкзака. Должен содержать экземпляр DropableContainerHUD </param>  
    void InitInventory(ref Inventory inventory, GameObject backpack)
    {
        parentForStoredItems = new GameObject("parentForStoredItems");
        parentForStoredItems.transform.parent = gameObject.transform;
        
        parentForStoredItems.transform.localPosition = new Vector3(-0.07f, 0.19f); //откуда будут падать вещи

        if (backpack != null)
        {
            try
            {
                DropableContainerHUD backPackClassExemplar = backpack.GetComponent<DropableContainerHUD>();
                inventory = new Inventory(backPackClassExemplar, parentForStoredItems);
            }
            catch
            {
                Debug.Log("Объект " + backpack.gameObject.name + " не имеет в себе компонента DropableContainerHUD. А должен. Создаю простой инвентарь.");
                
                inventory = new Inventory(parentForStoredItems);
            }
        }
        else
        {
            inventory = new Inventory(parentForStoredItems);
        }
    }

    internal void Fire(bool isFiring1, bool isFiring2, bool isFiring3)
    {
        if (isFiring1)
        {
            anim.SetTrigger("punch");
            Shoot();
            
        }
        if (isFiring2)
        {
            anim.SetTrigger("strike");
        }
        if (isFiring3)
        {
            anim.SetTrigger("kick");
        }
        

    }
    private void Shoot()
    {
        GameObject item;
        
        item = inventory.DropItemAtTop();
        
        if (item!=null  )
        {
            ThrowAndDamage thr = item.GetComponent<ThrowAndDamage>();
            if (thr != null)
            {
                Debug.Log("shoot");
                thr.Throw(dir.normalized * throwForce);
            }
        }

        
    }
    internal void Act(bool isUsing, bool isPicking, bool isDroping)

    {

        // print(isPicking);

        //надо это дерьмо переработать. сделать  вариацию для добавления в инвернтарь или взаимодействия. 
        //например на Е повесить дефолтное возаимодействие (подобрать для подбираемого и использовать для неподбираемого). 
        //при наличии других способов взаимодействия например сожрать это должно быть опцией
        // в классе Destription запилены 3 окошка
        if (isUsing && interactionField.nearestObject != null)
        {            
            interactionField.nearestObject.GetComponent<BaseItem>().UseObject();
        }
        if (isUsing && interactionField.nearestObject != null && interactionField.nearestObject.GetComponent<PickableItem>() )
        {
            inventory.AddItem(interactionField.nearestObject);
        }
        if (isDroping && inventory.items.Count>0)
        {            
            inventory.DropItemAtTop();
        }

        

    }
    protected override void HorizontalFlip()
    {
        facingRight = !facingRight;
        
        Transform CharRig = gameObject.transform.GetChild(0);
        Transform IK = gameObject.transform.GetChild(1);
        
        Vector3 charRigScale = CharRig.localScale;
        Vector3 IKScale = IK.localScale;
        charRigScale.x *= -1;
        IKScale.x *= -1;
        CharRig.localScale = charRigScale;
        IK.localScale = IKScale;

    }


}
