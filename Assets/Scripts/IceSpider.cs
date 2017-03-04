using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class IceSpider : MonoBehaviour
{

    public Character char1;
    public Collider2D cl;

    public IceSpider MyChar;
    [SerializeField]
    private float speed = 9.0F;
    [SerializeField]
    public float secondsMessage = 7;
    public float timer;
    private float jumpForce = 2000.0F;
//    new private Rigidbody2D rigidbody;
    [SerializeField]
    private bool isGr = false;

    public bool isEnabled = false;
    public GameObject targetObj;
    [SerializeField]
    private LayerMask whatIsGround;
    public bool isNearTarget;

    private Animator animator;
  //  private SpriteRenderer sprite;

    public  bool isAggro;
    private SpiderState State
    {
        get { return (SpiderState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {

        char1 = collider.GetComponentInParent<Character>();
        cl = collider;
        if (char1)
        {
            char1.Die();
        }
    }

    public void EnControl()
    {
        isEnabled = true;
    }
    public void DisControl()
    {
        isEnabled = false;
    }
    private void Awake()
    {
        
     //   rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
     //   sprite = GetComponentInChildren<SpriteRenderer>();
        
      //  if (MyChar == null)
        //    DontDestroyOnLoad(transform.gameObject); // позволяет переходить персонажу без изменений из уровня в уровень
        if (FindObjectsOfType(GetType()).Length > 1) //уничтожает клон если таковой появится при телепортации. жестокость нового уровня.
            Destroy(gameObject);



    }
    private void Start()
    {
        
    }

    private void CheckGr()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 1.5F, whatIsGround);

        isGr = colls.Length >= 1;

    }

    private void Update()
    {
        //if(targetObj==null)
        //targetObj = GameObject.FindObjectOfType<Character>().gameObject;
        if (char1 ==null)
           
        char1 = FindObjectOfType<Character>();
        targetObj = char1.gameObject;
        State = SpiderState.Idle;

        
        if (isAggro && !char1.isDead)
        {
            char1.isInDanger = true;
            Walk();
        }
        if (isNearTarget && !char1.isDead)
        {
            isAggro = false;

            Strike();
        }
    }
    private void CheckTarget()
    {
        if (Mathf.Abs(transform.position.x - targetObj.transform.position.x)<30)
        {
            isNearTarget = true;
        }
        if (Mathf.Abs(transform.position.x - targetObj.transform.position.x) < 60)
        {
            isAggro = true;
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


        Vector3 direction = Vector3.Normalize(-transform.position + targetObj.transform.position);
        
        //Debug.Log(direction.x);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        if (direction.x > 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //direction.x *= -1;
        }
        if (direction.x < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //transform.localScale.x *= -1;
            //direction.x *= -1;
        }
        if (isGr) State = SpiderState.Walk;

    }

    private void Jump()
    {

    }
    private void Strike()
    {


        State = SpiderState.Strike;

    }
    private void Kick()
    {


 

    }
    private void Duck()
    {


    }


}
public enum SpiderState
{
    Idle, Walk, Strike
}
