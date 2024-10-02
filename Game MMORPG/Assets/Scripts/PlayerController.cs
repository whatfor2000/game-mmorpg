using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks 
{
    public float baseMoveSpeed = 5f;
    public float rotateSpeed = 90f;

    private CharacterController controller;
    private PhotonView view;
    private float verticalVelocity;
    public float gravity = -9.81f;

    private CharacterClass currentClass;
    private Weapon equippedWeapon;

    private Camera camera;
    public TMP_Text playerNameText;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        view = GetComponent<PhotonView>();

        playerNameText.text = PhotonNetwork.NickName;
    }

    private void Update()
    {
        // Only allow movement and actions for the local player
        if (view.IsMine)
        {
            camera.enabled = true;

            // Get input for movement
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Calculate movement vector
            Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

            // Apply gravity
            if (controller.isGrounded)
            {
                verticalVelocity = -1f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
            movement.y = verticalVelocity;

            // Move the player
            float currentMoveSpeed = baseMoveSpeed;
            if (currentClass != null)
            {
                currentMoveSpeed *= currentClass.moveSpeedModifier;
            }
            controller.Move(movement * currentMoveSpeed * Time.deltaTime);

            // Rotate the player with mouse
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);

            // Perform special ability
            if (Input.GetKeyDown(KeyCode.Space) && currentClass != null)
            {
                view.RPC("RpcPerformSpecialAbility", RpcTarget.All);
            }
        }
        else
        {
            camera.enabled = false;
            playerNameText.text = photonView.Owner.NickName;
        }
    }

    public void ChangeClass(Weapon newWeapon)
    {
        if (view.IsMine)
        {
            // Check if there is an equipped weapon
            if (equippedWeapon != null)
            {
                // Drop the currently equipped weapon
                DropWeapon();
            }

            // Equip the new weapon
            equippedWeapon = newWeapon;

            // Change to the new class
            if (currentClass != null)
            {
                Destroy(currentClass);
            }
            currentClass = gameObject.AddComponent(newWeapon.associatedClass.GetType()) as CharacterClass;

            Debug.Log($"Changed to {currentClass.className} class!");

            // Call a RPC to synchronize class change with other players
            view.RPC("RpcChangeClass", RpcTarget.Others, currentClass.className);
        }
    }

    private void DropWeapon()
    {
        // Ensure the equipped weapon is not null
        if (equippedWeapon != null)
        {
            // Calculate the drop position based on the player's current position and forward direction
            Vector3 dropPosition = transform.position + transform.forward * 2f; // Adjust distance as needed

            // Call RPC to drop the weapon on all clients
            photonView.RPC("OnWeaponDropped", RpcTarget.All, equippedWeapon.photonView.ViewID, dropPosition);

            // Optionally deactivate or destroy the weapon locally
            equippedWeapon.SetActive(false);
            equippedWeapon = null; // Clear equipped weapon reference
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Check if the disconnected player is controlled by another player
        if (otherPlayer.ActorNumber == view.Owner.ActorNumber)
        {
            // Optionally, you can destroy the local player object if necessary
            PhotonNetwork.Destroy(gameObject);
        }

        // Optionally, update the UI or perform other logic here
        Debug.Log($"{otherPlayer.NickName} has left the room.");
    }

    void OnDestroy()
    {
        // Clean up resources if needed when the object is destroyed
        if (camera != null)
        {
            camera.enabled = false;
        }
    }

    [PunRPC]
    private void RpcChangeClass(string newClassName)
    {
        // Change class for non-local players
        Debug.Log($"Player changed to {newClassName} class!");

        // Optionally, you can instantiate the new class component here as well if needed
        // Example: AddComponent<CharacterClass>() depending on how you want to handle classes
    }

    [PunRPC]
    private void RpcPerformSpecialAbility()
    {
        if (currentClass != null)
        {
            currentClass.PerformSpecialAbility();
        }
    }
    
    [PunRPC]
    public void OnWeaponDropped(int weaponViewID, Vector3 dropPosition)
    {
        // Find the weapon by its PhotonView ID
        PhotonView weaponView = PhotonView.Find(weaponViewID);
        if (weaponView != null)
        {
            // Get the weapon component
            Weapon droppedWeapon = weaponView.GetComponent<Weapon>();
            
            // Activate the weapon again in the world
            droppedWeapon.SetActive(true); 
            
            // Set the position to where it drops
            droppedWeapon.transform.position = dropPosition; // Use the received drop position
            droppedWeapon.transform.rotation = Quaternion.identity; // Optional: reset rotation
            Debug.Log($"Weapon {droppedWeapon.weaponName} dropped at position: {dropPosition}");
        }
    }
}
