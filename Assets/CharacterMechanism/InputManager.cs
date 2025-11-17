using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem_Actions;

    #region Singleton
    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        inputSystem_Actions = new InputSystem_Actions();
    }
    #endregion

    public void OnEnable()
    {
        inputSystem_Actions.Enable();
    }

    public void OnDisable()
    {
        inputSystem_Actions.Disable();
    }

    public void OnInteractButton()
    {
        Debug.Log("Interact Button Pressed");
    }

    public Vector2 OnMove()
    {
        return inputSystem_Actions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 OnLook()
    {
        return inputSystem_Actions.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return inputSystem_Actions.Player.Jump.triggered;
    }

    public bool IsSprinting()
    {
        return inputSystem_Actions.Player.Sprint.IsPressed();
    }

    public bool IsCrouching()
    {
        return inputSystem_Actions.Player.Crouch.IsPressed();
    }

    public bool Flashlight()
    {
        return inputSystem_Actions.Player.Torch.IsPressed();
    }

    public bool Sonar()
    {
        return inputSystem_Actions.Player.Sonar.triggered;
    }

    public bool Exit()
    {
        return inputSystem_Actions.Player.Exit.triggered;
    }
}
