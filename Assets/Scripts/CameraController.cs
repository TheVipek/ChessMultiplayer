using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera cameraGlobal;
    private void Awake()
    {
        instance = this;
        cameraGlobal = Camera.main;
    }
    void Start()
    {
    }

    
    void Update()
    {
        
    }
}
