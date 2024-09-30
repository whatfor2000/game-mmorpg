using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float baseMoveSpeed = 5f;
    public float rotateSpeed = 90f;

    private CharacterController controller;
    private PhotonView view;
    private float verticalVelocity;
    public float gravity = -9.81f;

    private CharacterClass currentClass;
    private Weapon currentWeapon;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        // Only allow movement and actions for the local player
        if (!view.IsMine) return;

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

    public void ChangeClass(Weapon newWeapon)
    {
        if (view.IsMine)
        {
            if (currentWeapon != null)
            {
                // Drop the current weapon
                DropWeapon();
            }

            // Pick up the new weapon
            currentWeapon = newWeapon;
            currentWeapon.gameObject.SetActive(false);

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
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.transform.position = transform.position + transform.forward;
            currentWeapon = null;
        }
    }

    [PunRPC]
    private void RpcChangeClass(string newClassName)
    {
        // Change class for non-local players
        // You might want to handle loading the correct class here
        Debug.Log($"Player changed to {newClassName} class!");
    }

    [PunRPC]
    private void RpcPerformSpecialAbility()
    {
        if (currentClass != null)
        {
            currentClass.PerformSpecialAbility();
        }
    }
}
