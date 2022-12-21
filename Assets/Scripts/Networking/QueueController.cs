using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
namespace MirrorNetworking


{
    [System.Serializable]
    public class Queue
    {
        public string queueID;
        public int maxPlayers = 2;
        public List<GameObject> players = new List<GameObject>();
        

        public Queue(GameObject _player,string _queueID)
        {
            players.Add(_player);
            queueID = _queueID;
            
        }

        public void Reset()
        {
            queueID = string.Empty;

        }
        public Queue() { }


    }
    public class QueueController : NetworkBehaviour
    {
        public static QueueController instance;
        public List<Queue> queues = new List<Queue>();
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

        [Server]
        public void getIntoQueue(PlayerLobbyController _player)
        {
            if (queues.Count == 0)
            {
                Debug.Log("No queues,creating new one.");
                startQueue(_player);
                return;
            }
            else
            {
                Debug.Log("Looking for queue...");
                foreach (Queue queue in queues.ToArray())
                {
                    if (queue.players.Count < queue.maxPlayers)
                    {
                        Debug.Log("Found queue! \n"+_player.nickname+" is joining...");
                        JoinQueue(queue, _player);
                        string matchID = MatchMaker.GetRandomMatchID();
                        matchID = MatchMaker.instance.IDSecureCheck(matchID);

                        if (QueueHostGame(matchID, queue.players, queue) == true)
                        {
                            
                            foreach (var match in MatchMaker.instance.matches)
                            {
                                if (match.matchID == matchID)
                                {
                                    Debug.Log("Calling IntoGameFromQueue() function from server");
                                    IntoGameFromQueue(match, match.players);
                                    /*foreach(var player in match.players)
                                    {
                                        //Debug.Log("Players to turn manager and " +
                                        //    " displaying notification for each person");
                                        PlayerLobbyController _player = player.GetComponent<PlayerLobbyController>();
                                        _player.TimerEndedChecker();
                                    }*/
                                }
                            }


                        }


                    }

                }
            }
            
            
        }
        public bool QueueHostGame(string matchID, List<GameObject> players, Queue queue)
        {

            if (!MatchMaker.instance.matchIDs.Contains(matchID))
            {
                
                MatchMaker.instance.matchIDs.Add(matchID);
                Match match = new Match(matchID,playersList:players);
                foreach (var player in players)
                {
                    player.GetComponent<NetworkMatch>().matchId = match.matchID.ToGuid();
                    player.GetComponent<PlayerLobbyController>().matchID = match.matchID;
                }
                match.setDefault();
                MatchMaker.instance.matches.Add(match);

                Debug.Log("Calling DestroyQueue");
                DestroyQueue(queue);

                return true;

            }
            else
            {
                //Debug.Log("Unexpected error");
                return false;

            }
        }
        public void IntoGameFromQueue(Match match, List<GameObject> matchPlayers)
        {
            
            GameObject newTurnManager = Instantiate(MatchMaker.instance.turnManagerPrefab);
            NetworkServer.Spawn(newTurnManager);
            newTurnManager.GetComponent<NetworkMatch>().matchId = match.matchID.ToGuid();
            TurnManager turnManager = newTurnManager.GetComponent<TurnManager>();
            GameObject newServerManagerPrefab = Instantiate(MatchMaker.instance.serverManagerPrefab);
            NetworkServer.Spawn(newServerManagerPrefab);
            newServerManagerPrefab.GetComponent<NetworkMatch>().matchId = match.matchID.ToGuid();
            turnManager.players = new PlayersInGame();
            foreach (var player in matchPlayers)
            {
                PlayerLobbyController _player = player.GetComponent<PlayerLobbyController>();
                
                turnManager.players.AddPlayer(_player);
                _player.RpcStartGame("OnlineGame",match.matchID,_player);
                

            }
          
            


        }
        [Server]
        public void JoinQueue(Queue queue, PlayerLobbyController playerToJoin)
        {
            queue.players.Add(playerToJoin.gameObject);
        }
        public void startQueue(PlayerLobbyController _player)
        {
            string queueID = MatchMaker.GetRandomMatchID();
            queueID = IDSecureCheck(queueID);
            Queue queue = new Queue(_player.gameObject, queueID);
            _player.actualQueue = queue; 
            queues.Add(queue);
        }
        [Server]
        public void DestroyQueue(Queue _queue = null)
        {
            
            Debug.Log("Destroying queue : "  + _queue.queueID + " " +_queue);
            if(_queue == null)
            {
                Debug.Log("Check parameters!!!");
                return;
            }
            foreach (var queue in queues)
            {
                if(queue.queueID == _queue.queueID)
                {
                    Debug.Log("Removing...");

                    int pos = queues.IndexOf(queue);
                    queues.RemoveAt(pos);
                    return;
                }
            }
        }
        public string IDSecureCheck(string matchID)
        {
            foreach (var queue in queues)
            {
                if (!queue.queueID.Contains(matchID))
                {
                    return matchID;
                }
                else
                {
                    while (queue.queueID.Contains(matchID))
                    {
                        matchID = MatchMaker.GetRandomMatchID();
                    }
                }
            }
            return matchID;
            
        }
    }
}
