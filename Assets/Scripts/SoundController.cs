using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource source;
    private AudioClip chess_place, ui_interaction;

    public static SoundController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }
    void Start()
    {
        chess_place = Resources.Load<AudioClip>("chess_place");
        ui_interaction = Resources.Load<AudioClip>("ui_interaction");
        //Debug.Log(chess_place.name);
    }

    public void placeChessSound()
    {
        source.PlayOneShot(chess_place);
    }
    public void uiInteractionSound()
    {
        source.PlayOneShot(ui_interaction);
    }
}
