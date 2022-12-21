using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameControllerOnline : NetworkBehaviour
{
    public static GameControllerOnline instance;
    [SyncVar] public List<GameObject> blackTeam;
    [SyncVar] public List<GameObject> whiteTeam;
    public int white_direction, black_direction;
    public GameObject active_chess;
    public List<GameObject> active_chess_tiles;
    public List<GameObject> active_chess_circles;
    private float time_left;
    public float Wtime;
    public float Btime;
    public GameObject gameBoard;
    private int startingSide;
    [SerializeField] public GameObject endBox;
    public int playerTurn = 0; //0 - white ,1 - black
    private bool started = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
