using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Knight : MonoBehaviour
{
    Chess chess_global;
    private Dictionary<string, List<int>> knight_movements = new Dictionary<string, List<int>>();
    //{ "up":[10, -6 ], 17, 15, -10, 6, -17, -15 };
    void Start()
    {
        knight_movements["up"] = new List<int>() { 10, -6 };
        knight_movements["right"] = new List<int>() { 17,15};
        knight_movements["down"] = new List<int>() { -10,6};
        knight_movements["left"] = new List<int>() { -17,-15};
        chess_global = gameObject.GetComponent<Chess>();
        gameObject.GetComponent<Button>().onClick.AddListener(knightMovementPossibility);
    }

    public void knightMovementPossibility()
    {
        if (chess_global.showing_fields == true)
        {
            chess_global.ClearPossibleMovements();
        }
        else
        {
            knightMovementLauncher();

        }
    }
    public void knightMovementLauncher()
    {
        chess_global.index_current_at = gameObject.transform.parent.gameObject.GetComponent<Tile>().GetCurrentIndex(gameObject.transform.parent.gameObject);
        foreach (var item in knight_movements)
        {
            foreach (var item2 in item.Value)
            {
                //for down (chess_global.position.x>1 && chess_global.position.x<8) && (chess_global.position.y > 2 && chess_global.position.y < 7)
                if (item.Key == "up" && (chess_global.position.x >= 0 && chess_global.position.x <= 8) && (chess_global.position.y >= 1 && chess_global.position.y <= 6))
                {
                    knightMovementCore(item2);
                }
                else if (item.Key == "down" && (chess_global.position.x >= 0 && chess_global.position.x <= 8) && (chess_global.position.y >= 3 && chess_global.position.y <= 8))
                {
                    knightMovementCore(item2);
                }
                else if (item.Key == "left" && (chess_global.position.x >= 3 && chess_global.position.x <= 8) && (chess_global.position.y >= 1 && chess_global.position.y <= 8))
                {
                    knightMovementCore(item2);
                }
                else if (item.Key == "right" && (chess_global.position.x >= 1 && chess_global.position.x <= 6) && (chess_global.position.y >= 1 && chess_global.position.y <= 8))
                {
                    knightMovementCore(item2);
                }
            }
        }
    chess_global.ShowPossibleMovement();
    chess_global.GetPossibleCircles();
        GameController.instance.ActiveChess(gameObject);
        
    }
    public void knightMovementCore(int index)
    {

        if (BoardController.instance.ifWantedToGoExists(chess_global.index_current_at + index))
        {
            Vector2 tile_pos = chess_global.getPosition(BoardManager.instance.tileList[chess_global.index_current_at + index]);
            if((Mathf.Abs(chess_global.position.x - tile_pos.x) <= 2 && Mathf.Abs(chess_global.position.y - tile_pos.y) <= 2))
            {
                if (BoardManager.instance.tileList[chess_global.index_current_at + index].transform.childCount > 0)
                {
                    if (GameController.instance.IsEnemy(gameObject.tag, BoardManager.instance.tileList[chess_global.index_current_at + index].transform.GetChild(0).gameObject.tag) == true)
                    {

                        chess_global.movements.Add(BoardManager.instance.tileList[chess_global.index_current_at + index]);
                    }
                }
                else
                {
                    chess_global.movements.Add(BoardManager.instance.tileList[chess_global.index_current_at + index]);

                }
            }
            

        }
        
    }
}
