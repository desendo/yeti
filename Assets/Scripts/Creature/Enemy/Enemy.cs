using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature {

    CreaturePartManager partManager;
	// Use this for initialization
	void Start () {
        InitComponents(); 
        InitChecks();
        partManager = new CreaturePartManager(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (health.isBelowZero)
            partManager.DecomoseAllToItems();
	}

    private void FixedUpdate()
    {
        CheckGround();
        CheckSides();
    }

}
