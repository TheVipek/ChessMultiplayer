using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using TMPro;
using UnityEngine.UI;
namespace MirrorNetworking
{
    public class PlayerLobbyController : NetworkBehaviour
    {
        public static PlayerLobbyController localPlayer;
        [Header("Out of game settings")]
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;
        [SyncVar] public string nickname;
        [SyncVar] public bool inGame = false;
        [SyncVar] public Match actual_lobby = null;
        [SyncVar] public Queue actualQueue = null;
        [SyncVar] public bool isReady = false;
        [SyncVar] public bool isOwner = false;
        
        [SyncVar] public Color32 playerLobbyUIcolor;
        public GameObject playerLobbyUI;
        public UILobby uiLobby;
        public NetworkMatch networkMatch;
        Guid netIDGuid;
        // Start is called before the first frame update
        private void Awake()
        {
            networkMatch = GetComponent<NetworkMatch>();

        }
        private void Start()
        {

            uiLobby = UILobby.instance;
        }
        public override void OnStartServer()
        {
            netIDGuid = netId.ToString().ToGuid();
            networkMatch.matchId = netIDGuid;
        }
        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                localPlayer = this;
                GenerateNickname();
                DontDestroyOnLoad(this);
            }
            /*else
            {
                SpawnPlayerUI();
                playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab(this);
            }*/
        }
        public override void OnStopServer()
        {
            ServerDisconnect();
        }
        public override void OnStopClient()
        {
            ClientDisconnect();
        }
        //GENERATE NICKNAME
        [Command(requiresAuthority = false)]
        public void GenerateNickname()
        {
            nickname = GameManager.instance.gameObject.GetComponent<stringGenerator>().StringGenerate("Guest");
        }
        public void Reset()
        {
            matchID = string.Empty;
            playerIndex = 0;
            inGame = false;
            actual_lobby = null;
            actualQueue = null;
            isReady = false;
            isOwner = false;
        }
        //HOSTING STUFF
        public void HostGame()
        {
            string matchID = MatchMaker.GetRandomMatchID();
            UILobby.matchSettings createdLobbySettings = UILobby.instance.settings;

            Debug.Log("in host client");
            Debug.Log(createdLobbySettings.name);
            
            Debug.Log($"matchID:{matchID}");
            CmdHostGame(
                _matchID:matchID, 
                _publicMatch: uiLobby.publicMatchImg.enabled,
                _matchName: uiLobby.lobbyName.text,
                _matchLength: float.Parse(uiLobby.matchLength.options[uiLobby.matchLength.value].text),
                _moveEnabled:uiLobby.isTimeForMovEnabled.isOn,
                _timeForMove:float.Parse(uiLobby.timeForMove.options[uiLobby.timeForMove.value].text),
                _player:gameObject
                );
            
        }
        [Command]
        void CmdHostGame(string _matchID, bool _publicMatch,string _matchName,float _matchLength,bool _moveEnabled,float _timeForMove,GameObject _player)
        {
            matchID = _matchID;

            Debug.Log($"match id set to {matchID}");
            if (MatchMaker.instance.HostGame(_matchID,_publicMatch,_matchName,_matchLength,_moveEnabled,_timeForMove, _player,  out playerIndex))
            {
                Debug.Log("Game Joined");
                networkMatch.matchId = _matchID.ToGuid();
                isOwner = true;
                Debug.Log("networkMatch.matchId " + networkMatch.matchId);

                TargetHostGame(true, _matchID,playerIndex, _matchName, isOwner);
            }
            else
            {
                Debug.Log("Game Joining failed");
                isOwner = false;
                TargetHostGame(false, matchID, playerIndex, _matchName, isOwner);

            }
        }
        [TargetRpc]
        void TargetHostGame(bool success, string _matchID,int _playerIndex,string matchName,bool _isOwner)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            isOwner = _isOwner;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.instance.HostSuccess(success, _matchID, matchName);
        }

        //Queue Host Game

        public void QueueHostGame()
        {
            CmdQueueHostGame(localPlayer);
        }

        [Command]
        public void CmdQueueHostGame(PlayerLobbyController _player)
        {
            Debug.Log("Getting in queue : " + _player.nickname);
            QueueController.instance.getIntoQueue(_player);
        }
        
        public void IntoGameFromQueue(Match match, List<GameObject> matchPlayers)
        {
            CmdIntoGameFromQueue(match, matchPlayers);
        }

        [Command]
        public void CmdIntoGameFromQueue(Match match, List<GameObject> matchPlayers)
        {
            GameObject newTurnManager = Instantiate(MatchMaker.instance.turnManagerPrefab);
            NetworkServer.Spawn(newTurnManager);
            newTurnManager.GetComponent<NetworkMatch>().matchId = match.matchID.ToGuid();
            TurnManager turnManager = newTurnManager.GetComponent<TurnManager>();

            foreach (var player in matchPlayers)
            {
                PlayerLobbyController _player = player.GetComponent<PlayerLobbyController>();
                turnManager.players.AddPlayer(_player);
                _player.TargetQueueHostGame();
                        
            }

        }
        [TargetRpc]
        public void TargetQueueHostGame()
        {
            uiLobby.DisplayTimeToGame();
        }
        [TargetRpc]
        public void TimerEndedChecker()
        {
            StartCoroutine(uiLobby.counterEnded(localPlayer));
        }

        public void OnLeaveQueue()
        {
            Debug.Log("From client val to send " + actualQueue.queueID);
            CmdOnLeaveQueue(actualQueue,localPlayer);
        }
        [Command]
        public void CmdOnLeaveQueue(Queue queue,PlayerLobbyController player)
        {
            Debug.Log("Server: "+ queue.queueID);
            QueueController.instance.DestroyQueue(_queue:queue);
            player.actualQueue = null;
        }
   


        //JOINING LOBBY STUFF
        public void JoinGame(string _inputID)
        {
            CmdJoinGame(_inputID);
        }
        [Command]
        void CmdJoinGame(string _matchID)
        {
            matchID = _matchID;
            foreach (var match in MatchMaker.instance.matches)
            {
                if(match.matchID == _matchID)
                {
                    actual_lobby = match;
                }
            }
            string matchName;
            //Debug.Log($"match id set to {matchID}");
            if (MatchMaker.instance.JoinGame(_matchID,gameObject, out playerIndex,out matchName))
            {
                //Debug.Log("Game Joined");
                networkMatch.matchId = _matchID.ToGuid();
                
                TargetJoinGame(true, _matchID, matchName, playerIndex, actual_lobby);
            }
            else
            {
                //Debug.Log("Game Joining failed");
                TargetJoinGame(false, _matchID, matchName, playerIndex , actual_lobby);

            }
        }
        [TargetRpc]
        void TargetJoinGame(bool success,string _matchID,string matchName, int _playerIndex,Match _actualLobby)
        {
            actual_lobby = _actualLobby;
            playerIndex = _playerIndex;
            //Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.instance.JoinSuccess(success, _matchID, matchName);
        }

        //SEARCH MATCH (NOT IMPLEMENTED)
        public void SearchGame()
        {
            CmdSearchGame();
        }

        [Command]
        public void CmdSearchGame()
        {
            string matchName = string.Empty;
            if (MatchMaker.instance.SearchGame(gameObject, out playerIndex, out matchID , out matchName))
            {
                Debug.Log("Searched successfully");
                networkMatch.matchId = matchID.ToGuid();
                TargetSearchGame(true, matchID, matchName, playerIndex);
            }
            else
            {
                Debug.Log("Search failed");
                TargetSearchGame(false, matchID, matchName, playerIndex);

            }
        }
        [TargetRpc]
        public void TargetSearchGame(bool success, string _matchID,string matchName, int _playerIndex)
        {
            playerIndex = _playerIndex;
            Debug.Log(matchID);
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.instance.SearchSuccess(success, _matchID, matchName);
        }

        //STARTING MATCH
            
        public void BeginGame()
        {
            CmdBeginGame();
        }
        [Command]
        void CmdBeginGame()
        {
            MatchMaker.instance.BeginGame(matchID);
            Debug.Log("Game start");

        }

        [TargetRpc]
        public void RpcStartGame(string nameOfScene,string matchID,PlayerLobbyController _player)
        {
            //Debug.Log("STARTING AGME");
            CmdStartGame(nameOfScene, matchID,_player);

        }
        [Command(requiresAuthority = false)]
        void CmdStartGame(string nameOfScene,string matchID, PlayerLobbyController _player)
        {
            foreach (var match in MatchMaker.instance.matches)
            {
                if(match.matchID == matchID)
                {
                    match.inMatch = true;
                }
            }
            _player.TargetBeginGame(nameOfScene);
        }
        [TargetRpc]
        public void TargetBeginGame(string nameOfScene)
        {
            Debug.Log($"MatchID: {matchID} Beginning");
            //scene
            if(SceneController.instance.activeScene() != nameOfScene)
            {
                UILobby.instance.quickMatchWaitingBox.SetActive(false);
                SceneController.instance.SceneLoad(nameOfScene);    
            }
        }

        // ON DISCONNECT ACTIONS
        public void DisconnectGame()
        {
            CmdDisconnectGame();
        }
        [Command]
        void CmdDisconnectGame()
        {
            ServerDisconnect();
        }

        void ServerDisconnect()
        {
            MatchMaker.instance.PlayerDisconnected(this, matchID);
            networkMatch.matchId = string.Empty.ToGuid();
            //Pass owner lobby 
            
            RpcDisconnectGame();
        }
        [ClientRpc]
        void RpcDisconnectGame()
        {
            ClientDisconnect();
        }
        void ClientDisconnect()
        {
            if (playerLobbyUI != null)
            {
                Destroy(playerLobbyUI);
            }
        }



        // PASSING LEADERSHIP ON LEAVE LOBBY
        public void PassLeaderDefault()
        {
            CmdPassLeaderDefault(matchID);
        }
        [Command]
        public void CmdPassLeaderDefault(string _matchID)
        {
            for (int i = 0; i < MatchMaker.instance.matches.Count; i++)
            {
                if(MatchMaker.instance.matches[i].matchID == _matchID)
                {
                    foreach (var player in MatchMaker.instance.matches[i].players)
                    {
                        if(MatchMaker.instance.matches[i].players.IndexOf(player) == 0)
                        {
                            Debug.Log(player.GetComponent<PlayerLobbyController>().nickname + "has 0 index,. ");
                            MatchMaker.instance.matches[i].players[0].GetComponent<PlayerLobbyController>().isOwner = true;
                            PlayerLobbyController owner = MatchMaker.instance.matches[i].players[0].GetComponent<PlayerLobbyController>();
                            int players_count = MatchMaker.instance.matches[i].players.Count;
                            owner.RpcPassLeader(players_count);
                           // RpcPassLeader(_matchID, owner);

                        }
                    }
                }
            }
        }
        [TargetRpc]
        public void RpcPassLeader(int players)
        {
            uiLobby.beginGameButton.SetActive(true);
            if(players < 2)
            {
                uiLobby.beginGameButton.GetComponent<Button>().interactable = false;

            }
            //uiLobby.beginGameButton.SetActive(true);
        }
        [TargetRpc]
        public void StartAvaliable()
        {
            uiLobby.beginGameButton.GetComponent<Button>().interactable = true;
        }
        //ready state refresh

        //LOBBIES DISPLAYING IN LOBBIES LIST
        public void CheckForServers()
        {
            CheckForServersCmd();
        }
        [Command]
        public void CheckForServersCmd()
        {
            if (MatchMaker.instance.matches.Count > 0)
            {
                foreach (var item in MatchMaker.instance.matches)
                {
                    SpawnServerInstance(item);
                }
            }
        }
        [TargetRpc]
        public void SpawnServerInstance(Match match)
        {
            LobbySelector serverBoxClone = Instantiate(UILobby.instance.serverBox, UILobby.instance.serverBoxParent).GetComponent<LobbySelector>();
            serverBoxClone.lobbyName.text = match.matchName;
            serverBoxClone.lobbyPlayers.text = match.players.Count.ToString();
            serverBoxClone.code.text = match.matchID;

            Debug.Log(match.matchID);

        }
        //public lobbies refresh

        public IEnumerator RefreshList()
        {
            if (UILobby.instance.serverBoxParent.childCount > 0)
            {
                foreach (Transform item in UILobby.instance.serverBoxParent.transform)
                {
                    Destroy(item.gameObject);
                }
                yield return new WaitForSeconds(2f);
            }
            CheckForServers();

            UILobby.instance.LobbyExitButton.interactable = true;
            UILobby.instance.LobbyRefreshButton.interactable = true;

            yield return null;
        }

        //Spawning UI on join / host

        [Command]
        public void SetPlayerUI(PlayerLobbyController _player,GameObject _uiPlayer)
        {
            _player.playerLobbyUI = _uiPlayer;
        }

        [Client]
        public void SpawnPlayerUI()
        {
            // GameObject newUIPlayer = Instantiate(player.uiLobby.UIPlayerPrefab, player.uiLobby.UIPlayerParent);
            //newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player);
            //newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);
            //player.uiLobby.playerLobbyUI = newUIPlayer;
            //player.playerLobbyUI = newUIPlayer;
            CmdSpawnPlayerUI(localPlayer);
        }
        [Command(requiresAuthority = false)]
        public void CmdSpawnPlayerUI(PlayerLobbyController player)
        {
           // Debug.Log("Instantiating....");
            /*
            GameObject newUIPlayer = Instantiate(UILobby.instance.UIPlayerPrefab, UILobby.instance.UIPlayerParent);
            newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player);
            newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);

            Debug.Log(newUIPlayer);
            
            NetworkServer.Spawn(newUIPlayer);
            player.playerLobbyUI = newUIPlayer;
            */
            Match lobbyInside = null;
            foreach (var match in MatchMaker.instance.matches)
            {
                Debug.Log($"match:{match.matchID} player:{player.matchID}");
                if(match.matchID == player.matchID)
                {
                    Debug.Log($"found:{match.matchID} == {player.matchID}");
                    lobbyInside = match;
                }
            }
            
           // Debug.Log("Spawning....");
           // Debug.Log("Spawned");
           Debug.Log($"CmdSpawnPlayerUI:{lobbyInside}");
            SpawnPlayerUIClient(player, lobbyInside);
            
        }
        [ClientRpc]
        public void RpcSpawnPlayerUI(Match lobbyInside)
        {
            Debug.Log(lobbyInside);
            foreach (var player in lobbyInside.players)
            {
                Debug.Log(playerLobbyUI);
                Debug.Log(player);
                if (player.GetComponent<PlayerLobbyController>().playerLobbyUI != null)
                {
                    Destroy(player.GetComponent<PlayerLobbyController>().playerLobbyUI.gameObject);
                }
                
                GameObject newUIPlayer = Instantiate(UILobby.instance.UIPlayerPrefab, UILobby.instance.UIPlayerParent);
                newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player.GetComponent<PlayerLobbyController>());
                newUIPlayer.transform.SetSiblingIndex(player.GetComponent<PlayerLobbyController>().playerIndex - 1);
                player.GetComponent<PlayerLobbyController>().playerLobbyUI = newUIPlayer;
                if (isServer)
                {
                    NetworkServer.Spawn(newUIPlayer);

                }



            }
           Debug.Log("Setting newUIPlayer to uiLobby.playerLobbyUI....");
           Debug.Log("Setting stuff...");
           Debug.Log("Before");
        //    Debug.Log(UIPlayer);
        //    Debug.Log(player);
            
           // Debug.Log(player.uiLobby.playerLobbyUI);
            //Debug.Log(player.playerLobbyUI);
           // player.uiLobby.playerLobbyUI = UIPlayer;
            //player.playerLobbyUI = _UIPlayer;
          //  Debug.Log("After");
            //playerLobbyUI = _UIPlayer;
            //Debug.Log(player.uiLobby.playerLobbyUI);
            //Debug.Log(player.playerLobbyUI);
          //  Debug.Log("Setting newUIPlayer to player.playerLobbyUI....");
            //player.SetPlayerUI(player, UIPlayer);
            
        }
        public void SpawnPlayerUIClient(PlayerLobbyController player,Match lobbyInside)
        {
            Debug.Log(player);
            Debug.Log(lobbyInside);
            RpcSpawnPlayerUI(lobbyInside);
        }
        


        //ready state update (NOT IMPLEMENTED)

        public void ReadyStateUpdate()
        {
            if(isReady == false)
            {

                isReady = true;
                //playerLobbyUIcolor = new Color32(0, 180, 0, 255);

            }
            else
            {
                isReady = false;
                //playerLobbyUIcolor = new Color32(180, 0, 0, 255);

            }
            Debug.Log("LOCALisReady: " + isReady);
            //uiLobby.UIReadyChange(localPlayer);
            
            ReadyStateUpdateServerCmd(isReady, playerLobbyUIcolor, this);
        }
        [Command]
        public void ReadyStateUpdateServerCmd(bool _isReady,Color32 _playerLobbyUIColor, PlayerLobbyController _player)
        {
            Debug.Log("Server received data...");
            _player.GetComponent<PlayerLobbyController>().isReady = _isReady;
            //_player.GetComponent<Player>().playerLobbyUIcolor = _playerLobbyUIColor;
            ReadyStateUpdateOnClients(_isReady, _playerLobbyUIColor, _player);

            
           // _player.uiLobby.UIReadyChange(localPlayer);

            Debug.Log("SERVERisReady: " + isReady);
            //ReadyStateUpdateOnClients(_isReady, _playerLobbyUIColor, this);



        }
        [ClientRpc]
        public void ReadyStateUpdateOnClients(bool _isReady, Color32 _playerLobbyUIColor, PlayerLobbyController _player)
        {

            Debug.Log("Clients received data");
            _player.GetComponent<PlayerLobbyController>().isReady = _isReady;
           // _player.GetComponent<Player>().playerLobbyUIcolor = _playerLobbyUIColor;
            Debug.Log(_player.uiLobby);
            //_player.uiLobby.UIReadyChange(localPlayer);


            Debug.Log("CLIENTisReady: " + isReady);



        }




    }

}
