using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class King : MonoBehaviour
{
    Chess chess_global;
    private List<int> kingSpawningPlaces = new List<int>() { -1, 1, -8, 8, -9, 9, -7, 7 };
    void Start()
    {
        chess_global = gameObject.GetComponent<Chess>();
        gameObject.GetComponent<Button>().onClick.AddListener(kingMovementPossibility);

    }
    public void kingMovementPossibility()
    {
        if(chess_global.showing_fields== true)
        {
            chess_global.ClearPossibleMovements();
        }
        else
        {
            kingMovementLauncher();
        }

    }
    public void kingMovementLauncher()
    {
        chess_global.index_current_at = gameObject.transform.parent.gameObject.GetComponent<Tile>().GetCurrentIndex(gameObject.transform.parent.gameObject);
        foreach (var index in kingSpawningPlaces)
        {
            kingMovementCore(index);
        }


        chess_global.ShowPossibleMovement();
        chess_global.GetPossibleCircles();
        GameController.instance.ActiveChess(gameObject);
    }
    public void kingMovementCore(int index)
    {
        //position at which movement will spawn
        int spawning_index = chess_global.index_current_at + index;
        bool rightWayStarting = false;

        if (BoardController.instance.ifWantedToGoExists(spawning_index) == true)
        {

            Vector2 tile_pos = chess_global.getPosition(BoardManager.instance.tileList[spawning_index]);
            if (rightWayStarting == false)
            {
                //which means if FIRST tile is 1sqm left up right or bottom not at top of board while chess is at bottom
                if ((Mathf.Abs(chess_global.position.x - tile_pos.x) == 1 || Mathf.Abs(chess_global.position.x - tile_pos.x) == 0) && (Mathf.Abs(chess_global.position.y - tile_pos.y) == 1 || Mathf.Abs(chess_global.position.y - tile_pos.y) == 0))
                {

                }
                else
                {
                    return;
                }
            }
            if (BoardManager.instance.tileList[spawning_index].transform.childCount > 0)
            {
                if (GameController.instance.IsEnemy(gameObject.tag, BoardManager.instance.tileList[spawning_index].transform.GetChild(0).gameObject.tag) == true)
                {

                    chess_global.movements.Add(BoardManager.instance.tileList[spawning_index]);

                }
                else
                {
                    return;
                }
                
            }
            else
            {
                if ((tile_pos.x == 1 || tile_pos.x == 8) && (tile_pos.y == 1 || tile_pos.y == 8))
                {
                    chess_global.movements.Add(BoardManager.instance.tileList[spawning_index]);
                    
                }
                else
                {
                    chess_global.movements.Add(BoardManager.instance.tileList[spawning_index]);
                }
            }
        }
    }
    
    public void InDanger()
    {

    }
}

