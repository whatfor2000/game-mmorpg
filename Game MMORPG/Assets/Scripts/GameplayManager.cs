using Photon.Pun;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public GameObject playerPrefab;  // Assign the player prefab here
    public Transform[] spawnPoints;  // Predefined spawn points (optional)

    private void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is missing! Please assign a player prefab.");
            return;
        }

        Vector3 spawnPosition;

        // If you have spawn points, choose one at random, otherwise spawn at a random position
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPosition = spawnPoint.position;
        }
        else
        {
            // Generate a random position if there are no predefined spawn points
            spawnPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        }

        // Instantiate the player using PhotonNetwork.Instantiate
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
