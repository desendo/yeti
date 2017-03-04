using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.SceneManagement;



public enum StrikeMoveType
{
    impact,poke, swing,bullet
}
public enum HitType
{
    pierce, blunt, slash, cut,fire,cold,poison,death
}
public enum WeaponPreset
{
    custom,fist, spear, sword, axe,club
}
public class DamageChunk
{
    public Collider2D collider;
    public Collision2D collision;
    public int type;
    public float damage;
    public StrikeMoveType strikeMoveType;
    public HitType hitType;




    public DamageChunk(float damage, Collider2D col, StrikeMoveType strikeMoveType, HitType hitType )
    {
        this.strikeMoveType = strikeMoveType;
        this.hitType = hitType;
        this.damage = damage;
        this.collider = col;

    }
    public DamageChunk(float damage, Collision2D col1, StrikeMoveType strikeMoveType, HitType hitType)
    {
        this.strikeMoveType = strikeMoveType;
        this.hitType = hitType;
        this.damage = damage;
        this.collision = col1;

    }
}

public class MasterScript : MonoBehaviour {


    public static MasterScript GM;

    private static string prevScene = "";
    public Character yetiChar1;
	public GameObject yetiObject;
	
    public Text mainNotice;
    public CameraController camCtrl;
    public Text itemNotice;
   // public Text inv;
    public Image itemNotice_back;
    private GameObject gm;

    public BackPackHUD backpack;

    // Use this for initialization

    public class Inventory
    {

    }

    void Awake()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }


        yetiObject = GameObject.Find("CharPrefab");
        gm = GameObject.Find("_GM");
        backpack = FindObjectOfType<BackPackHUD>();
        mainNotice = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();       
        itemNotice = gameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();
        itemNotice_back = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        if (SceneManager.GetActiveScene() == null)
            SceneManager.LoadScene(PlayerPrefs.GetString("SCENE"));
        camCtrl = GameObject.FindObjectOfType<CameraController>();
    }
    void Start()
    {
        
            yetiChar1 = GameObject.FindObjectOfType<Character>();
            Debug.Log(yetiChar1.name);

    }
    void Update()
    {
        if (yetiChar1 == null)
            yetiChar1 = GameObject.FindObjectOfType<Character>();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
       
        if (yetiChar1 ==null)
        yetiChar1 = GameObject.FindObjectOfType<Character>();

       mainNotice.text = "";
        MoveChar();
        if(backpack ==null)
        backpack = FindObjectOfType<BackPackHUD>();
    }
    public void MoveChar()
    {
      
        if (prevScene != "" && prevScene != "menu")
        {
            string spawnPointName = "SpawnPoint_from_"+ prevScene;
            yetiChar1.transform.position = GameObject.Find(spawnPointName).transform.position;
        }
        else
        {
            yetiChar1.transform.position = GameObject.Find("SpawnPoint").transform.position;
        }

        camCtrl.MoveToXY(yetiChar1.transform);
     }


    public void PositionChar(string scene)

    {

    }
    public string GetScene()

    {
        return SceneManager.GetActiveScene().name;
    }
    public void LoadScene(string scene)
    {


        
        if (scene != "")
        {

          

            prevScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
     

        }
        else
            SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }


}
