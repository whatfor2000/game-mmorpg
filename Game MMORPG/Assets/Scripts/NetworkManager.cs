using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput; // UI input field for room name
    public GameObject roomPanel; // Panel to show room options
    public GameObject mainMenuPanel; // Main menu panel
    public GameObject camera;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon
        roomPanel.SetActive(false); // Hide the room panel initially
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master");
        // Show the room creation UI
        mainMenuPanel.SetActive(true);
    }

    public void CreateRoom()
    {
        // Check if connected to the Master Server
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 }; // Set max players
            PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions); // Create room with the name from input field
        }
        else
        {
            Debug.LogError("Not connected to Master Server. Can't create room.");
        }
    }

    public void JoinRoom()
    {
        // Check if connected to the Master Server
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(roomNameInput.text); // Join room with the name from input field
        }
        else
        {
            Debug.LogError("Not connected to Master Server. Can't join room.");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        camera.SetActive(false);
        mainMenuPanel.SetActive(false);
        FindObjectOfType<GameManager>().Start();
        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed: " + message);
    }
}
