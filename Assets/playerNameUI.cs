using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;



public class playerNameUI : NetworkBehaviour
{
    [SyncVar] public Color32 bgColor;
    [SyncVar] public Color32 textColor;
    [SyncVar] public Color32 imgColor;
    [SyncVar] public string nickname;
    public Image bg;
    public Text textField;
    public Image img;

    private void Awake()
    {
        if (SceneController.instance.activeScene() != "OnlineGame")
        {
            this.enabled = false;
        }
    }
    void Start()
    {
        setSettings();
    }
    public void setSettings()
    {
        bg.color = bgColor;
        textField.color = textColor;
        img.color = imgColor;
        textField.text = nickname;
    }
    void Update()
    {
        
    }
}
