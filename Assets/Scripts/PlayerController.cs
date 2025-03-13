using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private CharacterController characterController;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        //Check if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Gets the input from the player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Move the player
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move *speed * Time.deltaTime);

        //Do the jump stuff
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //Apply the gravity to our velocity
        velocity.y += gravity;
        characterController.Move(velocity * Time.deltaTime);
    }
}
