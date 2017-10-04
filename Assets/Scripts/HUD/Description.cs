using UnityEngine;
using UnityEngine.UI;


public class Description : MonoBehaviour {

    GameObject[] interactNotice;
    Image[] interactNoticeBG;
    Text[] interactNoticeText;

    public void UpdateDesriptionAboutIntracibleObject(GameObject intracibleObject)
    {
        
        for (int i = 0; i < interactNotice.Length; i++)
        {
            interactNoticeText[i].text = "";
            interactNoticeBG[i].enabled = false;


        }
        if (intracibleObject!=null)
        {
            string interactionName="";
            //interactNoticeBG[0].enabled = true;
            
            //interactionName = "использовать";//todo translate

            //interactNoticeText[0].text += intracibleObject.name + " (E - " + interactionName + ")" + "\n";
           // interactNoticeBG[0].enabled = true;
            if (intracibleObject.GetComponent<PickableItem>().isActiveAndEnabled)
            {
                interactionName = "подобрать";//todo translate
                interactNoticeText[0].text +=  intracibleObject.name + " (E - " + interactionName + ")";
                interactNoticeBG[0].enabled = true;
            }
            

        }
        
    }
    void Awake()
    {
       
         //+= UpdateDesription(gObj);
       EventManager.onNearestObjectAppear += new EventManager.DestriptionHandler((gObj) => UpdateDesriptionAboutIntracibleObject(gObj));
        EventManager.onNearestObjectDisappear += new EventManager.DestriptionHandler((gObj) => UpdateDesriptionAboutIntracibleObject(gObj));

    }

    // Use this for initialization
    void Start () {
        try
        {
            interactNotice = GameObject.FindGameObjectsWithTag("InteractionTip");

            interactNoticeBG = new Image[interactNotice.Length];
            interactNoticeText = new Text[interactNotice.Length];
            for (int i = 0; i < interactNotice.Length; i++)
            {
                interactNoticeBG[i] = interactNotice[i].GetComponent<Image>();
                interactNoticeText[i] = interactNotice[i].transform.GetChild(0).GetComponent<Text>();
            }

        }
        catch
        {
            Debug.Log("не найден объект для отображения подсказки");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
