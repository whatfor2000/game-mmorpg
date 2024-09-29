using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Assign your player prefab in the Inspector
    public Transform[] spawnPoints; // Assign spawn points in the Inspector

    public void Start()
    {
        // Check if we are the Master Client to manage player spawning
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayers();
        }

    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            // Randomly select a spawn point for each player
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void ChangePlayerClass(PlayerController playerController, Weapon newWeapon)
    {
        playerController.ChangeClass(newWeapon);
    }

    // Optionally implement game state management methods
    public void StartGame()
    {
        // Logic to start the game
    }

    public void EndGame()
    {
        // Logic to end the game
    }
}
