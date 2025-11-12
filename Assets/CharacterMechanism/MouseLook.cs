using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 2f;

    public Transform playerBody;

    private float xRotation = 0f;
    private InputManager inputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inputManager = InputManager.Instance;
        
        if (inputManager == null)
        {
            Debug.LogError("InputManager instance not found! Make sure InputManager is in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager == null) return;

        Vector2 lookInput = inputManager.OnLook();
        
        // Calculate rotation
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;
        
        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Rotate player body left/right
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
