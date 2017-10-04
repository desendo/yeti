using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]


public class Weapon : MonoBehaviour {
    

    public bool isHarmable;// переключает повреждаемоспособность оружия

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetArmed()
    {
        this.isHarmable = true;
    }
    public void SetArmed(bool isHarmable)
    {
        this.isHarmable = isHarmable;
        print("Switched to " + isHarmable);
    }
    public void SetDisrmed()
    {
        this.isHarmable = false;
    }
}
