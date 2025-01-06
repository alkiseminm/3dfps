using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f; // Movement speed
    public float jumpHeight = 3f; // Jump height
    public float gravity = -9.8f; // Gravity force
    public float bodySlamSpeed = -50f; // Speed of the body slam
    public Transform groundCheck; // Ground check position (empty GameObject below the character)
    public LayerMask groundMask; // Layer mask to define what is considered "ground"

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isBodySlamming = false; // Tracks whether the player is body-slamming

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get the CharacterController component
    }

    void Update()
    {
        // Check if the player is grounded using Physics.CheckSphere
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);

        // If grounded and falling, reset downward velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Prevent floating when grounded
            isBodySlamming = false; // Reset body slam state when grounded
        }

        // Horizontal movement
        if (!isBodySlamming) // Prevent horizontal movement during body slam
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isBodySlamming)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Body Slam
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isGrounded) // Initiate body slam in mid-air
        {
            isBodySlamming = true;
            velocity = Vector3.zero; // Stop all horizontal movement immediately
            velocity.y = bodySlamSpeed; // Apply fast downward speed
        }

        // Apply gravity (and body slam velocity, if active)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
