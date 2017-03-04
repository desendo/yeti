using UnityEngine;
using System.Collections;


enum ObjectType { rock, meat,skeleton}
public abstract class SaveableObject : MonoBehaviour {

    protected string statsToSave;
    [SerializeField]
    private ObjectType objectType;
    public bool  isBasicSaveEnabled=false;
	// Use this for initialization
	void Start () {
      //  if(isActiveAndEnabled)
      //  SaveGameManager.Instance.SaveableObjects.Add(this);
        
    }
    public virtual void Save(int id)
    {
        PlayerPrefs.SetString(MasterScript.GM.GetScene()+"-"+ id.ToString(), transform.position.ToString()+ "_" + objectType +"_"+transform.localScale + "_" + transform.localRotation+"_"+statsToSave);

    }

    public virtual void Load(string[] values)
    {

        transform.localPosition = SaveGameManager.Instance.StringToVector3(values[0]);
        transform.localScale = SaveGameManager.Instance.StringToVector3(values[2]);
        transform.localRotation = SaveGameManager.Instance.StringToQuaternion(values[3]);
    }
    public void DestroySaveable()
    {

    }
	// Update is called once per frame
	void Update () {
	
	}
    void OnDestroy()
    {
      //  if (isActiveAndEnabled)
        {
            try
            {
              //  SaveGameManager.Instance.SaveableObjects.Remove(this);
            }
            catch
            {
            }
        }
    }
}
