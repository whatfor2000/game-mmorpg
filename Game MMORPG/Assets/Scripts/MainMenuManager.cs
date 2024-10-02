using Photon.Pun;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput;  // Using TMP InputField for room name input
    public TMP_Text statusText;           // Using TMP Text for status messages

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        statusText.text = "Connecting to Photon...";
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        statusText.text = "Connected to Photon!";
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            PhotonNetwork.CreateRoom(roomNameInput.text);
            statusText.text = "Creating Room: " + roomNameInput.text;
        }
        else
        {
            statusText.text = "Room name cannot be empty!";
        }
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            PhotonNetwork.JoinRoom(roomNameInput.text);
            statusText.text = "Joining Room: " + roomNameInput.text;
        }
        else
        {
            statusText.text = "Room name cannot be empty!";
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
