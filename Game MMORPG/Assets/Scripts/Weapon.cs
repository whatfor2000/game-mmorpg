using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    public string weaponName;
    public CharacterClass associatedClass;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            PhotonView view = other.GetComponent<PhotonView>();
            if (playerController != null && view.IsMine)
            {
                // Call the method to change class and manage weapon pickups
                playerController.ChangeClass(this);
                
                // Notify all clients to deactivate this weapon
                photonView.RPC("DeactivateWeapon", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void DeactivateWeapon()
    {
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
