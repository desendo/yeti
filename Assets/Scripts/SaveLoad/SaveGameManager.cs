using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class PPSerialization
{
    public static BinaryFormatter binaryFormatter = new BinaryFormatter();
    public static void Save(string saveTag, object obj)
    {

        MemoryStream memory = new MemoryStream();
        binaryFormatter.Serialize(memory, obj);
        string temp = System.Convert.ToBase64String(memory.ToArray());
        PlayerPrefs.SetString(saveTag, temp);

    }

    public static object Load(string saveTag)

    {
        string temp = PlayerPrefs.GetString(saveTag);
        if (temp == string.Empty)
        {
            return null;
        }
        MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(temp));
        return binaryFormatter.Deserialize(memoryStream);


    }
        
    

}
public class SaveGameManager : MonoBehaviour {


    private static SaveGameManager instance;
    public List<SaveableObject> SaveableObjects { get; private set; }
    

    public static SaveGameManager Instance
    {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<SaveGameManager>();

            return instance;
        }

        set {instance= value; }
    }
    void Awake()
    {
        SaveableObjects = new List<SaveableObject>();
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        
	
	}
    public void Save()
    {

        PlayerPrefs.SetInt(MasterScript.GM.GetScene(), SaveableObjects.Count);
        for (int i = 0; i < SaveableObjects.Count; i++)
        {
            SaveableObjects[i].Save(i);
        }
        
    }
    public void BinarySave()
    {

        PlayerPrefs.SetInt(MasterScript.GM.GetScene(), SaveableObjects.Count);
        for (int i = 0; i < SaveableObjects.Count; i++)
        {
            SaveableObjects[i].Save(i);
        }

    }
    public void Load()
    {

        foreach (SaveableObject obj in SaveableObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
        SaveableObjects.Clear();
        int objectCount = PlayerPrefs.GetInt(MasterScript.GM.GetScene(), SaveableObjects.Count);
        for (int i = 0; i < objectCount; i++)
        {
            string[] values = (PlayerPrefs.GetString(MasterScript.GM.GetScene() + "-" + i.ToString())).Split('_') ;
            //  Debug.Log(value);

            try
            {
                GameObject tmp = Instantiate(Resources.Load(values[1]) as GameObject);
                if (tmp != null)

                    tmp.GetComponent<SaveableObject>().Load(values);
            }
            catch
            {
                Debug.LogError("Префаб объекта отсутствует в ресурсах :"+ values);
            }
            
        }
    }
    public void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
    }
    public Vector3 StringToVector3(string position)
    {
        position = position.Trim(new char[] { '(', ')' });
        position = position.Replace(" ", "");
        string[] axis = position.Split(',');
        return new Vector3(float.Parse(axis[0]), float.Parse(axis[1]), float.Parse(axis[2]));

        

        
    }
    public Quaternion StringToQuaternion(string rotation)

    {
        rotation = rotation.Trim(new char[] { '(', ')' });
        rotation = rotation.Replace(" ", "");
        string[] axis = rotation.Split(',');
        return new Quaternion(float.Parse(axis[0]), float.Parse(axis[1]), float.Parse(axis[2]), float.Parse(axis[3]));
        //return Quaternion.identity;

    }

}
