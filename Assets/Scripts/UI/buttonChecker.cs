using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class buttonChecker : MonoBehaviour
{
    Button btn;
    public TMP_Text textToShow;
    private void Awake()
    {
       TryGetComponent(out btn);
    }
    void Start()
    {
        if (btn != null && textToShow != null )
        {
            btn.onClick.AddListener(() => ShowHideText(textToShow));


        }
    }

    public void ShowHideText(TMP_Text text)
    {
        if(text.enabled == true)
        {
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
        }
    }
}
