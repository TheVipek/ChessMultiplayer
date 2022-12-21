using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pawn : MonoBehaviour
{
    public bool first_move = true;
    Chess chess_global;
    public int prev_index_at;
    
    private void Start()
    {
        chess_global = gameObject.GetComponent<Chess>();
        prev_index_at = chess_global.index_current_at;
        gameObject.GetComponent<Button>().onClick.AddListener(pawnMovementPossibility);
        

    }



    /// <summary>
    /// shows possible pawn movements
    /// -> take tile index actually at  
    /// -> check's if movements list is empty if not list is being cleared to not stack different moves 
    /// -> check's whether it's first move and if call FirstPawnMovement else NormalPawnMovement
    /// -> and after all call ActiveChess() function which lets know that we clicked at X chess and X chess has X possible movements
    /// </summary>
    public void pawnMovementPossibility()
    {
        
        

            chess_global.index_current_at = gameObject.transform.parent.gameObject.GetComponent<Tile>().GetCurrentIndex(gameObject.transform.parent.gameObject);
        if (chess_global.showing_fields == true)
            {
                chess_global.ClearPossibleMovements();
            }
            else
            {
                EnemyInFront(chess_global.movements, new List<int>() { 9, -7 });
                if ((chess_global.position.y > 1 && chess_global.position.y < 8))
                {
                    if (chess_global.can_move == true)
                    {
                        if (chess_global.index_current_at != prev_index_at)
                        {
                            first_move = false;
                        }
                        if (first_move == true)
                        {
                            pawnMovementLauncher(2);
                        }
                        else
                        {
                            pawnMovementLauncher(1);
                        }
                    }
                    if (chess_global.can_move == false)
                    {
                        pawnAttackLauncher();
                    }
                }
                chess_global.GetPossibleCircles();
                GameController.instance.ActiveChess(gameObject);

            }
        
        

        

    }
    public void pawnMovementLauncher(int amount_of_movements)
    {
        if (amount_of_movements <= 0)
        {
            return;
        }

        for (int i = 1; i <= amount_of_movements; i++)
        {
            chess_global.movements.Add(BoardManager.instance.tileList[chess_global.index_current_at + i * chess_global.orientation]);
        }
        EnemyInFront(chess_global.movements,new List<int>() { 9, -7 });
        chess_global.ShowPossibleMovement();
    }
    public bool EnemyInFront(List<GameObject> front_movements, List<int> side_positions)
    {
        for (int i = 0; i < front_movements.Count; i++)
        {
            //if has child (figure)
            if (front_movements[i].transform.childCount > 0)
            {
                //if enemy is in next tile we can't move anywhere
                if (front_movements.Count >= 1 && chess_global.movements[0] == front_movements[i])
                {
                    chess_global.ClearPossibleMovements();
                    chess_global.can_move = false;
                    return true;
                //if eveny is 2 tiles above we can move 1 square up
                }else if(chess_global.movements.Count > 1 && chess_global.movements[1] == front_movements[i])
                {
                    chess_global.movements.RemoveAt(i);
                    chess_global.movementsCircles.RemoveAt(i);
                    chess_global.can_move = true;
                    return false;
                }
            }
        }
        for (int i = 0; i < side_positions.Count; i++)
        {
            if (BoardController.instance.ifWantedToGoExists(chess_global.index_current_at + side_positions[i] * chess_global.orientation))
            {
                if (BoardManager.instance.tileList[chess_global.index_current_at + side_positions[i] * chess_global.orientation].transform.childCount > 0)
                {
                    if (GameController.instance.IsEnemy(gameObject.tag, BoardManager.instance.tileList[chess_global.index_current_at + side_positions[i] * chess_global.orientation].transform.GetChild(0).gameObject.tag) == true)
                    {
                        chess_global.can_move = false;
                        return true;
                    }
                }
            }            
        }
        chess_global.can_move = true;
        return false;
    }
    public void pawnAttackLauncher()
    {
        //add movement at ^> from Pawn
        if (BoardController.instance.ifWantedToGoExists(chess_global.index_current_at + 9 * chess_global.orientation))
        {
            chess_global.movements.Add(BoardManager.instance.tileList[chess_global.index_current_at + 9 * chess_global.orientation]);
        }
        //add movement at ^< from Pawn
        if (BoardController.instance.ifWantedToGoExists(chess_global.index_current_at - 7 * chess_global.orientation))
        {
            chess_global.movements.Add(BoardManager.instance.tileList[chess_global.index_current_at - 7 * chess_global.orientation]);
        }
        foreach (var item in chess_global.movements.ToArray()) 
        {
            //if nothing is there then player shouldn't be able to move there
            if(item.transform.childCount == 0)
            {
                chess_global.movements.Remove(item);
                
            }// if theres 'something' but it's not enemy figure it would be removed
            else if (item.transform.childCount > 0)
            {
                if (GameController.instance.IsEnemy(gameObject.tag, item.transform.GetChild(0).gameObject.tag) == false)
                {
                    chess_global.movements.Remove(item);

                }
            }
        }
        //if any movements left (which means that there's enemy at ^< or ^> PossibleMovement would be seen 
        if (chess_global.movements.Count > 0)
        {
            chess_global.ShowPossibleMovement();
        }
    }
}
