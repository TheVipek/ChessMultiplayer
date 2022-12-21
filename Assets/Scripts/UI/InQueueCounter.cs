using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InQueueCounter : MonoBehaviour
{
    [SerializeField] float timeInLobbySetter;
    [SerializeField] float timeInLobby;
    [SerializeField] Text textComponent;
    [SerializeField] bool running = false;
    [SerializeField] bool increasing;

    private void OnEnable()
    {
        timeInLobby = timeInLobbySetter;
        timeStart();
    }
    // Update is called once per frame
    void Update()
    {
        if(running == true)
        {
            if(increasing == true)
            {
                timeRefreshingIncrease();
                DisplayTime(timeInLobby);

            }
            else
            {
                timeRefreshingDecrease();
                if (timeInLobby < 0)
                {
                    timeInLobby = 0;
                    GetComponent<timerEnd>().TimeEnd = true;
                    timeStop();
                }
                DisplayTime(timeInLobby);

            }

        }
    }
    public void timeStart()
    {
        running = true;
    }

    public void timeStop()
    {
        running = false;
    }
    public void resetTimer()
    {
        timeInLobby = 0;
    }
    public void timeRefreshingIncrease()
    {
        timeInLobby += Time.deltaTime;
    }
    public void timeRefreshingDecrease()
    {
        timeInLobby -= Time.deltaTime;
    }
    public void DisplayTime(float time_left)
    {
        float minutes = Mathf.FloorToInt(time_left / 60);
        float seconds = Mathf.FloorToInt(time_left % 60);
        textComponent.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
