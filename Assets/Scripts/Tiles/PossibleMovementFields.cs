using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PossibleMovementFields : MonoBehaviour
{
    public void MoveTo()
    {
        GameObject possible_circle = EventSystem.current.currentSelectedGameObject.gameObject;
        GameObject circle_tile = possible_circle.transform.parent.gameObject;
        List<GameObject> movements_list = GameManager.instance.active_chess_tiles;
        Chess chess = GameManager.instance.active_chess.GetComponent<Chess>();
        Transform parent;
        GameObject chessToDestroy = null;
        for (int i = 0; i < movements_list.Count; i++)
        {
            //Debug.Log(BoardManager.instance.tileList.IndexOf(GameManager.instance.active_chess_movements[i]));
            int item_index = BoardManager.instance.tileList.IndexOf(movements_list[i]);
             if (circle_tile.gameObject == BoardManager.instance.tileList[item_index].gameObject)
             {
                //SETTING CHESS POS
                parent = gameObject.transform.parent.transform;
                chess.transform.SetParent(parent);
                chess.transform.localPosition = new Vector2(0, 0);
                chess.ClearPossibleMovements();
                chess.position = chess.getPosition();
                chess.index_current_at = gameObject.transform.parent.gameObject.GetComponent<Tile>().GetCurrentIndex(gameObject.transform.parent.gameObject);

                //DESTROYING OPPONENT IF IS THERE
                if (circle_tile.transform.childCount >2)
                {

                    GameObject circleChess = circle_tile.transform.GetChild(0).gameObject;
                    Debug.Log(circleChess);
                    chessToDestroy = circleChess;
                    if (circleChess.tag != chess.tag)
                    {

                        if (circleChess.name.Contains("King"))
                        {
                            Debug.Log(MirrorNetworking.PlayerController.playerController);
                            MirrorNetworking.PlayerController.playerController.CmdLostDueToQueen(MirrorNetworking.PlayerController.playerController);

                            /*if (circleChess.tag == "White")
                            {
                                MirrorNetworking.ServerMatchController.instance.EndGame(MirrorNetworking.PlayerController.playerController);
                                //string nickname = BoardManager.instance.bPlayerNickname.text;
                                //GameController.instance.EndGame($"{nickname} Won!");
                            }
                            else if (circleChess.tag == "Black")
                            {
                                MirrorNetworking.ServerMatchController.instance.EndGame(MirrorNetworking.PlayerController.playerController);
                                //string nickname = BoardManager.instance.wPlayerNickname.text;
                                //GameController.instance.EndGame($"{nickname} Won!");
                            }*/
                        }
                        MirrorNetworking.PlayerController.playerController.CmdAskForDestroy(circleChess.gameObject);
                        SoundController.instance.placeChessSound();
                        /*Destroy(circleChess.gameObject);
                        if (circleChess.tag == "White")
                        {
                            Instantiate(circleChess.gameObject, BoardManager.instance.wDestroyed.transform.GetChild(0).transform, true);

                        }
                        else if (circleChess.tag == "Black")
                        {
                            Instantiate(circleChess.gameObject, BoardManager.instance.bDestroyed.transform.GetChild(0).transform, true);

                        }*/
                        
                    }
                }


                //ADVANCING FOR PAWNS IF REACHING LAST TILE
                if (chess.name.Contains("Pawn") && chess.TryGetComponent(out Pawn pawn_component) == true)
                {
                    if(chess.position.y == 8 || chess.position.y == 1 && BoardManager.instance.advance_showing == false)
                    {
                        BoardController.instance.SpawnAdvancer(chess);
                    }
                   
                }
                if (BoardManager.instance.advance_showing == false)
                {
                    if(SceneManager.GetActiveScene().name == "LocalGame")
                    {
                        GameController.instance.SwapSide();
                    }
                    else
                    {
                        Debug.Log("Swapping Side...");
                        MirrorNetworking.PlayerController.playerController.CmdSwapSide();
                        MirrorNetworking.PlayerController.playerController.CmdSyncObjectParentThroughMainParent(chess.gameObject,
                            BoardController.instance.gameObject, parent.name);
                    }
                }
                break;
             }
            

        }
    }
}
