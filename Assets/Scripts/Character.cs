using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour {

    //бой и повреждения
    public int maxHealth = 100;
    public int currentHealth;

    public bool isPunching;
    public bool isStriking;
    //private bool isKicking;
    public float strikeTimer = 0;
    public float punchTimer = 0;
    //public float attackCoolDown = 0.3f;
    public Collider2D punchTrigger;
    public Collider2D strikeTrigger;
    private Weapon weaponCl;
    public GameObject weapon;
    public float strikeSpeed;
    public float punchSpeed;


    bool invincible;
    public float invincibilityTime = 0.9F;
    //движуха
    private float currentSpeed, oldSpeed = 9.0F;
    public float jumpForce = 1000.0F;
    new private Rigidbody2D rigidbody;
    public bool isGr 
        {
        get { return animator.GetBool("isGr"); }
        set {
            if (value == true) isDoublejumped = false;
            animator.SetBool("isGr", value);
        }
    }

    public float vSpeed

    {
        get { return animator.GetFloat("vSpeed"); }
        set { animator.SetFloat("vSpeed", value); }
    }
    public bool isSides

    {
        get { return animator.GetBool("isSides"); }
        set { animator.SetBool("isSides", value); }
    }
    GameObject check_side_1, check_side_2;


    public int dir; //+1 вправо -1 влево
    public List<GameObject> itemsNear;

    private float time;
    private bool isDucking
    {
        get { return animator.GetBool("isDucking"); }
        set { animator.SetBool("isDucking", value); }
    }
    private bool isRunning = false;
    public bool isInDanger = false;
    public bool isDoublejumped = false;


    public bool isEnabled = false;
    public bool isDead = false;


    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsItems;
    
    //инвентарь//
    private Text mainNotice;
    private Text itemNotice;
    public Text inv;
    public Image itemNotice_back;
    Item tempItem;
    public List<Item> items;
    int itemsQ;
    //инвентарь//

    //речь
    public Image speech_bg;
    public Text speech;
    public float secondsMessage = 7;
    public float textTimer;
    public float speechTimer;
    GameObject paw, foot;

    private Animator animator;
    private SpriteRenderer sprite;

    private CharSubState SubState
    {
        get { return (CharSubState)animator.GetInteger("SubState"); }
        set { animator.SetInteger("SubState", (int)value); }
    }

    public CharState State    {
         get { return (CharState)animator.GetInteger("State"); }
        private set { animator.SetInteger("State",(int)value); }
    }
    private bool isReleased
    {
        get { return (bool)animator.GetBool("released"); }
        set { animator.SetBool("released", (bool)value); }
    }

    private bool isReady
    {
        get { return animator.GetBool("isReady"); }
        set { animator.SetBool("isReady", value); }
    }
    public List<Item> inventory;


    private void Awake()
    {
        //isReady = true;
        //isReleased = true;
        if (speechTimer == 0) speechTimer = 1.0f;
        if (currentSpeed == 0) currentSpeed = oldSpeed;
        punchTimer = punchSpeed;
        strikeTimer = strikeSpeed;

        MasterScript.GM = FindObjectOfType<MasterScript>();//делаем ссылку на экземпляр класса ГлавныйСкрипт
        dir = 1;
        textTimer = secondsMessage;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        transform.position = GameObject.Find("SpawnPoint").transform.position;
        
        DontDestroyOnLoad (transform.gameObject); // позволяет переходить персонажу без изменений из уровня в уровень
		if (FindObjectsOfType (GetType ()).Length > 1) //уничтожает клон если таковой появится при телепортации. жестокость нового уровня.
			Destroy (gameObject);
        speech = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
        speech_bg = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        if (currentHealth == 0) currentHealth = maxHealth;
        //paw = GameObject.Find("yeti-parts_paw_left");
        paw = gameObject.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject;
        check_side_2 = gameObject.transform.GetChild(1).GetChild(2).gameObject;
        check_side_1 = gameObject.transform.GetChild(1).GetChild(1).gameObject;
     
        //foot = GameObject.Find("yeti-parts_foot_left");

        foot = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject;
        weapon = paw.transform.GetChild(0).gameObject;
        weaponCl = weapon.GetComponent<Weapon>();
        weaponCl.used = false;
        strikeSpeed = weaponCl.strikeSpeed;
        punchSpeed = weaponCl.punchSpeed;
        Debug.Log(weapon.name);


    }

    private void Start()
    {
        mainNotice = MasterScript.GM.mainNotice;
        itemNotice = MasterScript.GM.itemNotice;
        itemNotice_back = MasterScript.GM.itemNotice_back;
        speech.text = "";
        // inv.text = "";
        speech_bg.enabled = false;
        items = new List<Item>();
        MasterScript.GM.mainNotice.text = "Глава 1. Пробуждение.";
        UpdateDesription();
        EnControl();
        //MasterScript.GM.backpack;

        strikeTrigger = weapon.GetComponent<Collider2D>();
        punchTrigger = weapon.GetComponent<Collider2D>();
        
        punchTrigger.enabled = false;
        //punchTrigger.isTrigger = true;
        punchTrigger.isTrigger = true;
        weapon.transform.localScale = new Vector3(weapon.transform.localScale.x * (1f / paw.transform.localScale.x), weapon.transform.localScale.y * (1f / paw.transform.localScale.y), 1);

        isReleased = true;
    }

    private void FixedUpdate()
    {
        CheckGr();
        CheckItemsNear();
        CheckSides();
        vSpeed =  rigidbody.velocity.y;
    }
    private void Update()
    {
        CheckText();
        if (Input.GetKeyDown("q"))
        {
            //EvntMngr.TriggerEvent("drop");
            

            MasterScript.GM.backpack.DropLast();
            MasterScript.GM.backpack.changed = true;
        }



        if (!isDead)
        {
            if (isGr && !isDucking)

            {

                State = CharState.Idle;
                //SubState = CharSubState.Idle;
            }
            if (!isGr && !isDucking)

            {

                State = CharState.Jump;
                //SubState = CharSubState.Idle;
            }
            if (isGr && isDucking)

            {

                State = CharState.Duck;
                SubState = CharSubState.Idle;
            }
        }

        if (isEnabled)
        {
            if (isDucking && Mathf.Abs(vSpeed) > 15.5F && isGr)

                rigidbody.constraints = RigidbodyConstraints2D.None;

            if (Mathf.Abs(vSpeed) < 15.5F && isGr)
            {
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                gameObject.transform.localRotation = Quaternion.identity;
            }
            if (Input.GetButton("Horizontal") && !isDucking )
            {
                   Walk();

            }
            if (Input.GetButton("Horizontal") && !isDucking && Input.GetKey(KeyCode.LeftShift) )
            {
                Run();
                
                
            }
            if (Input.GetButtonDown("Jump") && isGr && !isDucking)
                Jump();
            if (Input.GetButtonDown("Jump") && !isGr && isSides && Input.GetButton("Horizontal") && !isDoublejumped)
            {
                Jump();
                
                
                
                isDoublejumped = true;
            }
            if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") <= -0.01F)
            {
                
                Duck();
                
            }
            if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0 && isDucking)
            {
                gameObject.transform.localRotation = Quaternion.identity;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                isDucking = false;
                State = CharState.Idle;

            }


            if (Input.GetButtonDown("Jump") && isGr)
            {
                isDucking = false;
                Jump();
            }
            /*  if (Input.GetButtonDown("Fire2") && !isPunching)
              {
                  isPunching = true;
                  punchTimer = punchSpeed;
                  punchTrigger.enabled = true;
                  weaponCl.currentStrikeMoveType = StrikeMoveType.poke;
              }
              if (isPunching)
              {
                  if (punchTimer > 0)
                  {
                      punchTimer -= Time.deltaTime;                    
                      SubState = CharSubState.Punch;


                  }
                  else
                  {
                      weaponCl.used = false;

                      punchTimer = punchSpeed;
                      punchTrigger.enabled = false;
                      isPunching = false;
                      SubState = CharSubState.Idle;

                  }
              }
              */

            

            /*
            if (Input.GetButtonDown("Fire2") )// && !isPunching)
            {
                SubState = CharSubState.Punch;
                isReleased = false;
                //isPunching = true;
                punchTrigger.enabled = false;
               // Debug.Log(Time.frameCount + "  " + SubState);

                
            }
            if (punchTimer < punchSpeed && !isPunching)
            {
                punchTimer += Time.deltaTime;
                
                isReady = false;


            }
            if (punchTimer>= punchSpeed)
            {
                //weaponCl.used = false;
                // punchTrigger.enabled = false;
                weaponCl.used = false;
                isReady = true;

            }
            if (Input.GetButtonUp("Fire2") && !isReleased)
            {
                Debug.Log("Fire2 released");
                if (isReady )
                {
                    SubState = CharSubState.Punch;
                    isReleased = true;
                    
                    punchTimer = 0;
                    punchTrigger.enabled = true;
                    weaponCl.currentStrikeMoveType = StrikeMoveType.poke;
                }
                else
                {
                    SubState = CharSubState.Idle;
                }
            }
            */
            if (Input.GetButtonDown("Fire1") && !isStriking)
            {
                isStriking = true;
                strikeTimer = strikeSpeed;
                strikeTrigger.enabled = true;
                weaponCl.currentStrikeMoveType = StrikeMoveType.swing;
            }
            if (isStriking)
            {
                if (strikeTimer > 0)
                {
                    strikeTimer -= Time.deltaTime;
                    SubState = CharSubState.Strike;
                    


                }
                else
                {
                    weaponCl.used = false;
                    strikeTimer = strikeSpeed;
                    strikeTrigger.enabled = false;
                    isStriking = false;
                    SubState = CharSubState.Idle;


                }
            }


            if (Input.GetButtonDown("Interact"))
            { 
                 
                Interact();
            }
  
        }

    }

    public void Die()
    {
        isEnabled = false;
        isDead = true;
        State = CharState.Die;
        speech_bg.enabled = false;
        speech.text = "";
        mainNotice.text = "Вы погибли";    }

    public void AddToInventory(Item gObject)
    {
        
        inventory.Add(gObject);

        UpdateInv();
    }
    private void UpdateInv()
    {
        //List<Item> SortedList = objListOrder.OrderBy(o => o.OrderDate).ToList();
        //inv.text = "";
        foreach (Item it in inventory)
        {
               // Debug.Log(inv.text);
              //  Debug.Log("имя "+ it.name1);
            //    inv.text +=
            //        it.name1 + 
        //            "\n";

        }
    
    }
    private void CheckGr()
    {
		Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.2F, whatIsGround);

        isGr = colls.Length > 0;

        //Debug.Log(colls.Length);
    }
    private void CheckSides()
    {
        Collider2D[] colls_1 = Physics2D.OverlapCircleAll(check_side_1.transform.position,0.4F, whatIsGround);
        Collider2D[] colls_2 = Physics2D.OverlapCircleAll(check_side_2.transform.position,0.4F, whatIsGround);
        
        isSides = (colls_1.Length > 0 || colls_2.Length > 0);
        

    }

    
    

    private void CheckItemsNear()
    {

        
        Collider2D[] itemsNearColls = Physics2D.OverlapAreaAll( new Vector2(transform.position.x-5 , transform.position.y-5), new Vector2(transform.position.x + 5, transform.position.y + 5),  whatIsItems);

        
        itemsNear.Clear();
        foreach (Collider2D col in itemsNearColls)
        {
            try
            {
                if (col.gameObject.GetComponent<Interaction>() && col.gameObject.GetComponent<Interaction>().isActive)
                    itemsNear.Add(col.gameObject);
            }
            catch
            {
                Debug.Log(col.gameObject.name);

            }
        }
        List<GameObject> SortedList = itemsNear.OrderBy(o => Mathf.Abs( o.transform.position.x - gameObject.transform.position.x)).ToList();
        itemsNear = SortedList;
        UpdateDesription();
    }
    void UpdateDesription()
    {
        if (itemsNear.Count > 0)
        {
            tempItem = (Item)itemsNear[0].GetComponent(typeof(Item));
            itemNotice.text = tempItem.humanName + " (E - "+tempItem.interactionName+")";
            itemNotice_back.enabled = true;
        }
        else
        {
            itemNotice.text = "" ;
            itemNotice_back.enabled = false;

        }
    }

    public void TakeDamage(DamageChunk dmg)
    {
        if (!invincible)
        { 
        currentHealth = currentHealth - (int)dmg.damage ;
        MasterScript.GM.backpack.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();

        StartCoroutine(Invincibility());
        }

    }
    private void RemoveMissing(List<Item> items)
    {
        for (var i = items.Count - 1; i > -1; i--)
        {
            if (items[i] == null)
                items.RemoveAt(i);
        }
    }
        private void CheckText()
    {
        if (mainNotice.text != "")
        {

            textTimer -= Time.deltaTime;
            if (textTimer < 1&& textTimer >0)
            {
                mainNotice.gameObject.GetComponent<Animator>().Play("fadeOut");
            }


            if (textTimer < 0)
            {
                mainNotice.text = "";
                textTimer = secondsMessage;
                mainNotice.gameObject.GetComponent<Animator>().Play("idle");

            }
        }
    }
    void Say(string text)
    {
        speech.text = text;
        speech_bg.enabled = true;
        
        StartCoroutine(SpeechTimer());
    }
    IEnumerator SpeechTimer()
    {
        
        yield return new WaitForSeconds(speechTimer);
        speech.text = "";
        speech_bg.enabled = false;
        
        yield break;
    }

 


    private void Interact()
	{

        GameObject[] temp_itemsNear = new GameObject[itemsNear.Count];
        itemsNear.CopyTo(temp_itemsNear);
        for (int i = 0; i < temp_itemsNear.Length; i++)

        {           
            tempItem = (Item) itemsNear[i].GetComponent(typeof(Item));
            Item.TypeOf temp_type = tempItem.tip;
            tempItem.DefaultAction();
            if (temp_type != Item.TypeOf.Interactible)
                MasterScript.GM.backpack.changed = true;
            if(tempItem.defaultActionResult!="")
            Say(tempItem.defaultActionResult);
            UpdateDesription();
            break;
        }
           
    }
    public void WriteMessage(string message)
    {
        mainNotice.text = message;
    }
    public void WriteMessage(string message, float secs)
    {
        this.textTimer = secs;
        mainNotice.text = message;

    }
    public void RemoveItemFromSight(Item it)
	{
			
		if(items.Remove(it))
		
		itemsQ = items.Count;
	}

    private void Walk()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        isRunning = false;
        isDucking = false;
            Vector3 direction = transform.right * Input.GetAxis("Horizontal");
            //Debug.Log(direction.x);
            
            if (direction.x < 0 && transform.localScale.x > 0 )
            {
            //direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //transform.rotation = new Quaternion(0, 180, 0);
            // rot = new Vector3(rot.x, rot.y - 180, rot.z);transform.rotation = Quaternion.Euler(rot);
            speech.transform.localScale = new Vector3(-speech.transform.localScale.x, speech.transform.localScale.y, speech.transform.localScale.z);
            dir = -1;
           
        }
            if (direction.x > 0 && transform.localScale.x < 0)
            {
            dir = 1;
           
            //direction = -direction;
            
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //rot = new Vector3(rot.x, rot.y - 180, rot.z);transform.rotation = Quaternion.Euler(rot);
            speech.transform.localScale = new Vector3(-speech.transform.localScale.x, speech.transform.localScale.y, speech.transform.localScale.z);
     
        }
        if ( !isSides )
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, currentSpeed * Time.deltaTime);
        if (isGr && !isSides) State = CharState.Walk;
        
    }
    private void Run()
    {

        isDucking = false;
        isRunning = true;
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        //Debug.Log(direction.x);
        if (!isSides)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, currentSpeed * 2 *Time.deltaTime);
        if (direction.x < 0 && transform.localScale.x > 0  )
        {
            
            dir = -1;
            
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            speech.transform.localScale = new Vector3(-speech.transform.localScale.x, speech.transform.localScale.y, speech.transform.localScale.z);
           

        }
        if (direction.x > 0 && transform.localScale.x < 0 )
        {
            dir = 1;
           
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            speech.transform.localScale = new Vector3(-speech.transform.localScale.x, speech.transform.localScale.y, speech.transform.localScale.z);

        }

       
        if (isGr && !isSides) State = CharState.Run;

    }
    private void Jump()
    {

        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        State = CharState.Jump;
    }
 

    IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
        yield break;
    }

    private void Duck()
	{

		isDucking = true;
		State = CharState.Duck;

	}
	public void EnControl()
	{isEnabled = true;	}
	public void DisControl()
	{isEnabled = false;}


}
public enum CharState
{
    Idle, Walk, Strike, Jump, Duck, Action, Kick,Die, Pickup
, Run}
public enum CharSubState
{
  Idle, Punch, Action, Kick, Pickup,Strike

}