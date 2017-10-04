using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour {

    public Camera cam;
    static Creature creature;
    public string NameOfCreatureToShowHealth;
    public GameObject camObject;
    Text DebugText;

    void Start()
    {

        DebugText = GetComponent<Text>();
        FindCamera();
        FindCreature(NameOfCreatureToShowHealth);
    }
    void FindCamera()
    {
        if (cam == null)
        {
            if (GameObject.Find("Main Camera") != null)
            {
                cam = GameObject.FindObjectOfType<Camera>();
            }
            else
            {
                Debug.Log("Камера Main Camera не найдена. Создайте или переименуйте");
            }
        }
        else
        {
            Debug.LogError("Не найдена камера");
        }
    }
    void FindCreature( string name)
    {
            
            if (GameObject.Find(name) != null)
            {
            
            GameObject creature_object = GameObject.Find(name);
            Debug.Log(creature_object.name);
            creature = creature_object.GetComponent<Creature>();
            }
            else
            {
                Debug.Log("объект с именем " +name+ " не найден");
            }

    }
    // Update is called once per frame
    void Update () {
        if (cam != null)
        {
            DebugText.text = "Высота: " + Screen.height + "\n" + "Ширина: " + Screen.width + "\n";
            DebugText.text += "Cam x: " + cam.gameObject.transform.position.x + "\n" + "Cam y: " + cam.gameObject.transform.position.y;
            
            //text.text += "\n Приземленность: " + Character.player.isGrounded.ToString();
            //text.text += "\n Скорость: " + Character.player.instanceSpeed.magnitude.ToString();
        }
        DebugText.text += "\n Здоровье of" + NameOfCreatureToShowHealth + " " + creature.health.currentHealth;

    }
}
