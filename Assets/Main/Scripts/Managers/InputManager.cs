using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
public static class InputManager
{
    public static MainInputActions playerInput;
    public static Camera currentCamera;
    public static Device inputDevice;
    public static Vector2 mousePosition { get; private set; }
    public static bool isRanged;
    private static InputAction checker;

    public static void Instantiate(Camera camera)
    {
        playerInput = new MainInputActions();
        playerInput.AlwaysActive.Enable();
        checker = new InputAction(binding: "/*/<button>");
        checker.started += context =>
        {
            InputDevice device = context.control.device;
            if (device is Mouse||device is Keyboard)
            {
                inputDevice = Device.KeyboardAndMouse;
            }
            else if (device is Gamepad)
            {
                inputDevice = Device.Gamepad;
            }
            // Debug.Log(inputDevice);
        };
        
        currentCamera = camera;

        isRanged = false;

        inputDevice = (Gamepad.current != null)? Device.Gamepad : Device.KeyboardAndMouse;

        ListenForDeviceChange();
    }

    public static void ExitCombatArea()
    {
        if (playerInput.Ranged.enabled)
        {
            CombatManager.CancelRangedAttack();
        }
        CombatManager.ExitCombatArea();
    }
    public static void EnterCombatArea()
    {
        CombatManager.EnterCombatArea();
    }

    public static void ChangeCurrentCamera(Camera camera)
    {
        currentCamera = camera;
    }

    public static void ChangeInputDevice(Device device)
    {
        inputDevice = device;
    }

    public static void UpdateMousePosition()
    {
        mousePosition = currentCamera.ScreenToWorldPoint(playerInput.Ranged.ShotDirectionM.ReadValue<Vector2>());
    }

    public static void ListenForDeviceChange()
    {
        checker.Enable();
    }
}

public enum Device
{
    Gamepad,
    KeyboardAndMouse
}