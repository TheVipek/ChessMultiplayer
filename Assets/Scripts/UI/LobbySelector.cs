using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


    public class LobbySelector : MonoBehaviour
    {
        [SerializeField] public TMP_Text code;
        [SerializeField] public Button btn;
        
        [SerializeField] public TMP_Text lobbyName;
        [SerializeField] public TMP_Text lobbyPlayers;
    void Start()
        {
        btn.onClick.AddListener(SetlobbyToGo);
        }

        public void SetlobbyToGo()
        {
            MirrorNetworking.UILobby.instance.LobbyJoinButton.interactable = true;
            MirrorNetworking.UILobby.instance.wantGoLobbyID = code.text;
            
        }
    }

