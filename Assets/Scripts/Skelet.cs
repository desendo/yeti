using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Skelet : MonoBehaviour
{
    public Skelet MyChar;
    
    private float speed = 9.0F;
    public bool shocked
    {
        set { animator.SetBool("isShocked", value); }
        get { return animator.GetBool("isShocked"); }
    }

    public float secondsMessage = 7;
    public float timer;
    private float jumpForce = 2000.0F;
    int dir = 1;
    
    private bool isGr = false;
    private bool isStriking;

    private float strikeTimer = 0;
   


    public Collider2D strikeTrigger;
    private Weapon weaponCl;
    public GameObject weapon;
    public float strikeSpeed;


    public bool isEnabled = false;
    public GameObject targetObj;
    [SerializeField]
    private LayerMask whatIsGround;

    //логика
    public bool isNearTarget;
    public bool isAggro;
    public Character char1;
    GameObject healthBarHUD;
    GameObject currentHealthBar;
    /// <summary>
    /// здоровье и сопротивление урону
    /// </summary>
    bool invincible;
    public int maxHealth = 100;
    public int currentHealth;
    Dictionary<HitType, float> hitTypeResistance;
    public float defaultRes;
    public float shockTime;

    /// <summary>
    ///hud
    /// </summary>
    GameObject hud;
    public GameObject combatDamagePopupPrefab;
    GameObject[] parts;
    Transform[] parts_tr;

    List<GameObject> partsList;
    private Animator animator;

    

    private SkeletState SkeletState
    {
        get { return (SkeletState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    private SkeletSubState SubState
    {
        get { return (SkeletSubState)animator.GetInteger("SubState"); }
        set { animator.SetInteger("SubState", (int)value); }
    }

    void SetHitTypeResistance()
    {
        int j = System.Enum.GetNames(typeof(HitType)).Length;
        for (int i = 0; i < j; i++)
        {
            hitTypeResistance.Add((HitType)i, defaultRes);
            
        }
        hitTypeResistance[HitType.blunt] = 1.1f;
        hitTypeResistance[HitType.slash] = 0.5f;
        hitTypeResistance[HitType.pierce] = 0.05f;
        hitTypeResistance[HitType.cut] = 0.9f;
    }
    private void Awake()
    {
        shockTime = 1f;
        defaultRes = 1.0f;
        hitTypeResistance = new Dictionary<HitType, float>();
        animator = GetComponent<Animator>();
        hud = gameObject.transform.GetChild(2).gameObject;
        if (currentHealth == 0)
            currentHealth = maxHealth;
        currentHealthBar = gameObject.transform.GetChild(1).GetChild(0).gameObject;
        healthBarHUD = gameObject.transform.GetChild(1).gameObject;
        parts_tr = gameObject.GetComponentsInChildren<Transform>();

        partsList = new List<GameObject>();

        int howManyParts = 0;
        for (int i = 0; i < parts_tr.Length; ++i)
          
        {
            if (parts_tr[i].gameObject.tag == "BodyPart")
            {
                partsList.Add(parts_tr[i].gameObject);
                howManyParts++;
            }

        }
        parts = new GameObject[howManyParts];
        partsList.CopyTo(parts);
        partsList.Clear();
        //Debug.Log(parts.Length);


        SetHitTypeResistance();
    }
    void InitPopUp(string text)
    {
        GameObject temp = Instantiate(combatDamagePopupPrefab) as GameObject;
        //Transform tempTr = gameObject.transform;
        temp.transform.SetParent(hud.transform);
        temp.GetComponent<Text>().text = text;
        temp.transform.localPosition = hud.transform.localPosition;
        temp.transform.localScale = new Vector3(1,1,1);
        //        StartCoroutine();
        Destroy(temp, 0.5F);
       // Destroy(temp, 0.6f);

        // tempTr.localPosition = combatDamagePopupPrefab.transform.localPosition;
        //tempTr.localScale = combatDamagePopupPrefab.transform.localScale;
    }

    public void UpdateHealth(int cH, int mH)
    {
  
        if (cH < 0) cH = 0;
        currentHealthBar.transform.localScale = new Vector3(100 * cH / mH, currentHealthBar.transform.localScale.y, currentHealthBar.transform.localScale.z);


    }
    void Impact(Vector2 collisionNormal)
    {

        Vector2 directionx = new Vector2(collisionNormal.normalized.x*100 , 0);

        
        directionx = directionx.normalized;

        gameObject.GetComponent<Rigidbody2D>().AddForce(-directionx * 3000, ForceMode2D.Impulse);
        }
    public void TakeDamage(DamageChunk damage)
    {
        float dmg;
        dmg =  damage.damage * hitTypeResistance[damage.hitType];
       // Debug.Log(damage);
       
        InitPopUp(((int)dmg).ToString());

        Debug.Log("Принял урон "+ (int)dmg + " в " + damage.collider.gameObject.name);
        // Vector2 collisionPoint = damage.collision.contacts[0].normal;
        isStriking = false;
        StartCoroutine(Shock());
      //  Impact( collisionPoint);
        if (!invincible)
        {
            currentHealth = currentHealth - (int)dmg;
            UpdateHealth(currentHealth, maxHealth);

            if (currentHealth <= 0)

                Die();
            StartCoroutine(Invincibility());
        }

    }
    void Die()
    {
        GameObject.Destroy(gameObject.transform.GetChild(1).gameObject);

        foreach(GameObject part in parts)

            {
            part.transform.parent = null;
            
            if (!part.GetComponent<Item>())
            {
                part.AddComponent<Item>().enabled = true;
            }
            part.GetComponent<Item>().enabled = true;
            if (!part.GetComponent<Interaction>())
            {
                part.AddComponent<Interaction>().enabled = true;
                part.GetComponent<Interaction>().isActive = true;

            }
            part.AddComponent<Rigidbody2D>();
            part.GetComponent<Rigidbody2D>().gravityScale = 5;
            part.GetComponent<Rigidbody2D>().useAutoMass = true;
            foreach (Collider2D col in part.GetComponents<Collider2D>())
            {
                col.enabled = true;
                col.isTrigger = false;
                col.sharedMaterial = new PhysicsMaterial2D("bp");
            }
            part.layer = 10;
            part.tag = "Untagged";
        }
        GameObject.Destroy(gameObject);
    }
    IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(0.1F);
        invincible = false;
        yield break;
    }


    public void EnControl()
    {
        isEnabled = true;
    }
    public void DisControl()
    {
        isEnabled = false;
    }
       private void Start()
    {


    }

    private void CheckGr()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 2.5F, whatIsGround);

        isGr = colls.Length > 0;


    }

    private void Update()
    {
        //if(targetObj==null)
        //targetObj = GameObject.FindObjectOfType<Character>().gameObject;
        if (char1 ==null)
           
        char1 = FindObjectOfType<Character>();
        targetObj = char1.gameObject;
        SkeletState = SkeletState.Idle;

        
        if (isAggro && !char1.isDead &&!isNearTarget)
        {
            char1.isInDanger = true;
            
            Walk();
        }
        if (isNearTarget && !char1.isDead)
        {
            isAggro = false;
            SkeletState = SkeletState.Idle;
            
          //  Strike();
        }
        if (char1.isDead) SubState = SkeletSubState.Idle;
        if (!isNearTarget) SubState = SkeletSubState.Idle;
    }

    private void CheckTarget()
    {
        if (Mathf.Abs(transform.position.x - targetObj.transform.position.x)<5)
        {
            isNearTarget = true;
        }
        if (Mathf.Abs(transform.position.x - targetObj.transform.position.x) < 30)
        {
            isAggro = true;
        }
        if (Mathf.Abs(transform.position.x - targetObj.transform.position.x) > 5)

        {
            isNearTarget = false;
        }
        if (Mathf.Abs(transform.position.x - targetObj.transform.position.x) > 30)
        {
            isAggro = false;
        }
    }
    private void FixedUpdate()
    {
        if (targetObj != null)
            CheckTarget();
        CheckGr();
    }



    private void Walk()
    {
        if (!shocked)
        {

            Vector3 direction = -Vector3.Normalize(new Vector3 (transform.position.x - targetObj.transform.position.x, transform.position.y - targetObj.transform.position.y, 0));
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
            if (direction.x > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                hud.transform.localScale = new Vector3(-hud.transform.localScale.x, hud.transform.localScale.y, 1);
                dir = 1;
            }
            if (direction.x < 0 && transform.localScale.x > 0)
            {

                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                dir = -1;
                hud.transform.localScale = new Vector3(-hud.transform.localScale.x, hud.transform.localScale.y, 1);
            }
            if (isGr) SkeletState = SkeletState.Walk;
        }
    }

    private void Jump()
    {

    }
    private void Strike()
    {


        SubState = SkeletSubState.Strike;

    }

    IEnumerator Shock()
    {
        shocked = true;
        yield return new WaitForSeconds(shockTime);

        shocked = false;
        yield break;
    }

}
public enum SkeletState
{
    Idle, Walk
}
public enum SkeletSubState
{
    Idle, Strike
}
