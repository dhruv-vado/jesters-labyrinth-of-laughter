using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeedWalk = 5.0f;
    [SerializeField] private float playerSpeedSprint = 8.0f;
    [SerializeField] private float playerSpeedCrouch = 3.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float _doorDetectionRange = 1f;
    [SerializeField] private LayerMask _doorDetectionMask;
    [SerializeField] public GameObject _footstepsAudioSource;

    public GameObject camera;

    private bool _isPlayable = true;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool crouch = false;
    private InputManager inputManager;
    private GameManager gameManager;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        _isPlayable = GameManager.Instance.IsPlayable;
        if(!_isPlayable) return;

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

        if(input != new Vector2(0f,0f))
        {
            _footstepsAudioSource.SetActive(true);
        }
        else
        {
            _footstepsAudioSource.SetActive(false);
        }

        if(inputManager.Exit())
        {
            if(RayCheck())
            {
                gameManager.PlayerEscaped();
                _footstepsAudioSource.SetActive(false);
            }
        }
    }

    private bool RayCheck()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, _doorDetectionRange, _doorDetectionMask, QueryTriggerInteraction.Ignore) && hitInfo.collider.CompareTag("Exit"))
        {
            return true;
        }

        return false;
    }

    public void PlayerCaught()
    {
        _isPlayable = false;
        _footstepsAudioSource.SetActive(false);
    }
}
