using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int startingSide; // 0 - white , 1 - black , 2 - random
    [SerializeField] public float startingTimeLength;
    public List<GameObject> blackTeam;
    public List<GameObject> whiteTeam;
    public int white_direction, black_direction;
    public GameObject active_chess;
    public List<GameObject> active_chess_tiles;
    public List<GameObject> active_chess_circles;
    public float Wtime;
    public float Btime;
    public GameObject gameBoard;
    [SerializeField] public GameObject endBox;
    public int playerTurn = 0; //0 - white ,1 - black
    public bool started = false;
    public bool isOnline; // option for multiplayer,local will start itself but multiplayer will be triggered by client

    //default values
    private float defaultTime = 600;
    private int defaultSide = 2;
    public float defaultAfterQueue = 5;

   // private GameObject cloneEndBox = null;
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
    private void Start()
    {
        startingTimeLength = defaultTime;
        startingSide = defaultSide;
    }
    public void SetGameLength(int length)
    {
        startingTimeLength = length;
    }
    public void SetStartingSide(int side)
    {
        startingSide = side;
    }
    public void SetStartingTime()
    {
        Wtime = startingTimeLength;
        Btime = startingTimeLength;
    }
}
