using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace MirrorNetworking
{
  /* public class BoardManagerOnline : NetworkBehaviour
    {
        public void GenerateFigures(int startingSide)
        {
            if (startingSide != 0 && startingSide != 1)
            {
                return;
            }
            BoardManager offlineManager = BoardManager.instance;
            GameManager.instance.white_direction = offlineManager.setDirection(1);
            //White King
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[0], offlineManager.tileList[32]));
            //White Queen
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[1], offlineManager.tileList[24]));
            //White Rooks
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[2], offlineManager.tileList[0]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[2], offlineManager.tileList[56]));

            //White Bishops
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[3], offlineManager.tileList[40]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[3], offlineManager.tileList[16]));
            //White Knights
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[4], offlineManager.tileList[48]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[4], offlineManager.tileList[8]));
            //White Pawns
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[1]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[9]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[17]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[25]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[33]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[41]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[49]));
            GameManager.instance.whiteTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[5], offlineManager.tileList[57]));

            GameManager.instance.black_direction = offlineManager.setDirection(-1);
            //Black King
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[6], offlineManager.tileList[39]));
            //Black Queen
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[7], offlineManager.tileList[31]));

            //Black Rooks
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[8], offlineManager.tileList[7]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[8], offlineManager.tileList[63]));
            //Black Bishops
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[9], offlineManager.tileList[23]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[9], offlineManager.tileList[47]));

            //Black Knights
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[10], offlineManager.tileList[15]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[10], offlineManager.tileList[55]));
            //Black Pawns
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[6]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[14]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[22]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[30]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[38]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[46]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[54]));
            GameManager.instance.blackTeam.Add(offlineManager.SpawnFigure(offlineManager.figuresList[11], offlineManager.tileList[62]));

            if (startingSide == 0)
            {
                lettersBot.reverseArrangement = false;
                numbersLeft.reverseArrangement = true;
                tiles.Rotate(new Vector3(0, 0, 0));
                Debug.Log(tiles.rotation);

                foreach (GameObject tile in tileList)
                {
                    tile.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 0));
                    Debug.Log(tile.GetComponent<RectTransform>().rotation);

                }
            }
            else
            {
                lettersBot.reverseArrangement = true;
                numbersLeft.reverseArrangement = false;
                tiles.Rotate(new Vector3(180, 180, 0));
                Debug.Log(tiles.rotation);
                foreach (GameObject tile in tileList)
                {
                    tile.GetComponent<RectTransform>().Rotate(new Vector3(180, 180, 0));
                    Debug.Log(tile.GetComponent<RectTransform>().rotation);

                }
            }
        }
    }
  */
}

