using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class toggleChecker : MonoBehaviour
{
    public Toggle toggle;
    public Dropdown dropdown;
    void Start()
    {
        toggle.onValueChanged.AddListener((_isOn) => { InteractionAfterChange(_isOn, dropdown); }); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InteractionAfterChange(bool _isOn,Dropdown _dropdown)
    {
        if(_isOn == true)
        {
            _dropdown.interactable = true;
        }
        else
        {
            _dropdown.interactable = false;
        }
    }
}
