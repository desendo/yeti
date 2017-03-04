using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Weapon : MonoBehaviour {

    public bool used = true;
   // public bool isSplash = true;
    public float basicDamage = 20;
    private float damage;
    public float strikeSpeed = 0.5f;
    public float punchSpeed = 0.9f;
    public LayerMask enemy;
    public Dictionary<StrikeMoveType, float> dmgModificatorTable;
    public Dictionary<StrikeMoveType, HitType> hitTypeTable;
    private float defaultDamageModificator;
    private float damageModificator;
    public StrikeMoveType currentStrikeMoveType;
    private HitType hitType;
    private HitType defaultHitType;
    public WeaponPreset preset;

    public float customBulletDamage, customSwingDamage, customPokeDamage, customImpactDamage;
    void Awake()
    {  
    }
    void Start()
    {
        int j = System.Enum.GetNames(typeof(StrikeMoveType)).Length;

        if (defaultDamageModificator == 0)
        {
            defaultDamageModificator = 1.0f;
        }
        defaultHitType = HitType.blunt;
        dmgModificatorTable = new Dictionary<StrikeMoveType, float>();
        hitTypeTable = new Dictionary<StrikeMoveType, HitType>();
        for (int i = 0; i<j; i++)
        {
            dmgModificatorTable.Add((StrikeMoveType)i, defaultDamageModificator);
            hitTypeTable.Add((StrikeMoveType)i, defaultHitType);
        }

        InitHitTypeTable(preset);
       
    }
    void InitHitTypeTable(WeaponPreset preset)
        {
        if (preset == WeaponPreset.axe)
            {
            dmgModificatorTable[StrikeMoveType.bullet] = 0.5f;
            dmgModificatorTable[StrikeMoveType.swing] = 1.2f;
            dmgModificatorTable[StrikeMoveType.poke] = 0.2f;
            dmgModificatorTable[StrikeMoveType.impact] = 0.5f;
            hitTypeTable[StrikeMoveType.bullet] = HitType.cut;
            hitTypeTable[StrikeMoveType.impact] = HitType.blunt;
            hitTypeTable[StrikeMoveType.poke] = HitType.blunt;
            hitTypeTable[StrikeMoveType.swing] = HitType.cut;

        }
        if (preset == WeaponPreset.club)
        {
            dmgModificatorTable[StrikeMoveType.bullet] = 0.6f;
            dmgModificatorTable[StrikeMoveType.swing] = 0.9f;
            dmgModificatorTable[StrikeMoveType.poke] = 0.7f;
            dmgModificatorTable[StrikeMoveType.impact] = 0.5f;

            hitTypeTable[StrikeMoveType.bullet] = HitType.blunt;
            hitTypeTable[StrikeMoveType.impact] = HitType.blunt;
            hitTypeTable[StrikeMoveType.poke] = HitType.blunt;
            hitTypeTable[StrikeMoveType.swing] = HitType.blunt;
        }
        if (preset == WeaponPreset.fist)
        {
            dmgModificatorTable[StrikeMoveType.bullet] = 0.0f;
            dmgModificatorTable[StrikeMoveType.swing] = 0.6f;
            dmgModificatorTable[StrikeMoveType.poke] = 0.9f;
            dmgModificatorTable[StrikeMoveType.impact] = 0.5f;
            hitTypeTable[StrikeMoveType.bullet] = HitType.blunt;
            hitTypeTable[StrikeMoveType.impact] = HitType.blunt;
            hitTypeTable[StrikeMoveType.poke] = HitType.blunt;
            hitTypeTable[StrikeMoveType.swing] = HitType.blunt;
        }
        if (preset == WeaponPreset.sword)
        {
            dmgModificatorTable[StrikeMoveType.bullet] = 0.6f;
            dmgModificatorTable[StrikeMoveType.swing] = 1.2f;
            dmgModificatorTable[StrikeMoveType.poke] = 1.0f;
            dmgModificatorTable[StrikeMoveType.impact] = 0.2f;
            hitTypeTable[StrikeMoveType.bullet] = HitType.pierce;
            hitTypeTable[StrikeMoveType.impact] = HitType.cut;
            hitTypeTable[StrikeMoveType.poke] = HitType.pierce;
            hitTypeTable[StrikeMoveType.swing] = HitType.slash;
        }
        if (preset == WeaponPreset.spear)
        {
            dmgModificatorTable[StrikeMoveType.bullet] = 1.0f;
            dmgModificatorTable[StrikeMoveType.swing] = 0.1f;
            dmgModificatorTable[StrikeMoveType.poke] = 1.1f;
            dmgModificatorTable[StrikeMoveType.impact] = 0.5f;
            hitTypeTable[StrikeMoveType.bullet] = HitType.pierce;
            hitTypeTable[StrikeMoveType.impact] = HitType.pierce;
            hitTypeTable[StrikeMoveType.poke] = HitType.pierce;
            hitTypeTable[StrikeMoveType.swing] = HitType.blunt;
        }
        if(preset == WeaponPreset.custom)

        {
            dmgModificatorTable[StrikeMoveType.bullet] = customBulletDamage;
            dmgModificatorTable[StrikeMoveType.swing] =customSwingDamage;
            dmgModificatorTable[StrikeMoveType.poke] = customPokeDamage;
            dmgModificatorTable[StrikeMoveType.impact] = customImpactDamage;
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        //Debug.Log(gameObject+"  ударил в  "+ col.collider.gameObject.name);
        if (!gameObject.GetComponent<Collider2D>().isTrigger)
        {
            //if (!used) Debug.Log("Ударил в " + col.collider.gameObject.name);

            if (((1 << col.collider.gameObject.layer) & enemy) != 0) // ("Enemy"))
            {
                if (!used)
                {
                    //Debug.Log(gameObject + "  ударил в  " + col.collider.gameObject.name );
                    //Debug.Log(col.contacts[0].normal);

                    if (dmgModificatorTable.TryGetValue(currentStrikeMoveType, out damageModificator))
                    {
                        //  Debug.Log(damageModificator);
                        damage = basicDamage * damageModificator;

                    }
                    if (hitTypeTable.TryGetValue(currentStrikeMoveType, out hitType))
                    {
                        Debug.Log(hitType);


                    }
                    DamageChunk dmgChunk = new DamageChunk(damage, col, currentStrikeMoveType, hitType);
                    col.collider.SendMessageUpwards("TakeDamage", dmgChunk);
                    gameObject.GetComponent<Collider2D>().enabled = false;

                    used = true;

                }
            }
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {

        Debug.Log(gameObject+"  ударил в  "+ col.gameObject.name);

        if (!used) Debug.Log("Ударил в " + col.gameObject.name);

        if (((1 << col.gameObject.layer) & enemy) != 0) // ("Enemy"))
        {
            if (!used)
            {
                Debug.Log(gameObject + "  ударил в  " + col.gameObject.name);
                //Debug.Log(col.contacts[0].normal);

                if (dmgModificatorTable.TryGetValue(currentStrikeMoveType, out damageModificator))
                {
                  //  Debug.Log(damageModificator);
                    damage = basicDamage * damageModificator;

                }
                if (hitTypeTable.TryGetValue(currentStrikeMoveType, out hitType))
                {
                   // Debug.Log(hitType);


                }
                Debug.Log(col);
                DamageChunk dmgChunk = new DamageChunk(damage, col, currentStrikeMoveType, hitType);
                col.SendMessageUpwards("TakeDamage", dmgChunk);
                gameObject.GetComponent<Collider2D>().enabled = false;

                used = true;

            }
        }

    }
}
