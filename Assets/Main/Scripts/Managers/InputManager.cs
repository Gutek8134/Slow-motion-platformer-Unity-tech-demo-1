using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

///<summary>Holder class for input-related data</summary>
public static class InputManager
{
    ///<value>Holds an instance of the new input system class, basically a fancy Singleton</value>
    public static MainInputActions playerInput;
    public static Camera currentCamera;
    ///<value>Holds the device player is currently using</value>
    public static Device inputDevice;
    public static Vector2 mousePosition { get; private set; }
    ///<value>Checks whether player is in Ranged Mode... I think</value>
    public static bool isRanged;
    ///<value>Holds a special action firing at every button press that updates currently used device</value>
    private static InputAction checker;

    ///<summary>Sets values for this class</summary>
    public static void Instantiate(Camera camera)
    {
        //Set up input
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
        
        //Set up camera
        currentCamera = camera;

        //Default <c>isRanged<c> to <c>false</c>
        isRanged = false;

        //Startup check for gamepad
        inputDevice = (Gamepad.current != null)? Device.Gamepad : Device.KeyboardAndMouse;

        //Start listening for changing devices
        ListenForDeviceChange();

    } 

    ///<summary>Disables any form of combat input</summary>
    public static void ExitCombatArea()
    {
        if (playerInput.Ranged.enabled)
        {
            CombatManager.CancelRangedAttack();
        }
        CombatManager.ExitCombatArea();
    }
    ///<summary>Enables combat related input</summary>
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

    ///<summary>Behaviour function for setting held mouse position</summary>
    public static void UpdateMousePosition()
    {
        mousePosition = currentCamera.ScreenToWorldPoint(playerInput.Ranged.ShotDirectionM.ReadValue<Vector2>());
    }

    ///<summary>Fancy name for enabling <see cref="checker"/></summary>
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