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
            if (playerController != null && playerController.photonView.IsMine)
            {
                // Call RPC to change class on all clients
                playerController.ChangeClass(this);
            }
        }
    }

    // Optionally, you can add a method to activate/deactivate the weapon
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
