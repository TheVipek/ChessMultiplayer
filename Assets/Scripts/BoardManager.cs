using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardManager : MonoBehaviour
{

    public static BoardManager instance;
    private bool instance_created;

    [Header("Board Settings")]
    public readonly int x_column, y_row;
    public readonly Transform _boardPosition;
    public readonly float min_x_pos;
    public readonly float min_y_pos;
    public readonly Tile _tilePrefab;
    public readonly float _tileWidth;
    public readonly float _tileHeight;
    public GameObject white_possible, black_possible;
    public List<GameObject> tileList;
    public List<GameObject> figuresList;
    public readonly Color32 w_field_color;
    public readonly Color32 b_field_color;
    public readonly Transform tilesContainer;

    [Header("Options to change dynamically")]
    [SerializeField] public VerticalLayoutGroup numbersLeft;
    [SerializeField] public HorizontalLayoutGroup lettersBot;
    [SerializeField] public RectTransform tiles;

    [Header("Camera")]
    [SerializeField] private Transform camera_pos;

    [Header("Advance Settings")]
    public GameObject advance_choose;
    public Vector2 advance_position;
    public Vector2 advanceOffset;
    public bool advance_showing = false;

    [Header("Timer Settings")]
    public GameObject timerBox;
    public Vector2 timer_position;
    public GameObject Wtimer;
    public GameObject Btimer;

    [Header("Profile Settings")]
    public GameObject playerBox;
    public Vector2 playerPosition;
    public GameObject wPlayer;
    public Text wPlayerNickname;
    public Color32 wPlayerUI;
    public GameObject bPlayer;
    public Text bPlayerNickname;

    public Color32 bPlayerUI;

    [Header("Destroyed Settings")]
    public GameObject destroyedBox;
    public Vector2 destroyedPosition;
    public GameObject wDestroyed;
    public GameObject bDestroyed;

    [Header("Players Info Prefabs")]
    public GameObject whitePlayerInfoPrefab;
    public GameObject blackPlayerInfoPrefab;


    
    private void Awake()
    {
        instance = this;
    }

   




}
