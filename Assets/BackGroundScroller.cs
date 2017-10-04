using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour {

    // Use this for initialization
    Vector2 offset;
    public float speed;
    Renderer rend;
    Transform cameraTransform;
    public float x, y, z;
   public bool isClouds;


    Transform tr;
	void Start () {
		rend =  gameObject.GetComponent<Renderer>();
        
        cameraTransform = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
       if(isClouds)
       offset = new Vector2(Time.time * speed, y);
       else
        offset = Camera.main.ViewportToWorldPoint(new Vector3(x, y, z)) * speed;
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
       
        rend.material.mainTextureOffset = offset;
	}
}
