using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.UI;
namespace MirrorNetworking
{

    public class ServerMatchController : NetworkBehaviour
    {
        public static ServerMatchController instance;
        [SyncVar] public int playerTour;
        public uint gameboardID;
        private void Awake()
        {
            instance = this;
        }
        private void LateUpdate()
        {
            if(Time.timeScale == 1 && ServerMatchSettings.instance.started== true && isServer)
            {
                if (playerTour == 1)
                {
                    ServerMatchSettings.instance.bTimerCounter.GetComponent<TimeRefresher>().timeStop();
                    ServerMatchSettings.instance.wTimerCounter.GetComponent<TimeRefresher>().timeStart();
                }
                else if (playerTour == -1)
                {
                    ServerMatchSettings.instance.wTimerCounter.GetComponent<TimeRefresher>().timeStop();
                    ServerMatchSettings.instance.bTimerCounter.GetComponent<TimeRefresher>().timeStart();
                }

               
                
            }
           
        }
        [Server]
        public void StartGame()
        {
            //Debug.Log("Game is ready to start!");
            //Debug.Log("Entry settings");
            GameManager.instance.SetStartingTime();
            SpawnGameboard();
            GenerateFigures();
            setSettings();
            SetTeamSide();
            SpawnFigures();
            setTeamClients();
            
            /*
            setTeamsParents();
            SpawnUIElements();
            //Debug.Log("TeamSide set");
            //Debug.Log("Spawn UIElements");
            ServerMatchSettings.instance.figureInitialized = 1;
            ServerMatchSettings.instance.started = true;
            PlayerTour();
            */

        }

        
        [Server]
        public void EndGame(PlayerController lose)
        {
            Time.timeScale = 0;
            
            foreach (var player in TurnManager.instance.players.players)
            {
                PlayerController _player = player.Key.GetComponent<PlayerController>();
                string nickname = player.Key.nickname;
                if(_player != lose)
                {
                    string notification = $"YOU WON!";
                    _player.TargetEndGame(notification);
                    
                }
                else
                {
                    string notification = $"YOU LOST... ";
                    _player.TargetEndGame(notification);
                }
            }


        }

        /// <summary>
        /// Gameboard needs to be spawned to be visible for all clients ,if not only host would see it
        /// </summary>
        [Server]
        public void SpawnGameboard()
        {

            NetworkServer.Spawn(GameController.instance.gameBoard);
            GameController.instance.gameBoard.gameObject.GetComponent<NetworkMatch>().matchId = GetComponent<NetworkMatch>().matchId;
            gameboardID = GameController.instance.gameBoard.GetComponent<NetworkIdentity>().netId;
            Debug.Log("Spawned GameBoard");
        }
        /// <summary>
        /// At start of each game figures needs to be generated and placed at board
        /// </summary>
        [Server]
        public void GenerateFigures()
        {
            BoardController.instance.GenerateFigures();
        }
        /// <summary>
        /// Server needs to get few settings like BTeam,WTeam and their time left 
        /// </summary>
        [Server]
        public void setSettings()
        {
            playerTour = 1;
            ServerMatchSettings.instance.BTeam = GameManager.instance.blackTeam;
            ServerMatchSettings.instance.WTeam = GameManager.instance.whiteTeam;
        }
        /// <summary>
        /// Players needs to have their identificator for their teamSide ,so player with 
        /// -1(black) side can't do anything with 1(white)
        /// </summary>
        [Server]
        public void SetTeamSide()
        {
            int prevSide = -1;
            int teamSide;
            foreach (var player in TurnManager.instance.players.players)
            {
                PlayerController playerController = player.Key.GetComponent<PlayerController>();
                teamSide = Random.Range(0,2);
                if(teamSide != prevSide)
                {
                    if(teamSide == 0)
                    {
                        prevSide = teamSide;
                        playerController.teamSide = -1;
                    }
                    else
                    {
                        playerController.teamSide = teamSide;
                        prevSide = playerController.teamSide;
                    }
                }
                else
                {
                    if(prevSide == 0)
                    {
                        playerController.teamSide = 1;
                    }
                    else
                    {
                        playerController.teamSide = -1;
                    }
                }
            }
            
        }
        /// <summary>
        /// Like in SpawnGameboard
        /// </summary>
        [Server]
        public void SpawnFigures()
        {
            Debug.Log("Spawning figures");
            for (int i = 0; i < ServerMatchSettings.instance.WTeam.Count; i++)
            {
                SpawnWantedObject(ServerMatchSettings.instance.WTeam[i]);
                ServerMatchSettings.instance.WTeam[i].GetComponent<Chess>().figureID = ServerMatchSettings.instance.WTeam[i].GetComponent<NetworkIdentity>().netId;
            }
            for (int i = 0; i < ServerMatchSettings.instance.BTeam.Count; i++)
            {
                SpawnWantedObject(ServerMatchSettings.instance.BTeam[i]);
                ServerMatchSettings.instance.BTeam[i].GetComponent<Chess>().figureID = ServerMatchSettings.instance.BTeam[i].GetComponent<NetworkIdentity>().netId;


            }
            Debug.Log(ServerMatchSettings.instance.figureInitialized);
            Debug.Log("Spawned Figures");
            
        }
        /// <summary>
        /// After spawning figures they may be assigned to players 
        /// </summary>
        [Server]
        public void setTeamClients()
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                PlayerController _player = player.Key.GetComponent<PlayerController>();
                if (_player.teamSide == 1)
                {
                    foreach (var figure in ServerMatchSettings.instance.WTeam)
                    {
                        figure.GetComponent<NetworkIdentity>().AssignClientAuthority(_player.GetComponent<NetworkIdentity>().connectionToClient);
                    }
                    _player.myTeam = ServerMatchSettings.instance.WTeam;
                    
                }
                else
                {
                    foreach (var figure in ServerMatchSettings.instance.BTeam)
                    {
                        figure.GetComponent<NetworkIdentity>().AssignClientAuthority(_player.GetComponent<NetworkIdentity>().connectionToClient);
                    }
                    _player.myTeam = ServerMatchSettings.instance.BTeam;
                }
            }

            
        }

        /// <summary>
        /// Host doesnt have this issue,but stuff like parent of object is impossible to keep
        /// so it needs to be manually set at all clients through TargetSyncObjectParentThroughMainParent()
        /// TargetSyncInteractableOnClient() basically sets wanted gameobject interactible in button
        /// and it's set to false at black team by default ,beacuse white always starts first
        /// </summary>
        [Server]
        public void setTeamsParents()
        {

            for (int i = 0; i < ServerMatchSettings.instance.WTeam.Count; i++)
            {
                 Transform figureParent = ServerMatchSettings.instance.WTeam[i].transform.parent;
                 uint objID = ServerMatchSettings.instance.WTeam[i].GetComponent<NetworkIdentity>().netId;
                 foreach (var player in TurnManager.instance.players.players)
                 {
                    PlayerController _player = player.Key.GetComponent<PlayerController>();
                    Chess _chess = ServerMatchSettings.instance.WTeam[i].GetComponent<Chess>();
                    _player.TargetSyncObjectParentThroughMainParent(ServerMatchSettings.instance.WTeam[i], BoardController.instance.gameObject, figureParent.name);
                    //_player.TargetSyncFigure(ServerMatchSettings.instance.WTeam[i].name, null, figureParent.name);
                 }
            }
            for (int i = 0; i < ServerMatchSettings.instance.BTeam.Count; i++)
            {
                Transform figureParent = ServerMatchSettings.instance.BTeam[i].transform.parent;
                uint objID = ServerMatchSettings.instance.BTeam[i].GetComponent<NetworkIdentity>().netId;

                foreach (var player in TurnManager.instance.players.players)
                {
                    PlayerController _player = player.Key.GetComponent<PlayerController>();
                    Chess _chess = ServerMatchSettings.instance.BTeam[i].GetComponent<Chess>();
                    //_player.TargetSyncFigure(ServerMatchSettings.instance.BTeam[i].name, null, figureParent.name);

                    _player.TargetSyncObjectParentThroughMainParent(ServerMatchSettings.instance.BTeam[i], BoardController.instance.gameObject, figureParent.name);
                    _player.TargetSyncInteractableOnClient(ServerMatchSettings.instance.BTeam[i], false);
                }

            }
        }
        /// <summary>
        /// *UNUSED*
        /// </summary>
        [Server]
        public void SpawnTiles()
        {
            BoardManager.instance.tiles.gameObject.GetComponent<NetworkMatch>().matchId = GetComponent<NetworkMatch>().matchId;
            NetworkServer.Spawn(BoardManager.instance.tiles.gameObject);
            foreach (var tile in BoardManager.instance.tileList)
            {
                tile.GetComponent<NetworkMatch>().matchId = GetComponent<NetworkMatch>().matchId;
                NetworkServer.Spawn(tile);

            }
            //Debug.Log("Spawned tiles");
        }


        /// <summary>
        /// Function that handles all smaller functions so it's executing in wanted sequence and 
        /// looks better than it would be when there would be 10 functions more in Start()
        /// </summary>
        [Server]
        public void SpawnUIElements()
        {
            SpawnPlayersInfo();
            SpawnPlayersTimers();
            SpawnDestroyedList();
            
            foreach (var player in TurnManager.instance.players.players.Keys)
            {
                PlayerController _player = player.GetComponent<PlayerController>();
                int teamside = _player.teamSide;
                _player.TargetSetRotationOfBoard(teamside);
                _player.TargetCorrectSpawnPlayersTimers(teamside, ServerMatchSettings.instance.wTimer, ServerMatchSettings.instance.bTimer);
                _player.TargetCorrectSpawnPlayersInfo(teamside, ServerMatchSettings.instance.wPlayer, ServerMatchSettings.instance.bPlayer);
                _player.TargetCorrectSpawnDestroyedList(teamside, ServerMatchSettings.instance.wDestroyed, ServerMatchSettings.instance.bDestroyed);

                /*if(teamside == 1)
                {
                    _player.myDestroyed = ServerMatchSettings.instance.wDestroyed;
                }
                else
                {
                    _player.myDestroyed = ServerMatchSettings.instance.bDestroyed;

                }*/
            }

            //Debug.Log("generating figures");
        }
        /// <summary>
        /// Spawns Players tables that contains their nicknames with default set (0) , Server POV ,
        /// Sync parents on clients and assigns variables
        /// </summary>
        [Server]
        public void SpawnPlayersInfo()
        {
            GameController.instance.playersInfoSpawn(0, players: TurnManager.instance.players.players.Keys.ToList());
            Transform PlayersInfoParent = GameController.instance.gameBoard.transform;
            foreach (var player in TurnManager.instance.players.players)
            {
                player.Key.GetComponent<PlayerController>().TargetSyncObjectParentByTransformParent(BoardManager.instance.wPlayer, PlayersInfoParent);
                player.Key.GetComponent<PlayerController>().TargetSyncObjectParentByTransformParent(BoardManager.instance.bPlayer, PlayersInfoParent);
            }
            ServerMatchSettings.instance.wPlayer = BoardManager.instance.wPlayer;
            ServerMatchSettings.instance.bPlayer = BoardManager.instance.bPlayer;
        }
        /// <summary>
        /// Like in SpawnPlayersInfo()
        /// </summary>
        [Server]
        public void SpawnPlayersTimers()
        {
            GameController.instance.TimersSpawn(0);
            Transform PlayersInfoParent = GameController.instance.gameBoard.transform;
            ServerMatchSettings.instance.wTimer = BoardManager.instance.Wtimer;
            ServerMatchSettings.instance.wTimerCounter = ServerMatchSettings.instance.wTimer.transform.GetChild(0).gameObject;
            ServerMatchSettings.instance.bTimer = BoardManager.instance.Btimer;
            ServerMatchSettings.instance.bTimerCounter = ServerMatchSettings.instance.bTimer.transform.GetChild(0).gameObject;
            SpawnWantedObject(ServerMatchSettings.instance.wTimer);
            SpawnWantedObject(ServerMatchSettings.instance.bTimer);
            foreach (var player in TurnManager.instance.players.players)
            {
                player.Key.GetComponent<PlayerController>().TargetSyncObjectParentByTransformParent(ServerMatchSettings.instance.wTimer, PlayersInfoParent);
                player.Key.GetComponent<PlayerController>().TargetSyncObjectParentByTransformParent(ServerMatchSettings.instance.bTimer, PlayersInfoParent);
            }
        }
        /// <summary>
        /// Like in SpawnPlayersInfo()
        /// </summary>
        [Server]
        public void SpawnDestroyedList()
        {
            GameController.instance.DestroyedListSpawn(0);
            SpawnWantedObject(BoardManager.instance.wDestroyed);
            SpawnWantedObject(BoardManager.instance.bDestroyed);
            Transform PlayersInfoParent = GameController.instance.gameBoard.transform;
            Debug.Log("setting destroyed values");
            ServerMatchSettings.instance.wDestroyed = BoardManager.instance.wDestroyed;
            ServerMatchSettings.instance.bDestroyed = BoardManager.instance.bDestroyed;
            Debug.Log("triggering hook?");
            foreach (var player in TurnManager.instance.players.players)
            {
                player.Key.GetComponent<PlayerController>().TargetSyncObjectParentByTransformParent(ServerMatchSettings.instance.wDestroyed, PlayersInfoParent);
                player.Key.GetComponent<PlayerController>().TargetSyncObjectParentByTransformParent(ServerMatchSettings.instance.bDestroyed, PlayersInfoParent);
            }

        }
        /// <summary>
        /// Quick function to set gameobject matchID and spawn it 
        /// </summary>
        /// <param name="wanted">object we want to spawn</param>
        [Server]
        public void SpawnWantedObject(GameObject wanted)
        {
            NetworkServer.Spawn(wanted);
            wanted.GetComponent<NetworkMatch>().matchId = GetComponent<NetworkMatch>().matchId;

        }
        
        /// <summary>
        /// *UNUSED*
        /// </summary>
        [Server]
        public void RemoveChessFromTeam(GameObject chessToRemove)
        {
            
        }

        /// <summary>
        /// sets figuresInteractable (which is hook ,that sets player team to be interactable of not)
        /// </summary>
        [Server]
        public void PlayerTour()
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                //Debug.Log(player.Key.nickname + " setting playerTour");
                PlayerController playerController = player.Key.GetComponent<PlayerController>();
                //playerController.TargetSwapSide(playerTour, TurnManager.instance.players.players.Keys.ToList());
                if (playerController.teamSide == playerTour)
                {
                    Debug.Log(playerController.teamSide + " is interactable");
                    playerController.figuresInteractable = true;
                    //Debug.Log(player.Key.nickname + "the same teamSide" + "enabling" +"\n" + playerController.teamSide + " : " + playerTour);
                }
                else
                {
                    Debug.Log(playerController.teamSide + " is not interactable");

                    playerController.figuresInteractable = false;
                    //Debug.Log(player.Key.nickname + "has different teamSide" + "disabling" + "\n" + playerController.teamSide + " : " + playerTour);
                }

            }
        }

        /// <summary>
        /// swaps side when player make move  example:from -1(black) to 1(white) 
        /// and call PlayerTour()
        /// </summary>
        [Server]
        public void SwapSide()
        {
            if (playerTour == 1)
            {
                playerTour = -1;
            }
            else
            {
                playerTour = 1;
            }
            PlayerTour();
        }
        [Server]
        public void SyncPosition(GameObject child,Transform parent)
        {
            if (ServerMatchSettings.instance.figureInitialized == 1)
            {
                foreach (var player in TurnManager.instance.players.players)
               {
                   player.Key.GetComponent<PlayerController>().TargetSyncObjectParentByTransformParent(child, parent);

               }

            }
            
        }
        
        
        /*public void LeaveGameCounter()
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                player.Key.
            }
        }*/
        
        [Server]
        public void PlayerDisconnected()
        {
            foreach (var players in TurnManager.instance.players.players)
            {
                Debug.Log(players);
            }
        }
        
    }
}

