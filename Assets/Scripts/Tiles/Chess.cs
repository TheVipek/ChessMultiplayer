using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Chess : NetworkBehaviour
{
    public int orientation;
    public GameObject movementPossibleSprite;
    public List<GameObject> movements = new List<GameObject>();
    public List<GameObject> movementsCircles = new List<GameObject>();
    [SyncVar] public uint figureID;
    public int index_current_at;
    public bool can_move = true;
    public bool showing_fields;
    public Vector2 position;
    public int parentChanged = 0;

    public bool ableToMove = false;
    private void Awake()
    {
        
    }
    private void Start()
    {

        
        // checking for chess gameobject tag to set direction of figure

        if (gameObject.tag == "White")
        {
            orientation = 1;
            movementPossibleSprite = BoardManager.instance.white_possible;
        }
        if (gameObject.tag == "Black")
        {
            orientation = -1;
            movementPossibleSprite = BoardManager.instance.black_possible;
        }

        
    }
    public void entrySettings()
    {
        showing_fields = false;
        index_current_at = gameObject.transform.parent.gameObject.GetComponent<Tile>().GetCurrentIndex(gameObject.transform.parent.gameObject);
        if (gameObject.transform.parent.tag == "Tile")
        {
            position = getPosition();
        }
    }
    /// <summary>
    /// takes list of figure possible movements and if movement is in tilelist display list
    /// </summary>
    /// <param name="indexes"></param>
    public void ShowPossibleMovement()
    {
        if (GetComponent<NetworkIdentity>().hasAuthority == true)
        {
            showing_fields = true;

            foreach (var index in movements)
            {
                if (BoardManager.instance.tileList.IndexOf(index) != -1)
                {
                    int it_index = BoardManager.instance.tileList.IndexOf(index);
                    GameObject instantiated_circle = Instantiate(movementPossibleSprite, BoardManager.instance.tileList[it_index].transform, false);
                    instantiated_circle.name = movementPossibleSprite.name;

                }
            }
        }
    }
    public void GetPossibleCircles()
    {
        foreach (var item in movements)
        {
            for (int i = 0; i < item.transform.childCount; i++)
            {
                if (item.transform.GetChild(i).gameObject.name == movementPossibleSprite.gameObject.name)
                {
                    movementsCircles.Add(item.transform.GetChild(i).gameObject);

                }
            }
        }
    }
    public void ClearPossibleMovements(List<GameObject> movementsList=null)
    {
        if (movementsList != null)
        {
            foreach (var item in movementsList)
            {
                Destroy(item);
            }
            movementsList.Clear();
        }
        else
        {
            foreach (var item in movementsCircles)
            {
                Destroy(item);
            }
            movementsCircles.Clear();
        }
         movements.Clear();
        showing_fields = false;
    }


    public void DestroyChess(GameObject chess)
    {
        Destroy(chess);
    }
    public Vector2 getPosition(GameObject tile = null)
    {
        List<char> str = new List<char>() { };
        if(tile == null)
        {
            for (int i = 0; i < gameObject.transform.parent.name.Length; i++)
            {
                str.Add(gameObject.transform.parent.name[i]);

            }
        }
        else
        {
            for (int i = 0; i < tile.transform.name.Length; i++)
            {
                str.Add(tile.transform.name[i]);

            }
        }
        switch (str[1])
        {
            case 'A': str[1] = '1';
                break;
            case 'B':str[1] = '2';
                break;
            case 'C':str[1] = '3';
                break;
            case 'D':str[1] = '4';
                break;
            case 'E':str[1] = '5';
                break;
            case 'F':str[1] = '6';
                break;
            case 'G':str[1] = '7';
                break;
            case 'H':str[1] = '8';
                break;
            default:
                break;
        }
        return new Vector2( int.Parse(str[1].ToString()), int.Parse(str[0].ToString()));
    }

}

