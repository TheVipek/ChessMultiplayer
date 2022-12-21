using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //public static Tile instance;
    //private bool instance_created;
   /* void Awake()
    {
        if (instance_created == false)
        {
            instance = this;
            instance_created = true;
        }
        else
        {
            Destroy(this);
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }


    public string GetCurrentName()
    {
        return gameObject.name;
    }
    public int GetCurrentIndex(GameObject tile)
    {
        return BoardManager.instance.tileList.IndexOf(tile);
    }
}
