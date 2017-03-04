using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using System.Collections;

public class door : Item
{	
	[SerializeField]
	string scene;
   // private UnityAction itemListener;

    void Awake()
	{
        

        tip = TypeOf.Interactible;
        name1 = "дверь в дом";
        //itemListener = new UnityAction(Func);
    }


	public override void DefaultAction()
	{
        

       // char1.RemoveItemFromSight (this);

        GameObject.Find("_GM").GetComponent<MasterScript>().LoadScene(scene);
        //EvntMngr.eventManager.ClearThis();
        GameObject.Find("_GM").GetComponent<MasterScript>().PositionChar(scene);
		//Destroy(gameObject);

    }
}
