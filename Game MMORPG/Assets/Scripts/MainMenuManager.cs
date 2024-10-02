using Photon.Pun;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput;  // Using TMP InputField for room name input
    public TMP_InputField playerNameInput;
    public TMP_Text statusText;           // Using TMP Text for status messages

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        statusText.text = "Connecting to Photon...";

        // Load the player's name from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string storedName = PlayerPrefs.GetString("PlayerName");
            playerNameInput.text = storedName;  // Set the input field to the stored name
            PhotonNetwork.NickName = storedName;  // Set the player's nickname in Photon
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        statusText.text = "Connected to Photon!";
    }

    public void CreateRoom()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(roomNameInput.text) && !string.IsNullOrEmpty(playerNameInput.text))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            PhotonNetwork.NickName = playerName;
            PhotonNetwork.CreateRoom(roomNameInput.text);
            statusText.text = "Creating Room: " + roomNameInput.text;
        }else if(!string.IsNullOrEmpty(playerNameInput.text)){
            statusText.text = "Player name cannot be empty!"; 
        }
    }

    public void JoinRoom()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(roomNameInput.text) && !string.IsNullOrEmpty(playerNameInput.text))
        {
            PhotonNetwork.JoinRoom(roomNameInput.text);
            PlayerPrefs.SetString("PlayerName", playerName);
            PhotonNetwork.NickName = playerName;
            statusText.text = "Joining Room: " + roomNameInput.text;
        }
        else
        {
            if(!string.IsNullOrEmpty(roomNameInput.text)){
                statusText.text = "Room name cannot be empty!"; 
            }else if(!string.IsNullOrEmpty(playerNameInput.text)){
                statusText.text = "Player name cannot be empty!"; 
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("ReadyRoom");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "Failed to join room: " + message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "Failed to create room: " + message;
    }
}
