using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class timerBoxUI : NetworkBehaviour
{
    [SyncVar] public Color32 bgColor;
    [SyncVar] public Color32 textColor;
    [SyncVar (hook =nameof(TimeLeftChanged))] public float time_left;
    public Image bg;
    public Text textField;

    private void Awake()
    {
        if (SceneController.instance.activeScene() != "OnlineGame")
        {
            this.enabled = false;
        }
    }
    private void Start()
    {
        SetSettings();
    }
    public void SetSettings()
    {
        bg.color = bgColor;
        textField.color = textColor;
    }
    void Update()
    {
        
    }
    public void TimeLeftChanged(float oldVal,float newVal)
    {
        
        textField.GetComponent<TimeRefresher>().DisplayTime(newVal);
    }

}
