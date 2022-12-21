using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class quickFindNewOpponent : MonoBehaviour
{
    Button button;
    string findOpponentScene = "OnlineMode";
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            MirrorNetworking.PlayerLobbyController.localPlayer.DisconnectGame();
            SceneController.instance.BackToLoadedScene(findOpponentScene);
            quickFind();

        });

    }
    public void quickFind()
    {
        MirrorNetworking.UILobby.instance.OnlineQuickMatch.gameObject.GetComponent<Button>().onClick.Invoke();
    }
}
