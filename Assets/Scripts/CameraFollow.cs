using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow camFol;
    private Vector2 velocity;
    public float smoothTimeX, smoothTimeY;
    public GameObject objectToFollow;
    public float CameraYShift;
    private Vector2 pos,diff;
    public bool followGroundedOnly;
    bool isFollowinCharacter;
    Character character;
    Vector3 position;
    public float tolerance;

    //Camera cam;
	void Start () {
        position = transform.position;
        if (objectToFollow == null)
        {
            if (objectToFollow = GameObject.FindGameObjectWithTag("Player"))
            {
                Debug.Log("Объект для камеры найден");
            }
            else
            {
                Debug.LogError("нет объекта для следования");
                objectToFollow = gameObject;
            }

        }
        CheckIfCharacter();

      //  cam = gameObject.GetComponent<Camera>();
        camFol = GetComponent<CameraFollow>();

    }
 
    void CheckIfCharacter()
    {
       if (character = objectToFollow.GetComponent<Character>())
            {
            isFollowinCharacter = true;            
            }

    }
	
    void Update()
    {
        //cam.orthographicSize = 0.5f * Screen.height / (PPUScale * pixelsPerUnit);
       

    }
    // Update is called once per frame
    void FixedUpdate () {

        UpdateFollowing(objectToFollow);

    }
    public void UpdateFollowing(GameObject objectToFollow_l)
    {
        
        pos = new Vector2(
        Mathf.SmoothDamp(position.x, objectToFollow_l.transform.position.x, ref velocity.x, smoothTimeX),
        Mathf.SmoothDamp(position.y, objectToFollow_l.transform.position.y, ref velocity.y, smoothTimeY));
            if (followGroundedOnly)
            {
                if (isFollowinCharacter && character.grounded)
                {

                    position = new Vector3(pos.x, pos.y, transform.position.z);
                }
                else

                    position = new Vector3(Mathf.Round(pos.x*100)/100, Mathf.Round(position.y*100)/100, transform.position.z);
            }
            else
                //position = new Vector3(pos.x, pos.y, transform.position.z);
                position = new Vector3(Mathf.Round(pos.x * 100) / 100, Mathf.Round(pos.y * 100) / 100, transform.position.z);

        transform.position = position + new Vector3(0, CameraYShift);
        
    }
}
