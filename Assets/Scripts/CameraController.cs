using UnityEngine;
using System;
using System.Collections;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private float speed = 0.005F;
    [SerializeField]
	private Transform target;
    [SerializeField]
    private bool shouldFollow = false;
    public bool walkingMode= false;
    public Transform trnsfrm;
    public float cruiseModeSecsToStart = 0;
    public float cruiseModeDelay = 10.0F;
    public bool isCruiseMod;

    public Vector3 zSpace;
    public Vector3 ySpace;
    public float zoomSpeed = 4;
    public float targetOrtho;
    public float smoothSpeed = 6.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 32.0f;


    private void Awake()
	{
        DontDestroyOnLoad(transform.gameObject); // позволяет переходить персонажу без изменений из уровня в уровень
        if (FindObjectsOfType(GetType()).Length > 1) //уничтожает клон если таковой появится при телепортации. жестокость нового уровня.
            Destroy(gameObject);

        if (FindObjectOfType<Character>()!=null)
            target = FindObjectOfType<Character> ().transform;
		else
			target = gameObject.transform;

        trnsfrm = transform;
        ySpace = new Vector3(0, -15, 0);
        
    }
    void Start()
    {
        zSpace = new Vector3(0, 0, target.position.z - transform.position.z)+ ySpace;
        targetOrtho = Camera.main.orthographicSize;
    }
    public void MoveToXY(Transform tr)
    {
        transform.position = new Vector3(tr.position.x, transform.position.y, transform.position.z);
    }
    bool wasSitting;
    private void Update()
    {
       
      //  float scroll = Input.GetAxis("Mouse ScrollWheel");
      //  if (scroll != 0.0f)
     //   {
      //      targetOrtho -= scroll * zoomSpeed;
      //      targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
     //   }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    


    isCruiseMod = cruiseModeSecsToStart > cruiseModeDelay;
        if (!target) {
            if (FindObjectOfType<Character>() != null)
                target = FindObjectOfType<Character>().transform;
            else
                target = gameObject.transform;
        }

            if ((transform.position - new Vector3(0, 0, transform.position.z - target.position.z) - target.position).magnitude > 15) shouldFollow = true;
            if ((transform.position - new Vector3(0, 0, transform.position.z - target.position.z) - target.position).magnitude < 2) shouldFollow = false;

        if (shouldFollow)
                     transform.position = Vector3.Lerp(transform.position, target.position -zSpace, speed * Time.deltaTime);

        if (Input.GetButton("Vertical") && Input.GetAxis("Vertical") <= -0.01F)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + 2 * ySpace, speed * Time.deltaTime);
            wasSitting = true;
        }
        else if (wasSitting)
        {
            wasSitting = false;
            Debug.Log("Шишка");
            shouldFollow = true;
            transform.position = Vector3.Lerp(transform.position, target.position - zSpace, speed * Time.deltaTime);

        }



        if (Input.GetButton("Horizontal"))
        {
            cruiseModeSecsToStart += Time.deltaTime;
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            cruiseModeSecsToStart = 0;
        }
    }
}

