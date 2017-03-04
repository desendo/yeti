using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class Item : MonoBehaviour
{
    //слушание
    public int transformationIndex;
    //private  UnityAction itemListener;
    public string interactionName, humanName,defaultActionResult;
    // Use this for initialization
	public Character char1;
	public Collider2D cl;
	public	TypeOf tip;
    public string name1;
    public GameObject backpack;

    private void Awake()
	{
        
       // itemListener = new UnityAction(Func);
		name1 = gameObject.name;
        if (transformationIndex==0) transformationIndex = 20;
        backpack = GameObject.Find("/backpack");
        //MasterScript.GM.backpack.gameObject
    }

    void OnEnable()
    {
        //EvntMngr.StartListening(name1, itemListener);
        //Debug.Log("Начал послушание " + name1);
        
    }

    public void Func()
    {
		//Debug.Log("Acted " + name1);
    }
    void OnDisable()
    {
       // EvntMngr.StopListening(name1, itemListener);
      //  Debug.Log("Окончил послушание "+ name1);

    }

	public virtual void DefaultAction()
	{
        AddToBackPack();

    }
    void AddToBackPack()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.parent = backpack.transform;
        gameObject.layer = 5;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * Camera.main.orthographicSize / transformationIndex, gameObject.transform.localScale.y * Camera.main.orthographicSize / transformationIndex, 1);
        transform.position = new Vector3(0, 8, -10) + backpack.transform.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        BackPackHUD bp = (BackPackHUD)backpack.GetComponent(typeof(BackPackHUD));
        bp.AddItemTo(gameObject);
        bp.changed = true;
    }
    public void DropToGround()
    {

        gameObject.transform.parent = null;
        gameObject.transform.position = FindObjectOfType<MasterScript>().yetiObject.transform.position+ new Vector3(FindObjectOfType<MasterScript>().yetiChar1.dir*3, 9,0);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x / Camera.main.orthographicSize * transformationIndex, gameObject.transform.localScale.y / Camera.main.orthographicSize * transformationIndex, 1);
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.layer = 10;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

	public enum TypeOf
	{
		Eat, Material, Furniture, Interactible, Wearable, Garbage
		
	}
}
