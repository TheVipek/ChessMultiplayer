using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject gameBoard;
    public GameObject endBox;
    public GameObject SeverGameSettings;
    private void Awake()
    {
        Debug.Log("GameController Awakning");
        instance = this;

    }
    void Start()
    {

    }
    void Update()
    {
        if(SceneController.instance.activeScene() == "LocalGame")
        {
            if (Time.timeScale == 1 && GameManager.instance.started == true)
            {
                if (GameManager.instance.playerTurn == 0)
                {
                    BoardManager.instance.Btimer.GetComponent<TimeRefresher>().timeStop();
                    BoardManager.instance.Wtimer.GetComponent<TimeRefresher>().timeStart();
                }
                else if (GameManager.instance.playerTurn == 1)
                {
                    BoardManager.instance.Wtimer.GetComponent<TimeRefresher>().timeStop();
                    BoardManager.instance.Btimer.GetComponent<TimeRefresher>().timeStart();
                }

                if (BoardManager.instance.Btimer.GetComponent<TimeRefresher>().time_over == true && endBox.gameObject.activeSelf == false)
                {
                    string nickname = BoardManager.instance.wPlayerNickname.text;
                    EndGame($"{nickname} Won!");

                }
                else if (BoardManager.instance.Wtimer.GetComponent<TimeRefresher>().time_over == true && endBox.gameObject.activeSelf == false)
                {
                    string nickname = BoardManager.instance.bPlayerNickname.text;
                    EndGame($"{nickname} Won!");

                }
            }
        }
        


    }


    public void StartGameOffline()
    {
        GameManager.instance.SetStartingTime();
        if (GameManager.instance.startingSide == 2) // which means that user selected that he wants to start randomly (for now only VS friend)
        {
            GameManager.instance.startingSide = Random.Range(0, 2);
        }
        //IF POS 0 WHITE BOTTOM BLACK UPPER ELSE REVERSED
        BoardController.instance.GenerateFigures();
        BoardController.instance.setRotationOfBoard(GameManager.instance.startingSide);
        //spawnReadyUIPrefab(startingSide);
        playersInfoSpawn(GameManager.instance.startingSide);
        TimersSpawn(GameManager.instance.startingSide);
        DestroyedListSpawn(GameManager.instance.startingSide);
        PlayerTour();
        GameManager.instance.started = true;

    }
    public void PlayerTour()
    {
        if (GameManager.instance.playerTurn == 0)
        {
            foreach (var item in GameManager.instance.blackTeam)
            {
                if (item != null)
                {
                    item.GetComponent<Button>().interactable = false;
                }
            }
            foreach (var item in GameManager.instance.whiteTeam)
            {
                if (item != null)
                {
                    item.GetComponent<Button>().interactable = true;
                }
            }
        }
        else
        {
            foreach (var item in GameManager.instance.blackTeam)
            {
                if (item != null)
                {
                    item.GetComponent<Button>().interactable = true;
                }
            }
            foreach (var item in GameManager.instance.whiteTeam)
            {
                if (item != null)
                {
                    item.GetComponent<Button>().interactable = false;
                }
            }
        }
    }
    public void SwapSide()
    {
        if (GameManager.instance.playerTurn == 0)
        {
            GameManager.instance.playerTurn = 1;
        }
        else
        {
            GameManager.instance.playerTurn = 0;
        }
        PlayerTour();

    }
    public void pauseGame()
    {
        Time.timeScale = 0;
    }
    public void unpauseGame()
    {
        Time.timeScale = 1;
    }
    public void unInteractableExcept(GameObject chess)
    {
        if (chess.tag == "White")
        {
            foreach (var item in GameManager.instance.whiteTeam)
            {
                if (item != null && (item.GetComponent<Chess>().index_current_at != chess.GetComponent<Chess>().index_current_at))
                {
                    item.GetComponent<Button>().interactable = false;
                }
            }
        }
        else if (chess.tag == "Black")
        {
            foreach (var item in GameManager.instance.blackTeam)
            {
                if (item != null && (item.GetComponent<Chess>().index_current_at != chess.GetComponent<Chess>().index_current_at))
                {
                    item.GetComponent<Button>().interactable = false;
                }
            }
        }

    }

    public void TimersSpawn(int StartingSide)
    {
        int correctPos;
        if (StartingSide == -1)
        {
            correctPos = -1;
        }
        else
        {
            correctPos = 1;
        }
        // multiplying it by white/black direction is just reversing their position  example (X:740 to -740 Y:280 to -280)
        //Wtimer instantiate and set it in BoardManager
        BoardManager.instance.Wtimer = Instantiate(BoardManager.instance.timerBox, gameBoard.transform, false);
        BoardManager.instance.Wtimer.tag = "White";
        BoardManager.instance.Wtimer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.timer_position.x * 1) * correctPos,
                                                                                    (BoardManager.instance.timer_position.y * 1) * correctPos
                                                                                    );
        //Wtimer color
        timerBoxUI WTimerBoxUI = BoardManager.instance.Wtimer.GetComponent<timerBoxUI>();
        WTimerBoxUI.bgColor = BoardManager.instance.wPlayerUI;
        WTimerBoxUI.textColor = BoardManager.instance.bPlayerUI;
        WTimerBoxUI.time_left = GameManager.instance.Wtime;
        //WTimerBoxUI.GetComponent<TimeRefresher>().time_left = GameManager.instance.Wtime;

        //BoardManager.instance.Wtimer.GetComponent<Image>().color = BoardManager.instance.wPlayerUI;

        //Getting 1st child Wtimer (time left)
        //BoardManager.instance.Wtimer = BoardManager.instance.Wtimer.transform.GetChild(0).gameObject;
        //Setting color for child Wtimer
        //BoardManager.instance.Wtimer.GetComponent<Text>().color = BoardManager.instance.bPlayerUI;

        //Entry value for Wtimer

        //Btimer instantiate and set it in BoardManager
        BoardManager.instance.Btimer = Instantiate(BoardManager.instance.timerBox, gameBoard.transform, false);
        BoardManager.instance.Btimer.tag = "Black";

        BoardManager.instance.Btimer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.timer_position.x * -1) * correctPos,
                                                                                    (BoardManager.instance.timer_position.y * -1) * correctPos
                                                                                    );

        timerBoxUI BTimerBoxUI = BoardManager.instance.Btimer.GetComponent<timerBoxUI>();
        BTimerBoxUI.bgColor = BoardManager.instance.bPlayerUI;
        BTimerBoxUI.textColor = BoardManager.instance.wPlayerUI;
        BTimerBoxUI.time_left = GameManager.instance.Btime;
        //Btimer color
        //BoardManager.instance.Btimer.GetComponent<Image>().color = BoardManager.instance.bPlayerUI;
        //Getting 1st child Btimer (time left)
        //BoardManager.instance.Btimer = BoardManager.instance.Btimer.transform.GetChild(0).gameObject;
        //Setting color for child Btimer
        //BoardManager.instance.Btimer.GetComponent<Text>().color = BoardManager.instance.wPlayerUI;
        //Entry value for Btimer
        //BoardManager.instance.Btimer.GetComponent<TimeRefresher>().time_left = GameManager.instance.Btime;

        //SET FROM SERVER

        //SET LOCALLY

    }
    public void playersInfoSpawn(int StartingSide,List<MirrorNetworking.PlayerLobbyController> players =null)
    {
        int correctPos;
        if (StartingSide == -1)
        {
            correctPos = -1;
        }
        else
        {
            correctPos = 1;
        }


        //White player info
        BoardManager.instance.wPlayer = Instantiate(BoardManager.instance.playerBox, gameBoard.transform, false);

        BoardManager.instance.wPlayer.tag = "White";
        BoardManager.instance.wPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.playerPosition.x * 1) * correctPos,
                                                                                    (BoardManager.instance.playerPosition.y * 1) * correctPos
                                                                                    );
        playerNameUI wPlayerNameUI = BoardManager.instance.wPlayer.GetComponent<playerNameUI>();
        wPlayerNameUI.bgColor = BoardManager.instance.wPlayerUI;
        wPlayerNameUI.textColor = BoardManager.instance.bPlayerUI;
        wPlayerNameUI.imgColor = BoardManager.instance.bPlayerUI;

        //box color
        //BoardManager.instance.wPlayer.GetComponent<Image>().color = BoardManager.instance.wPlayerUI;
        //text color
        //BoardManager.instance.wPlayerNickname = BoardManager.instance.wPlayer.transform.GetChild(0).gameObject.GetComponent<Text>();
        //BoardManager.instance.wPlayerNickname.color = BoardManager.instance.bPlayerUI;
        //image color
        //BoardManager.instance.wPlayer.transform.GetChild(1).gameObject.GetComponent<Image>().color = BoardManager.instance.bPlayerUI;


        //Black player info
        BoardManager.instance.bPlayer = Instantiate(BoardManager.instance.playerBox, gameBoard.transform, false);
        BoardManager.instance.bPlayer.tag = "Black";
        BoardManager.instance.bPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.playerPosition.x * -1) * correctPos,

                                                                                    (BoardManager.instance.playerPosition.y * -1) * correctPos
                                                                                    );
        playerNameUI bPlayerNameUI = BoardManager.instance.bPlayer.GetComponent<playerNameUI>();
        bPlayerNameUI.bgColor = BoardManager.instance.bPlayerUI;
        bPlayerNameUI.textColor = BoardManager.instance.wPlayerUI;
        bPlayerNameUI.imgColor = BoardManager.instance.wPlayerUI;

        //box color
        //BoardManager.instance.bPlayer.GetComponent<Image>().color = BoardManager.instance.bPlayerUI;
        //text color
       //BoardManager.instance.bPlayerNickname = BoardManager.instance.bPlayer.transform.GetChild(0).gameObject.GetComponent<Text>();
       // BoardManager.instance.bPlayerNickname.color = BoardManager.instance.wPlayerUI;
        //image color
        //BoardManager.instance.bPlayer.transform.GetChild(1).gameObject.GetComponent<Image>().color = BoardManager.instance.wPlayerUI;

        if (players != null)
        {
            foreach (var player in players)
            {
                int teamside = player.GetComponent<MirrorNetworking.PlayerController>().teamSide;
                if(teamside == -1)
                {
                    wPlayerNameUI.nickname = player.nickname;
                }
                else
                {
                    bPlayerNameUI.nickname = player.nickname;

                }
            }
        }
        else
        {
            wPlayerNameUI.nickname = "Player 1";
            bPlayerNameUI.nickname = "Player 2";

        }

        if (SceneController.instance.activeScene() == "OnlineGame")
        {
            MirrorNetworking.ServerMatchController.instance.SpawnWantedObject(BoardManager.instance.wPlayer);
            MirrorNetworking.ServerMatchController.instance.SpawnWantedObject(BoardManager.instance.bPlayer);

        }
        Debug.Log("players info ended");


    }
    public void DestroyedListSpawn(int StartingSide)
    {
        int correctPos;
        if (StartingSide == -1)
        {
            correctPos = -1;
        }
        else
        {
            correctPos = 1;
        }
        //White player info
        BoardManager.instance.wDestroyed = Instantiate(BoardManager.instance.destroyedBox, gameBoard.transform, false);
        BoardManager.instance.wDestroyed.tag = "White";
        BoardManager.instance.wDestroyed.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.destroyedPosition.x * 1) * correctPos,
                                                                                    (BoardManager.instance.destroyedPosition.y * 1) * correctPos);
        DestroyedListController WDestroyedBox = BoardManager.instance.wDestroyed.GetComponent<DestroyedListController>();
        WDestroyedBox.objName= "wDestroyed";
        WDestroyedBox.bgColor = BoardManager.instance.wPlayerUI;
        WDestroyedBox.childBgColor = new Color32(BoardManager.instance.wPlayerUI.r, BoardManager.instance.wPlayerUI.g, BoardManager.instance.wPlayerUI.b, 20);
        //box color
        //BoardManager.instance.wDestroyed.GetComponent<Image>().color = BoardManager.instance.wPlayerUI;
        //image color
        //BoardManager.instance.wDestroyed.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(BoardManager.instance.wPlayerUI.r, BoardManager.instance.wPlayerUI.g, BoardManager.instance.wPlayerUI.b, 20);

        //Black player info
        BoardManager.instance.bDestroyed = Instantiate(BoardManager.instance.destroyedBox, gameBoard.transform, false);
        BoardManager.instance.bDestroyed.tag = "Black";
        BoardManager.instance.bDestroyed.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.destroyedPosition.x * -1) * correctPos,
                                                                                    (BoardManager.instance.destroyedPosition.y * -1) * correctPos);
        DestroyedListController BDestroyedBox = BoardManager.instance.bDestroyed.GetComponent<DestroyedListController>();
        BDestroyedBox.objName = "bDestroyed";
        BDestroyedBox.bgColor = BoardManager.instance.bPlayerUI;
        BDestroyedBox.childBgColor = new Color32(BoardManager.instance.bPlayerUI.r, BoardManager.instance.bPlayerUI.g, BoardManager.instance.bPlayerUI.b, 20);
        //box color
        //BoardManager.instance.bDestroyed.GetComponent<Image>().color = BoardManager.instance.bPlayerUI;
        //image color
        //BoardManager.instance.wDestroyed.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(BoardManager.instance.bPlayerUI.r, BoardManager.instance.bPlayerUI.g, BoardManager.instance.bPlayerUI.b, 20);
    }
    
    

    public void TimersSpawnCorrect(int StartingSide,GameObject wTimer,GameObject bTimer)
    {
        int correctPos;
        if (StartingSide == -1)
        {
            correctPos = -1;
        }
        else
        {
            correctPos = 1;
        }

        wTimer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.timer_position.x * 1) * correctPos,
                                                                                   (BoardManager.instance.timer_position.y * 1) * correctPos);

        bTimer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.timer_position.x * -1) * correctPos,
                                                                                    (BoardManager.instance.timer_position.y * -1) * correctPos);

    }
    public void playersInfoCorrect(int StartingSide,GameObject wPlayer,GameObject bPlayer)
    {
        int correctPos;
        if (StartingSide == -1)
        {
            correctPos = -1;
        }
        else
        {
            correctPos = 1;
        }
        wPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.playerPosition.x * 1) * correctPos,
                                                                            (BoardManager.instance.playerPosition.y * 1) * correctPos);
        bPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.playerPosition.x *-1) * correctPos,
                                                                                   (BoardManager.instance.playerPosition.y *-1) * correctPos);
    }
    public void DestroyedListCorrect(int StartingSide,GameObject wDestroyed,GameObject bDestroyed)
    {
        int correctPos;
        if (StartingSide == -1)
        {
            correctPos = -1;
        }
        else
        {
            correctPos = 1;
        }
        wDestroyed.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.destroyedPosition.x * 1) * correctPos,
                                                                                    (BoardManager.instance.destroyedPosition.y * 1) * correctPos);
        bDestroyed.GetComponent<RectTransform>().anchoredPosition = new Vector2((BoardManager.instance.destroyedPosition.x * -1) * correctPos,
                                                                                    (BoardManager.instance.destroyedPosition.y * -1) * correctPos);

    }
    
    public void EndGame(string end_text)
    {
        endBox.SetActive(true);
        endBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject text = endBox.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        text.GetComponent<Text>().text = end_text;
    }
    public void ActiveChess(GameObject chess)
    {
        if ((GameManager.instance.active_chess_circles.Count != 0 || GameManager.instance.active_chess_circles.Count != 0) && chess != GameManager.instance.active_chess)
        {
            GameManager.instance.active_chess.GetComponent<Chess>().ClearPossibleMovements(GameManager.instance.active_chess_circles);
        }
        GameManager.instance.active_chess = chess;
        GameManager.instance.active_chess_tiles = chess.GetComponent<Chess>().movements;
        GameManager.instance.active_chess_circles = chess.GetComponent<Chess>().movementsCircles;
    }
    public bool IsEnemy(string chess1Tag, string chess2Tag)
    {
        if ((chess1Tag == "Black" || chess1Tag == "White") && (chess2Tag == "Black" || chess2Tag == "White"))
        {
            if (chess1Tag != chess2Tag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;

    }
}
