using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MirrorNetworking
{ 

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject nickname;
    public GameObject readyState;
        private bool turnedOn = false;
        private bool turnedOff = false;
        PlayerLobbyController player;

    public void SetPlayer(PlayerLobbyController p)
    {
            player = p;
            nickname.GetComponent<TextMeshProUGUI>().text = p.nickname;
    }
    public void SetReady()
    {
            if(player.GetComponent<PlayerLobbyController>().isReady == false)
            {
                readyState.GetComponent<TextMeshProUGUI>().color = player.playerLobbyUIcolor;

            }
            else
            {
                readyState.GetComponent<TextMeshProUGUI>().color = player.playerLobbyUIcolor;

            }
            
        }
    public void setVisibility()
        {
            
            if (player.GetComponent<PlayerLobbyController>().isReady && turnedOn == false)
            {
                readyState.SetActive(true);
                turnedOn = true;
                turnedOff = false;
            }
            else if (!player.GetComponent<PlayerLobbyController>().isReady && turnedOff == false)
            {
                readyState.SetActive(false);
                turnedOff = true;
                turnedOn = false;
            }
        }
    void Start()
    {
        
    }
    void Update()
    {
           /* if (player.GetComponent<Player>().isReady && turnedOn == false)
            {
                Debug.Log(player);
                Debug.Log(player.GetComponent<Player>().isReady);
                readyState.SetActive(true);
                turnedOn = true;
                turnedOff = false;
            }
            else if (!player.GetComponent<Player>().isReady && turnedOff == false)
            {
                Debug.Log(player);
                Debug.Log(player.GetComponent<Player>().isReady);
                readyState.SetActive(false);
                turnedOff = true;
                turnedOn = false;
            }
           */
        }
}
}
