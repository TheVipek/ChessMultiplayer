using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardController : MonoBehaviour
{
    public static BoardController instance;
    private void Awake()
    {
        instance = this;
    }

    public void GenerateBoard()
    {
        for (int x = 0; x < BoardManager.instance.x_column; x++)
        {
            for (int y = 0; y < BoardManager.instance.y_row; y++)
            {
                Debug.Log(x + " +" + y);
                var spawnedTile = Instantiate(BoardManager.instance._tilePrefab, BoardManager.instance._boardPosition);
                spawnedTile.transform.SetParent(BoardManager.instance._boardPosition, true);

                spawnedTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * BoardManager.instance._tileWidth + BoardManager.instance.min_x_pos, y * BoardManager.instance._tileHeight + BoardManager.instance.min_y_pos);

                ColorSetter(x, y, spawnedTile);
            }
        }
    }
    void ColorSetter(int x_column, int y_row, Tile spawnedTile)
    {
        if (x_column % 2 == 0 && y_row % 2 != 0)
        {

            spawnedTile.GetComponent<Image>().color = BoardManager.instance.w_field_color;
        }
        else if (x_column % 2 != 0 && y_row % 2 == 0)
        {
            spawnedTile.GetComponent<Image>().color = BoardManager.instance.w_field_color;
        }
        else
        {
            spawnedTile.GetComponent<Image>().color = BoardManager.instance.b_field_color;
        }
    }
    public GameObject SpawnFigure(GameObject figure, GameObject tile_to_put_in)
    {
        GameObject figure_instance = Instantiate(figure, tile_to_put_in.transform, false);
        Rect rect = figure_instance.GetComponent<RectTransform>().rect;
        figure_instance.GetComponent<RectTransform>().rect.Set(10, 0, rect.width, rect.height);
        return figure_instance;
    }

    public void SpawnAdvancer(Chess chess)
    {
        BoardManager.instance.advance_showing = true;
        GameObject advanceChooser = null;
        Vector2 chess_pos = chess.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log(chess.transform.parent);

        advanceChooser = Instantiate(BoardManager.instance.advance_choose, BoardManager.instance.tilesContainer, false);



        if (advanceChooser == null)
        {
            return;
        }
        advanceChooser.GetComponent<RectTransform>().anchoredPosition = new Vector2((chess_pos.x + BoardManager.instance.advanceOffset.x), (350 * chess.orientation));
    }
    public void GenerateFigures()
    {
        
        GameManager.instance.white_direction = setDirection(1);
        //White King
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[0], BoardManager.instance.tileList[32]));
        //White Queen
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[1], BoardManager.instance.tileList[24]));
        //White Rooks
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[2], BoardManager.instance.tileList[0]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[2], BoardManager.instance.tileList[56]));

        //White Bishops
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[3], BoardManager.instance.tileList[40]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[3], BoardManager.instance.tileList[16]));
        //White Knights
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[4], BoardManager.instance.tileList[48]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[4], BoardManager.instance.tileList[8]));
        //White Pawns
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[1]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[9]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[17]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[25]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[33]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[41]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[49]));
        GameManager.instance.whiteTeam.Add(SpawnFigure(BoardManager.instance.figuresList[5], BoardManager.instance.tileList[57]));

        GameManager.instance.black_direction = setDirection(-1);
        //Black King
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[6], BoardManager.instance.tileList[39]));
        //Black Queen
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[7], BoardManager.instance.tileList[31]));

        //Black Rooks
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[8], BoardManager.instance.tileList[7]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[8], BoardManager.instance.tileList[63]));
        //Black Bishops
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[9], BoardManager.instance.tileList[23]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[9], BoardManager.instance.tileList[47]));

        //Black Knights
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[10], BoardManager.instance.tileList[15]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[10], BoardManager.instance.tileList[55]));
        //Black Pawns
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[6]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[14]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[22]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[30]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[38]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[46]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[54]));
        GameManager.instance.blackTeam.Add(SpawnFigure(BoardManager.instance.figuresList[11], BoardManager.instance.tileList[62]));

    }
    public void setRotationOfBoard(int startingSide)
    {
        if (startingSide == 1)
        {
            BoardManager.instance.lettersBot.reverseArrangement = false;
            BoardManager.instance.numbersLeft.reverseArrangement = true;
            BoardManager.instance.tiles.Rotate(new Vector3(0, 0, 0));


            for (int i = 0; i < BoardManager.instance.tileList.Count; i++)
            {
                RectTransform tileRect = BoardManager.instance.tileList[i].GetComponent<RectTransform>();
                tileRect.Rotate(new Vector3(0, 0, 0));

            }
            /*foreach (GameObject tile in tileList)
            {
                tile.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 0));
                //Debug.Log(tile.GetComponent<RectTransform>().rotation);

            }*/
        }
        else
        {
            BoardManager.instance.lettersBot.reverseArrangement = true;
            BoardManager.instance.numbersLeft.reverseArrangement = false;
            BoardManager.instance.tiles.Rotate(new Vector3(180, 180, 0));
            for (int i = 0; i < BoardManager.instance.tileList.Count; i++)
            {
                RectTransform tileRect = BoardManager.instance.tileList[i].GetComponent<RectTransform>();
                tileRect.Rotate(new Vector3(180, 180, 0));

            }
            /*foreach (GameObject tile in tileList)
            {
                tile.GetComponent<RectTransform>().Rotate(new Vector3(180, 180, 0));
               // Debug.Log(tile.GetComponent<RectTransform>().rotation);

            }*/
        }
    }

    public int setDirection(int direction)
    {
        return direction;
    }
    public bool ifWantedToGoExists(int index)
    {
        if (index >= 0 && BoardManager.instance.tileList.Count > index)
        {
            return true;
        }
        return false;
    }
}
