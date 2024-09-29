using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public void StartGame()
    {
        // Start hosting
        NetworkRunner runner = gameObject.AddComponent<NetworkRunner>();
        runner.StartGame(new StartGameArgs()
        {
            Scene = 1, // Set your desired scene index
            GameMode = GameMode.Host
        });
    }

    public override void Spawned(NetworkRunner runner, PlayerRef player)
    {
        // Handle player spawning
        Debug.Log("Player has spawned!");
    }
}
