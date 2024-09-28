using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public CharacterClass associatedClass;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ChangeClass(this);
            }
        }
    }
}