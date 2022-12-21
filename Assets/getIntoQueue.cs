using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getIntoQueue : MonoBehaviour
{
    public InQueueCounter queueCounter;
    private void Start()
    {
        Debug.Log("Starting time , getting int oqeueu");
        MirrorNetworking.UILobby.instance.getIntoQueue();
        queueCounter.timeStart();
    }

}
