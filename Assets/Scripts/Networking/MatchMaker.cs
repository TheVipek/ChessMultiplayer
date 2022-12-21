using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MirrorNetworking
{
    [System.Serializable]
    public class Match
    {
        public string matchID;
        public bool publicMatch;
        public bool inMatch;
        public bool matchFull;

        public string matchName;
        public float matchLength;
        public bool isTimeMoveEnabled;
        public float timeForMove;
        public bool isPublicMatch;

        public List<GameObject> players = new List<GameObject>();

        public Match(string matchID, GameObject player = null,List<GameObject> playersList = null)
        {
            Debug.Log(player);
            if(player == null && playersList == null)
            {
                Debug.LogError("You need assign to match player or playerList");
                return;
            }
            this.matchID = matchID;
            if (player)
            {
                players.Add(player);
            }
            else
            {
                players.AddRange(playersList);
            }
        }
        public void setSettings(string _matchName,float _matchLength,bool _isTimeMoveEnabled,float _timeForMove,bool _isPublicMatch)
        {
            matchName = _matchName;
            matchLength = _matchLength;
            isTimeMoveEnabled = _isTimeMoveEnabled;
            timeForMove = _timeForMove;
            isPublicMatch = _isPublicMatch;
        }
        public void setDefault()
        {
            matchName = GameManager.instance.gameObject.GetComponent<stringGenerator>().StringGenerate("Match");
            matchLength = 600;
            isTimeMoveEnabled = false;
            timeForMove = 0;
            isPublicMatch = true;

        }

        public string getMatchName()
        {
            return matchName;
        }
        public float getMatchLength()
        {
            return matchLength;
        }
        public bool getisTimeMoveEnabled()
        {
            return isTimeMoveEnabled;
        }
        public bool getIsPublic()
        {
            return isPublicMatch;
        }
        public float getTimeForMove()
        {
            return timeForMove;
        }
        
        public Match() { }
    }


    
    public class MatchMaker : NetworkBehaviour
    {
    

        public static MatchMaker instance;

        public readonly SyncList<Match> matches = new SyncList<Match>();

        public readonly SyncList<string> matchIDs = new SyncList<string>();

        

        [SerializeField] public GameObject turnManagerPrefab;
        [SerializeField] public GameObject serverManagerPrefab;

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
        

        public bool HostGame(string matchID, bool publicMatch, string matchName, float matchLength, bool moveEnabled, float timeForMove, GameObject _player, out int playerIndex)
        {
            playerIndex = -1;
            if (!matchIDs.Contains(matchID))
            {
                Debug.Log(_player);
                
                matchIDs.Add(matchID);
                Match match = new Match(matchID, player:_player);

                //Get settings from UI
                match.setSettings(_matchName: matchName,
                    _matchLength: matchLength,
                    _isTimeMoveEnabled: moveEnabled,
                    _timeForMove: timeForMove,
                    _isPublicMatch: publicMatch
                    );

                matches.Add(match);
                _player.GetComponent<PlayerLobbyController>().actual_lobby = match;


                playerIndex = 1;
                return true;

            }
            else
            {
                Debug.Log("Match already exists");
                return false;

            }
        }
       
       
        public bool JoinGame(string _matchID, GameObject _player, out int playerIndex,out string matchName)
        {
            playerIndex = -1;
            matchName = string.Empty;
            int playerCount = 0;
            if (matchIDs.Contains(_matchID))
            {
                
                for (int i = 0; i < matches.Count; i++)
                {
                    if(matches[i].matchID == _matchID)
                    {
                        matches[i].players.Add(_player);
                        playerIndex = matches[i].players.Count;
                        matchName = matches[i].matchName;
                        playerCount = matches[i].players.Count;
                        if(playerCount >= 2)
                        {
                            foreach (var player in matches[i].players)
                            {
                                if (player.GetComponent<PlayerLobbyController>().isOwner)
                                {
                                    player.GetComponent<PlayerLobbyController>().StartAvaliable();
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                Debug.Log("Match joined");
                return true;

            }
            else
            {

                Debug.Log("Match ID doesnt exist");
                return false;

            }
        }
        public bool SearchGame(GameObject _player,out int playerIndex,out string matchID,out string matchName)
        {
            playerIndex = -1;
            matchID = string.Empty;
            matchName = string.Empty;
            for (int i = 0; i < matches.Count; i++)
            {
                if(matches[i].publicMatch && !matches[i].matchFull && !matches[i].inMatch)
                {
                    matchID = matches[i].matchID;
                    if (JoinGame(matchID,_player,out playerIndex,out matchName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void BeginGame(string _matchID)
        {
           
            Debug.Log("All players are ready,game is ready to start");
            
            GameObject newTurnManager = Instantiate(turnManagerPrefab);
            NetworkServer.Spawn(newTurnManager);
            newTurnManager.GetComponent<NetworkMatch>().matchId = _matchID.ToGuid();
            TurnManager.instance = newTurnManager.GetComponent<TurnManager>();
            
            GameObject newServerManagerPrefab = Instantiate(serverManagerPrefab);
            NetworkServer.Spawn(newServerManagerPrefab);
            newServerManagerPrefab.GetComponent<NetworkMatch>().matchId = _matchID.ToGuid();
            ServerMatchController.instance = newServerManagerPrefab.GetComponent<ServerMatchController>();
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == _matchID)
                {

                    foreach (var player in matches[i].players)
                    {
                        PlayerLobbyController _player = player.GetComponent<PlayerLobbyController>();
                        Debug.Log(_player.nickname + " going in game");
                        TurnManager.instance.players.AddPlayer(_player);
                        
                        _player.RpcStartGame("OnlineGame",matches[i].matchID,_player);
                    }
                    break;
                }
            }
            
            
            

        }

        public static string GetRandomMatchID()
        {
            string _id = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);
                if(random < 26)
                {
                    _id += (char)(random +65);
                }
                else
                {
                    _id += (random - 26).ToString();
                }
            }
            //Debug.Log($"generated name of lobby ; {_id}");
            return _id;
        }
        public string IDSecureCheck(string matchID)
        {
            if (!matchIDs.Contains(matchID))
            {
                return matchID;
            }
            while (matchIDs.Contains(matchID))
            {
                matchID = GetRandomMatchID();
            }
            return matchID;
        }
        public void PlayerDisconnected(PlayerLobbyController player,string _matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if(matches[i].matchID == _matchID)
                {
                    int playerIndex = matches[i].players.IndexOf(player.gameObject);
                    matches[i].players.RemoveAt(playerIndex);
                    
                    Debug.Log($"Player disconnected from match {_matchID} | {matches[i].players.Count} players remaining");
                    if(matches[i].players.Count == 0)
                    {
                        Debug.Log($"No more players in Match.Terminating {_matchID}");
                        matches.RemoveAt(i);
                        matchIDs.Remove(_matchID);
                        break;
                    }
                    if (matches[i].inMatch == true)
                    {
                        Debug.Log("Starting counter to reconnect....");
                        ServerMatchController.instance.EndGame(player.GetComponent<PlayerController>());
                        //GameController.instance.EndGame($"{matches[i].players[0].GetComponent<PlayerLobbyController>().nickname} Won!");
                    }
                    player.Reset();
                    player.GetComponent<PlayerController>().Reset();
                    PassLeader(matches[i], 0);
                    break; 
                }
            }
        }
        
        public void PassLeader(Match match,int playerIndex)
        {
            match.players[playerIndex].GetComponent<PlayerLobbyController>().isOwner = true;

        }
        
        

        //NOT IN-GAME
        public bool CheckIfAllReady(string _matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == _matchID)
                {
                    int PlayersCount = matches[i].players.Count;
                    int readyPlayers = 0;

                    foreach (GameObject player in matches[i].players)
                    {
                        PlayerLobbyController _player = player.GetComponent<PlayerLobbyController>();
                        if (_player.isReady)
                        {
                            readyPlayers += 1;
                        }
                       
                    }
                    if(PlayersCount == readyPlayers)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
    }
        public static class MatchExtensions{
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            return new Guid(hashBytes);
        }
    }

}
