using UnityEngine;


/// <summary>
/// Класс наследуется от MonoBehaviour. обладает аниматором, твердым телом, проверками земли, хождением, здоровьем
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(Animator))]


public class Creature : MonoBehaviour {

    protected Rigidbody2D rb;
    protected Animator anim;

    public bool grounded;
    /// <summary>
    /// уперся в стенку?
    /// </summary>
    public bool sideChecked;
    public bool airControl;
    public bool facingRight;
    public float jumpForce;

    public float crouchSpeed = 0.25f;
    public float maxSpeed = 10f;
    public float runMultiplier = 3f;

    private float AttackSpeed = 1f;
    private float AttackPause = 50f;

    public GameObject groundCheck;
    public GameObject sideCheck1, sideCheck2;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Health health;

    

    /// <summary>
    /// инициализация тела2д, рендерера и аниматора
    /// </summary>
    protected void InitComponents()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        health = new Health();
        
    }

    internal void SetAnimatorParameters()
    {

        try
        {
            anim.SetFloat("AttackSpeed", AttackSpeed);
            anim.SetFloat("AttackPause", AttackPause);
        }
        catch
        {
            print("отсутствуют ряд переменных в аниматоре");
        }
    }

    /// <summary>
    /// инициализация сенсоров  и их допусков
    /// </summary>
    protected void InitChecks()
    {
        
        checkRadius = 0.05f;       
        if (gameObject.transform.Find("groundCheck") != null)        
        {
            groundCheck = gameObject.transform.Find("groundCheck").gameObject;
            
        }
        else
        {
            groundCheck = new GameObject("groundCheck");
            groundCheck.transform.parent = gameObject.transform;
            groundCheck.transform.localPosition = Vector3.zero;
            
            Debug.Log("Нет объекта проверки земли, создан");
        }

        if (sideCheck1 == null || sideCheck2 == null)
        {
            Debug.LogError("Задайте боковые сенсоры у "+ gameObject.name);
        }
            
        
        

    }


    /// <summary>
    /// движение существа
    /// </summary>
    /// <param name="moveH">значение оси Horizontal (либо значение от -1 до 1)</param>  
    /// /// <param name="moveV">значение оси Vertical (либо значение от -1 до 1)</param>  
    /// <param name="isCrouch">приседает ли</param>  
    /// <param name="isJumping">прыгает ли</param>  
    /// <param name="isRunning">бежит ли ли</param>  
    public void Move(float moveH, float moveV, bool isCrouch, bool isJumping, bool isRunning)
    {
        // проверка на потолок
        /*if (!crouch && m_Anim.GetBool("Crouch"))
        {
            
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }
        */
       
        // передача значения приседа аниматору
        anim.SetBool("isCrouching", isCrouch);
        
        //проверка на землю или контроль воздуха
        if (grounded || airControl )
        {

            
            // уменьшение скорости
            if (isCrouch) moveH = moveH * crouchSpeed;
            else if (isRunning) moveH = moveH * runMultiplier;

            //moveH = (crouch ? moveH * crouchSpeed : moveH);
            //print(moveH);
            
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            
            float totalSpeed = moveH * maxSpeed;
            totalSpeed *= .1f;
            if (!sideChecked)
            {
                anim.SetFloat("Speed", Mathf.Abs(moveH));
                
                rb.velocity = new Vector2(totalSpeed, rb.velocity.y);
                
            }
            else
                anim.SetFloat("Speed", 0);
            
            if (moveH > 0 && !facingRight)
            {
            
                HorizontalFlip();
            }
            
            else if (moveH < 0 && facingRight)
            {
            
                HorizontalFlip();
            }
        }
        // If the creature should jump...
        
        if (grounded && isJumping && anim.GetBool("isGrounded"))
        {
            grounded = false;
            anim.SetBool("isGrounded", false);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    protected void Act()
    {
        //todo
    }

    protected void CheckGround()
    {
        grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, checkRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                
                grounded = true;

            }
        }
        anim.SetBool("isGrounded", grounded);
        anim.SetFloat("vSpeed", rb.velocity.y);
    }


    protected void CheckSides()
    {
        sideChecked = false;
        Collider2D[] colliders1 = Physics2D.OverlapCircleAll(sideCheck1.transform.position, checkRadius/2, whatIsGround);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(sideCheck2.transform.position, checkRadius/2, whatIsGround);
        //Debug.Log("1: "+colliders1.Length);
        //Debug.Log("2: " + colliders2.Length);
        for (int i = 0; i < colliders1.Length; i++)
        {
            if (colliders1[i].gameObject != gameObject)
            {

                sideChecked = true;

            }
        }
        if(!sideChecked)
        for (int i = 0; i < colliders2.Length; i++)
        {
            if (colliders2[i].gameObject != gameObject)
            {

                sideChecked = true;

            }
        }
        //Debug.Log(sideChecked);


    }



    /// <summary>
    /// зеркальное отображение слева направо при изменении направления движения
    /// </summary>
    protected virtual void HorizontalFlip()
    {
        
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
