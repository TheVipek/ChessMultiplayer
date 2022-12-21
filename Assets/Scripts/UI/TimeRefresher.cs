using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeRefresher : MonoBehaviour
{
    public bool running = false;
    public bool time_over = false;
    public float timeBetweenDisappearing;
    private bool coroutine_running = false;
    private Text text_component;
    void Start()
    {
        text_component = GetComponent<Text>();
        DisplayTime(transform.parent.GetComponent<timerBoxUI>().time_left);
    }

    // Update is called once per frame
    void Update()
    {
        if(running == true)
        {
            timeRefreshing();
        }
    }


    public void timeStart()
    {
        if(time_over == false)
        {
            running = true;
        }
    }

    public void timeStop()
    {
        if (time_over == false)
        {
            running = false;    
        }
    }
    public void timeRefreshing()
    {
        if(transform.parent.GetComponent<timerBoxUI>().time_left > 0)
        {
            transform.parent.GetComponent<timerBoxUI>().time_left -= Time.deltaTime;
        }
        else
        {
            timeStop();
            transform.parent.GetComponent<timerBoxUI>().time_left = 0;
            time_over = true;
            if (coroutine_running == false)
            {
                StartCoroutine(ShowingEffect(timeBetweenDisappearing));
                coroutine_running = true;
            }
            //CALL END GAME
            MirrorNetworking.PlayerController.playerController.CmdLostDueToTime(MirrorNetworking.PlayerController.playerController);

        }
        DisplayTime(transform.parent.GetComponent<timerBoxUI>().time_left);
    }
    public void DisplayTime(float time_left)
    {
        float minutes = Mathf.FloorToInt(time_left / 60);
        float seconds = Mathf.FloorToInt(time_left % 60);
        GetComponent<Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    IEnumerator ShowingEffect(float showingtime)
    {
        for (int i = 0; i < 3; i++)
        {
            text_component.enabled = false;
            yield return new WaitForSeconds(showingtime);
            text_component.enabled = true;
            yield return new WaitForSeconds(showingtime);
        }
        yield return null;
    }



}
