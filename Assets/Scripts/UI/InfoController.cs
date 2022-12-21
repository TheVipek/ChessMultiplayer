using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoController : MonoBehaviour
{
    public Text text;
    public Image image;
    void Start()
    {
        if(gameObject.tag == "White")
        {
            text.text = "Player1";
        }
        else
        {
            text.text = "Player2";


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
