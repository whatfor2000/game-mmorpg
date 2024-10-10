using Photon.Pun;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public GameObject playerPrefab;  // Assign the player prefab here
    public Transform[] spawnPoints;  // Predefined spawn points (optional)

    // Define the boundaries for the random spawn position
    public Vector3 spawnAreaMin = new Vector3(-10f, 1f, -10f);  // Minimum X, Y, Z coordinates
    public Vector3 spawnAreaMax = new Vector3(10f, 1f, 10f);    // Maximum X, Y, Z coordinates


    private void Start()
    {
        SpawnPlayer();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is missing! Please assign a player prefab.");
            return;
        }

        // Generate a random position within the defined spawn area
        Vector3 randomSpawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            spawnAreaMin.y,  // Set to the fixed Y coordinate (ground level)
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        // Instantiate the player at the random spawn position
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPosition, Quaternion.identity, 0);
    }
}
