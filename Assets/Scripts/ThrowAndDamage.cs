using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ThrowAndDamage : MonoBehaviour
{

    public float ImpulseDamageFactor;
    Rigidbody2D rgbd;
    Collider2D col;
    public bool isArmed;
    private void Awake()
    {
        isArmed = false;
    }

    void Start()
    {
        
        rgbd = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<Collider2D>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isArmed)
        {
            
            GameObject part = collision.gameObject;
            print(part.name);
            Creature creature = part.GetComponentInParent<Creature>();
            if (creature && creature.health != null)
            {   
                //print(gameObject.name + " массой" + rgbd.mass + " отн. скоростью " + collision.relativeVelocity.magnitude +
                    //" (" + collision.relativeVelocity + ") наносит ущерб " + rgbd.mass * collision.relativeVelocity.magnitude * ImpulseDamageFactor +
                    //" этому: " + creature.name + " в " + part.name);
                creature.health.ReceiveDamage(rgbd.mass * collision.relativeVelocity.magnitude * ImpulseDamageFactor);
                print("ударяю");
                
              
                
            }

        }
        //rgbd.velocity = -collision.relativeVelocity / 2;
        SetDisarmed();
    }

    void Update()
    {

    }
    void SetRigidbodyQuick()
    {
        rgbd.interpolation = RigidbodyInterpolation2D.Interpolate;
        rgbd.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void SetRigidbodyDefault()
    {
        rgbd.interpolation = RigidbodyInterpolation2D.None;
        rgbd.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

    }
    public void SetDisarmed()

    {
        SetRigidbodyDefault();
        this.isArmed = false;
    }
    public void SetArmed()
        
    {
        SetRigidbodyQuick();
        this.isArmed = true;
    }

    internal void Throw(Vector3 velocity)
    {
        rgbd.velocity = velocity/rgbd.mass;
        gameObject.transform.rotation = Quaternion.identity;        
        SetArmed();
    }
}
