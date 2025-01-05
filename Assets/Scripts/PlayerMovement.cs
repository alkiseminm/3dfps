using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f; // Movement speed
    public float jumpHeight = 3f; // Jump height
    public float gravity = -9.8f; // Gravity force
    public Transform groundCheck; // Ground check position (empty GameObject below the character)
    public LayerMask groundMask; // Layer mask to define what is considered "ground"

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Snappy movement values (acceleration/deceleration)
    public float groundAcceleration = 20f; // How fast you accelerate while moving
    public float groundDeceleration = 50f; // How fast you decelerate when not moving

    private Vector3 currentSpeed; // Current velocity for snappier movement

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get the CharacterController component
    }

    void Update()
    {
        // Check if the player is grounded using Physics.CheckSphere
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);

        // Visualize the ground check in the Scene view for debugging
        Debug.DrawRay(groundCheck.position, Vector3.down * 0.3f, Color.red);

        // If the player is grounded and falling, reset downward velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Prevent floating when grounded
        }

        // Get player movement input
        float x = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float z = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

        // Calculate target movement direction based on player’s orientation
        Vector3 targetDirection = transform.right * x + transform.forward * z;

        // Check if the player is moving
        if (targetDirection.magnitude > 0.1f)
        {
            // Accelerate instantly towards the target direction
            currentSpeed = targetDirection * speed;
        }
        else
        {
            // Apply deceleration instantly when not moving
            currentSpeed = Vector3.zero;
        }

        // Apply movement using CharacterController
        controller.Move(currentSpeed * Time.deltaTime);

        // Jumping logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jumping!"); // Debugging the jump action
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculate jump velocity
        }

        // Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;

        // Apply the velocity (including gravity) to the character's movement
        controller.Move(velocity * Time.deltaTime);
    }
}
