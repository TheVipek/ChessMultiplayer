using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MirrorNetworking
{
    public class UILobby : MonoBehaviour
    {
        [Header("Host Join")]
        public static UILobby instance;
        [SerializeField] TMP_InputField joinMatchInput;
        [SerializeField] List<Selectable> CustomMatchEntry = new List<Selectable>();
        [SerializeField] GameObject lobbyCanvas;
        [SerializeField] GameObject searchCanvas;
        [SerializeField] GameObject quickMatchCanvas;
        [Header("canvas gameobjects for reference")]
        public GameObject Main;
        public GameObject OnlineQuickMatch;
        [Header("Lobby Settings")]
        public TMP_Text lobbyName;
        public Dropdown matchLength;
        public Toggle isTimeForMovEnabled;
        public Dropdown timeForMove;
        public Image publicMatchImg, privateMatchImg;

        [Header("Quick Match")]
        public GameObject quickMatchPanel;
        public GameObject quickMatchWaitingBox;
        public bool TimeEnd = false;
        [Header("Lobby")]
        public Transform UIPlayerParent;
        public GameObject UIPlayerPrefab;
        [SerializeField] TMP_Text matchIDText;
        [SerializeField] TMP_Text lobbyNameText;
        [SerializeField] public GameObject beginGameButton;
        [SerializeField] GameObject readyGameButton;

        //public GameObject playerLobbyUI;
        [Header("ListOfLobbies")]
        public string wantGoLobbyID = string.Empty;
        [SerializeField] public Button LobbyExitButton;
        [SerializeField] public Button LobbyJoinButton;
        [SerializeField] public Button LobbyRefreshButton;
        [SerializeField] public Transform serverBoxParent;
        [SerializeField] public GameObject serverBox;


        public struct matchSettings
        {
            public matchSettings(string _name, float _length, bool _moveEnabled, float _timeForMove, bool _isPublic)
            {
                name = _name;
                length = _length;
                moveEnabled = _moveEnabled;
                timeForMove = _timeForMove;
                isPublic = _isPublic;
            }
            public string name { get; }
            public float length { get; }
            public bool moveEnabled { get; }
            public float timeForMove { get; }
            public bool isPublic { get; }
        }
        bool searching = false;
        public matchSettings settings;

        void Start()
        {
            instance = this;
        }

        void Update()
        {
        
        }
      
        
        public void HostGame()
        {
            joinMatchInput.interactable = false;
            CustomMatchEntry.ForEach(x => x.interactable = false);
            settings = new matchSettings(lobbyName.text,
                    float.Parse(matchLength.options[matchLength.value].text),
                    isTimeForMovEnabled.isOn,
                    float.Parse(timeForMove.options[timeForMove.value].text),
                    publicMatchImg.enabled);
            PlayerLobbyController.localPlayer.HostGame();


        }
        public void HostSuccess(bool success, string match_id,string match_name)
        {
            if (success)
            {
                lobbyCanvas.SetActive(true);
               // ClearLobby(UIPlayerParent);
                if (PlayerLobbyController.localPlayer.playerLobbyUI != null)
                {
                    Destroy(PlayerLobbyController.localPlayer.playerLobbyUI.gameObject);
                }


                PlayerLobbyController.localPlayer.SpawnPlayerUI();


                //playerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer);
                beginGameButton.GetComponent<Button>().interactable = false;
                matchIDText.text = match_id;
                lobbyNameText.text = match_name;

            }
            else
            {
                joinMatchInput.interactable = true;
                CustomMatchEntry.ForEach(x => x.interactable = true);

            }
        }
        public void ClearLobby(Transform lobbyBox)
        {
            foreach (Transform child in lobbyBox.transform)
            {
                Debug.Log(child.gameObject);
                Destroy(child.gameObject);
            }
        }
        public void Join()
        {
            joinMatchInput.interactable = false;
            CustomMatchEntry.ForEach(x => x.interactable = false);
            PlayerLobbyController.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }
        public void JoinTo()
        {
            LobbyExitButton.interactable = false;
            LobbyJoinButton.interactable = false;
            if(wantGoLobbyID != string.Empty)
            {
                PlayerLobbyController.localPlayer.JoinGame(wantGoLobbyID.ToUpper());
                wantGoLobbyID = string.Empty;
            }
        }
       
        public void JoinSuccess(bool success,string match_id, string match_name)
        {
            if (success)
            {
                lobbyCanvas.SetActive(true);
                //beginGameButton.SetActive(false);
                //ClearLobby(UIPlayerParent);
                
                if (PlayerLobbyController.localPlayer.playerLobbyUI != null)
                {
                    Destroy(PlayerLobbyController.localPlayer.playerLobbyUI.gameObject);
                }

                PlayerLobbyController.localPlayer.SpawnPlayerUI();
                beginGameButton.SetActive(false);
                //playerLobbyUI = SpawnPlayerUIPrefab();

                matchIDText.text = match_id;
                lobbyNameText.text = match_name;
            }
            else
            {
                joinMatchInput.interactable = true;
                CustomMatchEntry.ForEach(x => x.interactable = true);

            }
            LobbyExitButton.interactable = true;
        }
        public void SpawnPlayerUIPrefab(PlayerLobbyController player)
        {
            player.SpawnPlayerUI();
            //GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            //newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player);
            //NetworkServer.Spawn(newUIPlayer);
            //newUIPlayer.transform.SetSiblingIndex(player.playerIndex-1);
            //player.playerLobbyUI = newUIPlayer;
            //player.SetPlayerUI(player,newUIPlayer);
            //return newUIPlayer;
        }
        public void BeginGame()
        {
            PlayerLobbyController.localPlayer.BeginGame();
        }
        public void SearchGame()
        {
            Debug.Log("Searching for game");
            searchCanvas.SetActive(true);
            StartCoroutine(SearchingForGame());
        }
        public IEnumerator SearchingForGame()
        {
            searching = true;
            WaitForSeconds checkEveryFewSeconds = new WaitForSeconds(1);
            while (searching)
            {
                yield return checkEveryFewSeconds;
                if (searching)
                {
                    PlayerLobbyController.localPlayer.SearchGame(); 
                }
            }
        }
        public void SearchSuccess(bool success, string match_id,string matchName)
        {
            if (success)
            {
                searchCanvas.SetActive(false);
                JoinSuccess(success, match_id, matchName);
                searching = false;
            }
        }
        public void SearchCancel()
        {
            searchCanvas.SetActive(false);
            searching = false;
            CustomMatchEntry.ForEach(x => x.interactable = true);
        }
        public void DisconnectLobby()
        {
            if (PlayerLobbyController.localPlayer.playerLobbyUI != null)
            {
                Destroy(PlayerLobbyController.localPlayer.playerLobbyUI);
            }
            PlayerLobbyController.localPlayer.DisconnectGame();
            lobbyCanvas.SetActive(false);
            CustomMatchEntry.ForEach(x => x.interactable = true);
            //beginGameButton.SetActive(false);

        }
        public void LookForMatches()
        {
            RefreshList();
        }
        public void RefreshList()
        {
            LobbyExitButton.interactable = false;
            LobbyJoinButton.interactable = false;
            LobbyRefreshButton.interactable = false;
            StartCoroutine(PlayerLobbyController.localPlayer.RefreshList());
        }

        public void ReadyUpdate()
        {
            PlayerLobbyController.localPlayer.ReadyStateUpdate();
        }
        public void UIReadyChange(PlayerLobbyController _player)
        {
            //_player.playerLobbyUI.GetComponent<PlayerUI>().SetReady();
            _player.playerLobbyUI.GetComponent<PlayerUI>().setVisibility();

        }
        public void PassLeaderDefault()
        {
            PlayerLobbyController.localPlayer.PassLeaderDefault();
        }
        
        public void getIntoQueue()
        {
            PlayerLobbyController.localPlayer.QueueHostGame();
           // QueueController.instance.getIntoQueue(Player.localPlayer.gameObject);
        }
        public void DisplayTimeToGame()
        {
            
            quickMatchWaitingBox.SetActive(false);
            quickMatchPanel.SetActive(true);
        }
        public IEnumerator counterEnded(PlayerLobbyController player)
        {
            while (TimeEnd == false)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (TimeEnd == true)
            {

                Debug.Log(player.nickname + " - going into game...");
                player.RpcStartGame("OnlineGame",player.matchID,player);
                TimeEnd = false;
                yield return null;
            }
        }
        public void leaveQueue()
        {
            Debug.Log("Triggering leave queue");
            PlayerLobbyController.localPlayer.OnLeaveQueue();
        }
    }

}
