using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Creature))]
public class DefaultEnemyAI : MonoBehaviour {


    public bool isNearTarget;
    public bool isAggro;
    public Character character;
    GameObject targetGamebject;
    Transform targetTransform;
    Vector2 dirToTarget;
    public float  dirToTargetX,dirToTargetY;
    Creature creature;
    // Use this for initialization
    void Start () {
        
        creature = gameObject.GetComponent<Creature>();
        
        
		
	}
    private void CheckTarget()
    {
        dirToTarget = new Vector2(transform.position.x - targetTransform.position.x, transform.position.y - targetTransform.position.y);
        dirToTargetX = -Mathf.Clamp( (transform.position.x - targetTransform.position.x)*10, -1, 1);
        
        isNearTarget = Mathf.Abs(transform.position.x - targetTransform.position.x) <= 0.5;
        isAggro = Mathf.Abs(transform.position.x - targetTransform.position.x) <= 3;
        
   
    }
    // Update is called once per frame
    void Update () {

        if (character == null)
        {

            character = FindObjectOfType<Character>();
            targetGamebject = character.gameObject;
            targetTransform = targetGamebject.transform;
        }
        else
        {
            CheckTarget();
        }

        if (isAggro && !isNearTarget)
        {

            creature.Move(dirToTargetX, dirToTargetY, false, false, false);
        }
        else
            creature.Move(0, 0, false, false, false);


    }
}
