using UnityEngine;
using Photon.Pun;

public abstract class CharacterClass : MonoBehaviourPunCallbacks
{
    public string className;
    public float moveSpeedModifier = 1f;
    public float attackSpeedModifier = 1f;

    // Call this method in derived classes to initialize class-specific attributes
    protected virtual void InitializeClassAttributes(string name, float moveModifier, float attackModifier)
    {
        className = name;
        moveSpeedModifier = moveModifier;
        attackSpeedModifier = attackModifier;
    }

    // Abstract method for special ability
    public abstract void PerformSpecialAbility();

    // Method to synchronize class changes across the network
    protected void SynchronizeClassChange()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Here you can implement logic to sync class changes, if necessary
            photonView.RPC("RpcUpdateClass", RpcTarget.All, className);
        }
    }

    [PunRPC]
    protected void RpcUpdateClass(string newClassName)
    {
        // Logic for updating class info across all clients
        Debug.Log($"Class updated to: {newClassName}");
    }
}
