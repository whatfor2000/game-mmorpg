using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyRoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text playerListText;  // TMP Text to show the list of players
    public TMP_Text readyStatusText; // TMP Text to show ready status
    public TMP_Text waitingText;     // TMP Text to display waiting for players
    public Button readyButton;
    public Button startButton;

    private void Start()
    {
        UpdatePlayerList();
        waitingText.text = "Waiting for players...";

        PhotonNetwork.AutomaticallySyncScene = true;
        
        // Check if the player is the host (the one who created the room)
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);  // Enable Start Button for the host
        }
        else
        {
            startButton.gameObject.SetActive(false); // Hide Start Button for other players
        }
    }

    public void UpdatePlayerList()
    {
        playerListText.text = "Players in Room:\n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";  // Display player names using TMP_Text
        }
    }

    public void OnReadyButtonPressed()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", true } });
        readyStatusText.text = "You are ready!";
        readyButton.interactable = false;
    }

    public void OnStartButtonPressed()
    {
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel("Lobby");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void CheckAllPlayersReady()
    {
        bool allReady = true;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                allReady = false;
                break;
            }
        }
        if(allReady){
            waitingText.text = "All players are ready!";
            startButton.interactable = true;
        }else{
            startButton.interactable = false;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            CheckAllPlayersReady();
        }
    }
    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // If the current player is now the host, enable the Start Button
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
        }
    }

}
