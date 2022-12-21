using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdvanceSettings : MonoBehaviour
{
    private GameObject active_chess;
    private Pawn pawn_component;
    public List<Sprite> Wchess_images;
    public List<Sprite> Bchess_images;
    public List<Sprite> usingChessImages;
    void Start()
    {
        active_chess = GameManager.instance.active_chess;

        pawn_component = active_chess.GetComponent<Pawn>();

        if(active_chess.tag == "White")
        {
            usingChessImages = Wchess_images;
            optionsWithStyle(new Color32(0, 0, 0, 67));

        }
        else
        {
            usingChessImages = Bchess_images;

            optionsWithStyle(new Color32(67, 67, 67, 67));
        }
        GameController.instance.unInteractableExcept(active_chess);

    }
    void Update()
    {
        
    }
    public void optionsWithStyle(Color32 color_bg)
    {
        gameObject.GetComponent<Image>().color = color_bg;
        for (int i = 0; i < usingChessImages.Count; i++)
        {
            
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.GetComponent<Image>().sprite = usingChessImages[i];
        }
    }
    public void advanceToKnight()
    {
        Destroy(pawn_component);
        active_chess.AddComponent<Knight>();
        active_chess.GetComponent<Image>().sprite = usingChessImages[3];
    }
    public void advanceToBishop()
    {
        Destroy(pawn_component);
        active_chess.AddComponent<Bishop>();
        active_chess.GetComponent<Image>().sprite = usingChessImages[2];
    }
    public void advanceToQueen()
    {
        Destroy(pawn_component);
        active_chess.AddComponent<Queen>();
        active_chess.GetComponent<Image>().sprite = usingChessImages[1];
    }
    public void advanceToRook()
    {
        Destroy(pawn_component);
        active_chess.AddComponent<Rook>();
        active_chess.GetComponent<Image>().sprite = usingChessImages[0];
    }
    public void DestroyChooser()
    {
        BoardManager.instance.advance_showing = false;
        GameController.instance.SwapSide();
        Destroy(gameObject);
    }
    
}
