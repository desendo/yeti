using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {


    public  delegate void DestriptionHandler(GameObject gObj);
    public static event DestriptionHandler onNearestObjectAppear;
    public static event DestriptionHandler onNearestObjectDisappear;
    public static void NearestObjectAppear(GameObject gObj)
    {
        if (onNearestObjectAppear != null)
        {
            onNearestObjectAppear(gObj);
        }
    }
    public static void NearestObjectDisappear()
    {
        if (onNearestObjectDisappear!= null)
        {
            onNearestObjectDisappear(null);
        }
    }

}
