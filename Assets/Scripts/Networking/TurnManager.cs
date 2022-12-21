using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
namespace MirrorNetworking
{
    public class PlayersInGame
    {
        public Dictionary<PlayerLobbyController, bool> players = new Dictionary<PlayerLobbyController, bool>();
        public int maxPlayers = 2;
        public void AddPlayer(PlayerLobbyController player)
        {
            players.Add(player,false);
        }
        public void SetPlayerReady(PlayerLobbyController _player)
        {
            players[_player] = true;
        }
        public bool CheckIfReady()
        {
            
            int playersReady = 0;
            List<bool> playerValues = players.Values.ToList();
            Debug.Log("Checking if players are ready....");
            for (int i = 0; i < playerValues.Count; i++)
            {
                if(playerValues[i] == true)
                {
                    Debug.Log(playerValues[i] + " is ready...");
                    playersReady += 1;
                }
            }
            if (playersReady == maxPlayers)
            {
                Debug.Log("everyone is ready...");
                return true;
            }
            return false;
        }
        public PlayersInGame() { }
    }
    public class TurnManager : NetworkBehaviour{

        public static TurnManager instance;
        public PlayersInGame players;
        public NetworkMatch networkMatch;
        public bool readyToStart = false;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            checkClients();
        }
        

        [Server]
        public void checkClients()
        {
            StartCoroutine(IfClientsReady());
        }

        public IEnumerator IfClientsReady()
        {
           // Debug.Log("Checking for clients");
            while (true)
            {
                if(players.CheckIfReady() == true)
                {
                   
                    Debug.Log("Both clients loaded");
                    
                    break;
                }
                yield return new WaitForSeconds(1f);
            }
            readyToStart = true;
            ServerMatchController.instance.StartGame();
            yield return true;

        }

    }
}
