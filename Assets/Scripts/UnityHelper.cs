
using UnityEngine;
using System.Collections;



public static class Helper
{
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();

            }

        }
        return null;
    }
}
public class UnityHelper
{


    public static GameObject[] FindGameObjectsWithTag(string tag)
    {
        ArrayList resultList = new ArrayList();
        foreach (GameObject result in GameObject.FindGameObjectsWithTag(tag))
        {
            if (!result.name.Contains("(Clone)"))
            {
                resultList.Add(result);
            }
        }

        GameObject[] resultArray = new GameObject[resultList.Count];
        resultList.CopyTo(resultArray);
        return resultArray;
    }

    public static GameObject FindWithTag(string tag)
    {
        try
        {
            return FindGameObjectsWithTag(tag)[0];
        }
        catch
        {
            return null;
        }
    }
}