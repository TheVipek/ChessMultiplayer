using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace MirrorNetworking {

    public class Team
    {
        public List<GameObject> members;
        public Team(List<GameObject> team) {
            members = team;
        }
        public Team() { }
    }
    public class PlayerController : NetworkBehaviour
    {
        [Header("In game settings")]
        public static PlayerController playerController;
        [SyncVar] public int teamSide;  // -1 = black  1 = white
        public List<GameObject> myTeam;
        [SyncVar] public GameObject myDestroyed;
        [SyncVar(hook =nameof(OnFigureInteractableChange))] public bool figuresInteractable;
        private void Awake()
        {
            playerController = this;
        }
     


        /* 
         public void SetTeamSide(PlayerController _player)
         {
             CmdSetTeamSide(_player);
         }
         [Command(requiresAuthority = false)]
         public void CmdSetTeamSide(PlayerController _player)
         {
             BoardManager.instance.GenerateFigures();

             foreach (var player in TurnManager.instance.players)
             {
                 if(player.nickname == _player.GetComponent<PlayerLobbyController>().nickname)
                 {
                     PlayerController playerController = player.GetComponent<PlayerController>();
                     int teamSide = Random.Range(0, 2);
                     foreach (var Jplayer in TurnManager.instance.players)
                     {
                         while(teamSide == Jplayer.GetComponent<PlayerController>().teamSide)
                         {
                             teamSide = Random.Range(0, 2);
                         }
                     }
                     playerController.teamSide = teamSide;

                     playerController.TargetSetTeamSide(teamSide,TurnManager.instance.players);
                     Debug.Log("Spawning figures for: " + player.nickname + " with side: " + teamSide);
                 }

             }
         }
         [TargetRpc]
         public void TargetSetTeamSide(int _teamSide, List<PlayerLobbyController> players)
         {
             teamSide = _teamSide;
             Debug.Log("Client has " + teamSide);
             Debug.Log(teamSide);
             Debug.Log(players.Count);
             BoardManager.instance.setRotationOfBoard(teamSide);
             GameController.instance.playersInfoSpawn(teamSide, players);
             GameController.instance.TimersSpawn(teamSide);
             GameController.instance.DestroyedListSpawn(teamSide);
             GameController.instance.PlayerTour();
             GameManager.instance.started = true;
         }

         public void InstantiateItems(PlayerController _player)
         {
             CmdInstantiateItems(_player);
         }
         [Command(requiresAuthority = false)]
         public void CmdInstantiateItems(PlayerController _player)
         {
             BoardManager.instance.GenerateFigures();
             foreach (var player in TurnManager.instance.players)
             {
                 if(player.nickname == _player.GetComponent<PlayerLobbyController>().nickname)
                 {
                     player.GetComponent<PlayerController>().TargetInstantiateItems(TurnManager.instance.players);
                 }
             }
         }
         [TargetRpc]
         public void TargetInstantiateItems(List<PlayerLobbyController> players)
         {
             BoardManager.instance.setRotationOfBoard(teamSide);
             Debug.Log("generating figures");
             GameController.instance.playersInfoSpawn(teamSide,players);
             GameController.instance.TimersSpawn(teamSide);
             GameController.instance.DestroyedListSpawn(teamSide);
             GameController.instance.PlayerTour();
             GameManager.instance.started = true;
         }

         public void SpawnTiles()
         {
             CmdSpawnTiles();
         }
         [Command(requiresAuthority = false)]
         public void CmdSpawnTiles()
         {
             foreach(var tile in BoardManager.instance.tileList)
             {
                 NetworkServer.Spawn(tile);
             }
         }
         public void SpawnFigures()
         {
             CmdSpawnFigures();
         }
         [Command(requiresAuthority = false)]
         public void CmdSpawnFigures()
         {
             Debug.Log("Spawning figures");
             foreach (var figure in GameManager.instance.whiteTeam)
             {
                 NetworkServer.Spawn(figure);
             }
             foreach (var figure in GameManager.instance.blackTeam)
             {
                 NetworkServer.Spawn(figure);
             }
             foreach (var item in NetworkServer.spawned)
             {
                 Debug.Log(item.Key + " " + item.Value);
             }
         }

         public void SwapSide()
         {
             CmdSwapSide();
         }
         [Command(requiresAuthority = false)]
         public void CmdSwapSide()
         {
             foreach (var player in TurnManager.instance.players)
             {
                 player.GetComponent<PlayerController>().TargetSwapSide();
             }
         }
         [TargetRpc]
         public void TargetSwapSide()
         {
             GameController.instance.SwapSide();
         }
         public void SetTeam()
         {
             CmdSetTeam();
         }
         [Command(requiresAuthority = false)]
         public void CmdSetTeam()
         {
             TurnManager.instance.setSettings();

             foreach (var player in TurnManager.instance.players)
             {
                 player.GetComponent<PlayerController>().TargetSetTeam();
             }



         }
         [TargetRpc]
         public void TargetSetTeam()
         {
             if (teamSide == 0)
             {
                 Team newTeam = new Team(GameManager.instance.whiteTeam);
                 Tiles.Add(newTeam);
             }
             else
             {
                 Team newTeam = new Team(GameManager.instance.blackTeam);
                 Tiles.Add(newTeam);



             }
         }

         public void GenerateFiguresOnline(int startingSide)
         {

         }
         public void SyncChessPos(GameObject chess,Transform parent,PlayerController player,GameObject destroyedChess)
         {
             CmdSyncChessPos(chess, parent, player, destroyedChess);
         }
         [Command(requiresAuthority = false)]
         public void CmdSyncChessPos(GameObject chess, Transform parent, PlayerController player, GameObject destroyedChess)
         {
             foreach (var _player in TurnManager.instance.players)
             {
                 if(_player.nickname != player.GetComponent<PlayerLobbyController>().nickname)
                 {
                     _player.GetComponent<PlayerController>().TargetSyncChessPos(chess, parent, destroyedChess);
                 }
             }
         }
         [TargetRpc]
         public void TargetSyncChessPos(GameObject chess, Transform parent, GameObject destroyedChess)
         {
             Debug.Log(destroyedChess.name);
             Debug.Log(chess.name);
             Chess chessClass = chess.GetComponent<Chess>();
             Debug.Log(chessClass);
             chessClass.transform.SetParent(parent);
             chessClass.transform.localPosition = new Vector2(0, 0);
             chessClass.position = chessClass.getPosition();
             chessClass.index_current_at = Tile.instance.GetCurrentIndex(parent.gameObject);
             /*if (destroyedChess != null)
             {
                 Destroy(destroyedChess);
             }
         }
     */



        public void Reset()
        {
            teamSide = 0;
            myTeam.Clear();
            myDestroyed = null;
            figuresInteractable = false;
        }

        [Command(requiresAuthority = false)]
        public void CmdSwapSide()
        {
            ServerMatchController.instance.SwapSide();
        }
        [TargetRpc]
        public void TargetSwapSide(int _playerTour,List<PlayerLobbyController> players)
        {
            Debug.Log(players.Count);
            Debug.Log(_playerTour + " playerTourFromServer");
            foreach (var player in players)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                Debug.Log(playerController.teamSide + " playerSide");

                if (playerController.teamSide == _playerTour)
                {
                    Debug.Log(player.nickname + "has the same teamSide" + "enabling");
                    foreach (var figure in playerController.myTeam)
                    {
                        if (figure != null)
                        {
                            figure.GetComponent<Button>().interactable = true;
                        }
                    }
                }
                else
                {
                    Debug.Log(player.nickname + "has different teamSide" + "disabling");

                    foreach (var figure in playerController.myTeam)
                    {
                        if (figure != null)
                        {
                            figure.GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }
        }
        [TargetRpc]
        public void TargetCorrectUIElements(int teamSide)
        {
            
            
            
        }
        [TargetRpc]
        public void TargetSetRotationOfBoard(int teamSide)
        {
            BoardController.instance.setRotationOfBoard(teamSide);

        }
        [TargetRpc]
        public void TargetCorrectSpawnPlayersInfo(int teamSide,GameObject wPlayer,GameObject bPlayer)
        {
            GameController.instance.playersInfoCorrect(teamSide,wPlayer,bPlayer);
        }
        [TargetRpc]
        public void TargetCorrectSpawnPlayersTimers(int teamSide,GameObject wTimer,GameObject bTimer)
        {
            GameController.instance.TimersSpawnCorrect(teamSide, wTimer, bTimer);
        }
        [TargetRpc]
        public void TargetCorrectSpawnDestroyedList(int teamSide,GameObject wDestroyed,GameObject bDestroyed)
        {
            GameController.instance.DestroyedListCorrect(teamSide,wDestroyed,bDestroyed);
            
        }
        [Command(requiresAuthority =false)]
        public void CmdSyncObjectParentOnClient(GameObject objectChild, Transform objectParent)
        {
            ServerMatchController.instance.SyncPosition(objectChild, objectParent);
        }
        
        [Command(requiresAuthority = false)]
        public void CmdSyncObjectParentThroughMainParent(GameObject objectChild, GameObject mainParent, string childName)
        {
            foreach (var player in TurnManager.instance.players.players)
            {
                PlayerController _player = player.Key.GetComponent<PlayerController>();
                _player.TargetSyncObjectParentThroughMainParent(objectChild, mainParent, childName);
            }
        }
        
        [TargetRpc]
        public void TargetSyncObjectParentThroughMainParent(GameObject objectChild,GameObject mainParent,string childName)
        {
            
            Debug.Log(playerController);
            foreach (var item in NetworkServer.spawned)
            {
                Debug.Log(item);
            }
            Debug.Log(objectChild);
            Debug.Log(mainParent);
            Debug.Log(childName);
            foreach (Transform item in mainParent.transform.GetChild(0).transform)
            {
                if(item.name == childName)
                {
                    objectChild.GetComponent<RectTransform>().transform.SetParent(item.transform, false);
                }
            }
        }
        [TargetRpc]
        public void TargetSyncObjectParentByTransformParent(GameObject objectChild, Transform parentObj)
        {
            Debug.Log(objectChild.name);
            Debug.Log(parentObj.name);
            objectChild.GetComponent<RectTransform>().transform.SetParent(parentObj.transform, false);
        }
        [TargetRpc]
        public void TargetSyncObjectParentByParentName(uint objID, string parentName)
        {
            Debug.Log(objID);
            foreach (var item in NetworkServer.spawned)
            {
                Debug.Log(item);
            }
            GameObject objToSync = NetworkClient.spawned[objID].gameObject;
            Debug.Log(objToSync.name);

            for (int i = 0; i < BoardManager.instance.tileList.Count; i++)
            {
                if (BoardManager.instance.tileList[i].name == parentName)
                {
                    objToSync.GetComponent<RectTransform>().transform.SetParent(BoardManager.instance.tileList[i].gameObject.transform, false);

                }
            }
        }
        [TargetRpc]
        public void TargetSyncInteractableOnClient(GameObject obj,bool val)
        {
            obj.GetComponent<Button>().interactable = val;
        }

        [TargetRpc]
        public void TargetSyncFigure(string objectName,string parentTag,string childNameSyncTo)
        {
            Debug.Log(childNameSyncTo);
            //GameObject parentObject = GameObject.FindGameObjectWithTag(parentTag);
            //Debug.Log(parentObject);
            GameObject figureObject = GameObject.Find(objectName);
            Debug.Log(figureObject);
            foreach (GameObject item in BoardManager.instance.tileList)
            {
                if(item.name == childNameSyncTo)
                {
                    Debug.Log(childNameSyncTo + " -> " + figureObject);
                    figureObject.GetComponent<RectTransform>().transform.SetParent(item.transform, false);
                }
            }
        }
         

        [Command(requiresAuthority =false)]
        public void CmdPlayerReadyForGame(PlayerLobbyController _player)
        {
            TurnManager.instance.players.SetPlayerReady(_player);
        }

        public void OnFigureInteractableChange(bool _oldValue, bool _newValue)
        {
            if (_newValue == true)
            {
                Debug.Log(myTeam + " side " + teamSide);
                Debug.Log(myTeam + " is being set to " + _newValue);
                foreach (var figure in myTeam)
                {

                    if (figure != null)
                    {
                        figure.GetComponent<Button>().interactable = _newValue;
                    }
                }
            }
            else
            {
                Debug.Log(myTeam + " side " + teamSide);
                Debug.Log(myTeam + " is being set to " + _newValue);

                foreach (var figure in myTeam)
                {
                    if (figure != null)
                    {
                        figure.GetComponent<Button>().interactable = _newValue;
                    }
                }
            }
        }

        [Command(requiresAuthority =false)]
        public void CmdAskForDestroy(GameObject chessToDestroy)
        {
            //Destroy(chessToDestroy.gameObject);
            Debug.Log("Asking server for destroy chess...");
            if(chessToDestroy.tag == "White")
            {
                ServerMatchSettings.instance.WTeam.Remove(chessToDestroy);
                chessToDestroy.GetComponent<RectTransform>().transform.SetParent(
                ServerMatchSettings.instance.wDestroyed.transform.GetChild(0).transform, false);
                foreach (var player in TurnManager.instance.players.players)
                {
                    PlayerController _player = player.Key.GetComponent<PlayerController>();
                    _player.TargetTeamDestroyedUpdate(ServerMatchSettings.instance.WTeam, ServerMatchSettings.instance.wDestroyed, chessToDestroy);
                    
                }
            }
            else
            {
                ServerMatchSettings.instance.BTeam.Remove(chessToDestroy);
                chessToDestroy.GetComponent<RectTransform>().transform.SetParent(
                ServerMatchSettings.instance.bDestroyed.transform.GetChild(0).transform, false);
                foreach (var player in TurnManager.instance.players.players)
                {
                    PlayerController _player = player.Key.GetComponent<PlayerController>();
                    _player.TargetTeamDestroyedUpdate(ServerMatchSettings.instance.BTeam, ServerMatchSettings.instance.bDestroyed, chessToDestroy);
                }
                

            }
            /*if (chessToDestroy.name.Contains("King"))
             {
                 if (chessToDestroy.tag == "White")
                 {
                     string nickname = BoardManager.instance.bPlayerNickname.text;
                     GameController.instance.EndGame($"{nickname} Won!");
                 }
                 else if (chessToDestroy.tag == "Black")
                 {
                     string nickname = BoardManager.instance.wPlayerNickname.text;
                     GameController.instance.EndGame($"{nickname} Won!");
                 }
             }
            */
        }
        [TargetRpc]
        public void TargetTeamDestroyedUpdate(List<GameObject> _myTeam,GameObject _destroyedList,GameObject chessDestroyed)
        {
            _myTeam.Remove(chessDestroyed);
            chessDestroyed.GetComponent<RectTransform>().transform.SetParent(_destroyedList.transform.GetChild(0).transform, false);
            Debug.Log("Chess destroyed and everything is synced.");

        }

        [Command(requiresAuthority =false)]
        public void CmdAskForTeam(int teamVal , PlayerLobbyController _player)
        {
            Debug.Log("Client asking for teamSet");
            ServerMatchSettings.instance.setTeam(teamVal, _player);
        }
        public void OnTeamSideSet(int oldVal, int newVal)
        {
            Debug.Log("Client teamSideSetTo: " + newVal);
            CmdAskForTeam(newVal, PlayerLobbyController.localPlayer);
        }
        [Command(requiresAuthority =false)]
        public void CmdLostDueToTime(PlayerController winner)
        {
            ServerMatchController.instance.EndGame(winner);
        }

        [Command(requiresAuthority = false)]
        public void CmdLostDueToQueen(PlayerController winner)
        {
            ServerMatchController.instance.EndGame(winner);
        }
        [TargetRpc]
        public void TargetEndGame(string notification)
        {
            if (SceneController.instance.activeScene() != "OnlineMode") 
            { 
                GameController.instance.EndGame(notification); 
            }

        }

        
    }


}


