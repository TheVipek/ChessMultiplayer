using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class DestroyedListController : NetworkBehaviour
{
    [SyncVar] public Color bgColor;
    [SyncVar] public Color childBgColor;
    [SyncVar] public string objName;
    public GameObject destroyed_tiles;
    public Text text;
    public Image bgImg;
    public Image bgChildImage;
    private int numberNotVisible = 0;
    void Start()
    {
        SetSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed_tiles.transform.childCount > 4)
        {
            Destroy(destroyed_tiles.transform.GetChild(0).gameObject);
            numberNotVisible += 1;
            UpdateText();
        }
    }
    public void UpdateText()
    {
        text.text = "+" + numberNotVisible.ToString();
    }
    public void SetSettings()
    {
        bgImg.color = bgColor;
        bgChildImage.color = childBgColor;
        gameObject.name = objName;
    }
}
