using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{


    private float maxHealth;
    private float curHealth;

    public  bool isBelowZero
    {
        get
        {
            return curHealth < 0;
        }
        
    }
    public float currentHealth
    {
        get { return curHealth; }
    }

    /// <summary>
    /// здоровье по умолчанию равно 100
    /// </summary>
    public Health()
    {
        this.maxHealth = 100f;
        curHealth = maxHealth;
    }

    /// <summary>
    /// задать максимум здоровья
    /// </summary>
    public Health(float maxHealth)
    {
        this.maxHealth = maxHealth;
        curHealth = this.maxHealth;
    }
    /// <summary>
    /// задать здоровье напрямую
    /// </summary>
    public void SetHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        curHealth = maxHealth;
    }
    /// <summary>
    /// уменьшить здоровье на величину
    /// </summary>
    public void ReceiveDamage(float damage)
    {   

        curHealth -= damage;
        
    }

}