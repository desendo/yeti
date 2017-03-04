using UnityEngine;
using System.Collections;

public class SpecificObject : SaveableObject {

    [SerializeField]
  private float speed;
    [SerializeField]
    private float strenght;
    
	// Update is called once per frame
	void Update () {
        
        

    }
    public override void Save(int id)
    {
        statsToSave = speed.ToString() + ";" + strenght.ToString();
        base.Save(id);
    }
    public override void Load(string[] values)
    {
        speed = float.Parse(values[4].Split(';')[0]);
        strenght = float.Parse(values[4].Split(';')[1]);
        base.Load(values);
    }
}
