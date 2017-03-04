using UnityEngine;

using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EvntMngr : MonoBehaviour {

    public Dictionary<string, UnityEvent> eventDict;
    public static EvntMngr eventManager;
    public static EvntMngr instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EvntMngr)) as EvntMngr;
                if (!eventManager)
                {
                    Debug.LogError("No EventManager");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;     
        }
    }
    void Init()
    {
        if (eventDict == null)
        {
            eventDict = new Dictionary<string, UnityEvent>();
        }
    }
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDict.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            
            thisEvent.AddListener(listener);
            instance.eventDict.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {   
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
       // Debug.Log(listener.ToString());
        if (instance.eventDict.TryGetValue(eventName, out thisEvent))
        {
           // thisEvent.RemoveListener(listener);
        }

    }
    public void ClearThis()
    {
        eventDict.Clear();
    }
    public static void TriggerEvent(string eventName)
    {
        
        UnityEvent thisEvent = null;
        if (instance.eventDict.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log(eventName);
            Debug.Log(instance.eventDict[eventName].ToString());
            thisEvent.Invoke();
        }
    }

}
