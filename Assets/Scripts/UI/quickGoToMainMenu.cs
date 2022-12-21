using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class quickGoToMainMenu : MonoBehaviour
{
    Button button;
    public string wantGoScene;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(() => 
        {
            // change active scene to previous and unload this
            SceneController.instance.BackToLoadedScene(wantGoScene);
            //set main menu active at MainMenu Scene
            quickMainMenu();
            //trigger disconnect game
            MirrorNetworking.PlayerLobbyController.localPlayer.DisconnectGame();
        });

    }

    public void quickMainMenu()
    {
        MirrorNetworking.UILobby.instance.Main.SetActive(true);

    }
    
}
