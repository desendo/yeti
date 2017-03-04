using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
