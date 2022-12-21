using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropdownController : MonoBehaviour
{
    public Dropdown dropdown;
    void Start()
    {
        
    }

    public void setTime()
    {
        int index = dropdown.value;
        List<Dropdown.OptionData> options = dropdown.options;
        int time = int.Parse(options[index].text);
        GameManager.instance.SetGameLength(time*60);
    }
}
