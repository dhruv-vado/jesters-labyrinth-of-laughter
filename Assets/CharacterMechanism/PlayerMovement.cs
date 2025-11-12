using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeedWalk = 5.0f;
    [SerializeField]
    private float playerSpeedSprint = 8.0f;
    [SerializeField]
    private float playerSpeedCrouch = 3.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    public GameObject camera;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool crouch = false;
    private InputManager inputManager;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = inputManager.OnMove();
        
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move = Vector3.ClampMagnitude(move, 1f);

        // Sprint Mechanism
        float currentSpeed = inputManager.IsCrouching() ? playerSpeedCrouch : (inputManager.IsSprinting() ? playerSpeedSprint : playerSpeedWalk);
        
        playerVelocity.y += gravityValue * Time.deltaTime;

        //Crouch Mechanism
        if(inputManager.IsCrouching())
        {
            if (!crouch)
			{
                camera.transform.Translate( 0f, -0.3f, 0f);
                controller.height = 1.35f;
                crouch = true;
            }
        }
        else
        {
            if (crouch)
			{
            camera.transform.Translate ( 0f, 0.3f, 0f);
            controller.height = 2f;
			crouch = false;
			}
        }
        Vector3 finalMove = (move * currentSpeed * Time.deltaTime) + (playerVelocity.y * Vector3.up * Time.deltaTime);
        controller.Move(finalMove);
    }
}
