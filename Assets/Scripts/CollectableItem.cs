using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeableItem : PickableItem {




    private void Awake()
    {

        print("awaken CollectibleItem");

    }

    void OnEnable()
    {


    }

    public void Func()
    {
    }
    void OnDisable()
    {

    }

    
    void AddToBackPack()
    {

    }
    public void DropToGround()
    {


    }

    public enum TypeOf
    {
        Eat, Material, Furniture, Interactible, Wearable, Garbage

    }
}
