using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerEnd : MonoBehaviour
{
    public bool TimeEnd;
    private bool defaultTime = false;
    private void OnEnable()
    {
        TimeEnd = defaultTime;
        StartCoroutine(IsTimeEnd());
    }

    IEnumerator IsTimeEnd()
    {
        while(TimeEnd == false)
        {
            yield return new WaitForSeconds(1);
        }
        if (TimeEnd == true)
        {
            MirrorNetworking.UILobby.instance.TimeEnd = true;
            gameObject.SetActive(false);
            //MirrorNetworking.Player.localPlayer.uiLobby.quickMatchPanel.SetActive(false);
            yield return null;
        }

    }
}
