using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorNetworking
{
    
    public class ServerMatchSettings : NetworkBehaviour
    {

        [SyncVar] public float WTimeLeft;
        [SyncVar (hook=nameof(OnWTeamChanged))] public List<GameObject> WTeam = new List<GameObject>();
        [SyncVar(hook = nameof(OnWDestroyedChanged))] public GameObject wDestroyed;
        public GameObject wTimer;
        public GameObject wTimerCounter;
        public GameObject wPlayer;
        [SyncVar] public float BTimeLeft;
        [SyncVar(hook = nameof(OnBTeamChanged))] public List<GameObject> BTeam = new List<GameObject> ();
        [SyncVar (hook=nameof(OnBDestroyedChanged))] public GameObject bDestroyed;
        public GameObject bTimer;
        public GameObject bTimerCounter;
        public GameObject bPlayer;



        public bool started = false;
        public int figureInitialized = 0;

        public string matchID;

        public static ServerMatchSettings instance;
        private void Start()
        {
            instance = this;
        }


        public void OnWTeamChanged(List<GameObject> oldTeam, List<GameObject> newTeam)
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                PlayerController _player = player.Key.GetComponent<PlayerController>();
                if (_player.teamSide == 1)
                {
                    Debug.Log(_player.teamSide + "OnClient value updated");
                    _player.myTeam = newTeam;
                    //_player.myTeam = newTeam;
                }
            }
        }
        public void OnBTeamChanged(List<GameObject> oldTeam, List<GameObject> newTeam)
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                PlayerController _player = player.Key.GetComponent<PlayerController>();
                if (_player.teamSide == -1)
                {
                    Debug.Log(_player.teamSide + "OnClient value updated");
                    //_player.myTeam = newTeam;
                    _player.myTeam = newTeam;

                }
            }
        }
        public void setTeam(int teamVal, PlayerLobbyController _player)
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                if (player.Key.nickname == _player.nickname)
                {
                    PlayerController playerController = player.Key.GetComponent<PlayerController>();
                    if (teamVal == -1)
                    {
                        Debug.Log(player.Key.nickname);
                        Debug.Log("Server: Your team is BTeam");
                        playerController.myTeam = BTeam;
                    }
                    else
                    {
                        Debug.Log(player.Key.nickname);

                        Debug.Log("Server: Your team is WTeam");

                        playerController.myTeam = WTeam;
                    }
                }
            }
        }

        public void OnWDestroyedChanged(GameObject oldVal, GameObject newVal)
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                
                PlayerController _player = player.Key.GetComponent<PlayerController>();
                if (_player.teamSide == 1)
                {
                    _player.myDestroyed = newVal;
                }
            }

        }

        public void OnBDestroyedChanged(GameObject oldVal, GameObject newVal)
        {
            foreach (var player in TurnManager.instance.players.players)
            {

                PlayerController _player = player.Key.GetComponent<PlayerController>();
                if (_player.teamSide == -1)
                {
                    _player.myDestroyed = newVal;

                }
            }
        }

    }
}

