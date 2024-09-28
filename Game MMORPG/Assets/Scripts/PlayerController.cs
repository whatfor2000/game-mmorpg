using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseMoveSpeed = 5f;
    public float rotateSpeed = 90f;

    private CharacterController controller;
    private float verticalVelocity;
    public float gravity = -9.81f;

    private CharacterClass currentClass;
    private Weapon currentWeapon;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
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
            currentClass.PerformSpecialAbility();
        }
    }

    public void ChangeClass(Weapon newWeapon)
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
}