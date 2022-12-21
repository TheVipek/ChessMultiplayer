using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cameraCatchOnAwake : MonoBehaviour
{
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = CameraController.instance.cameraGlobal;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
