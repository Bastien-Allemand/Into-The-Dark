using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class LastDeviceDetector : MonoBehaviour
{
    private DisplayInventory inventory;
    public static InputDevice LastDevice;
    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()    // ne devrai jamais être disable
    {
        InputSystem.onEvent -= OnInputEvent;
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    private void Awake()
    {
        DeviceChangeUpdate(Keyboard.current);
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;

        DeviceChangeUpdate(device);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (inventory == null)
        {
            inventory = FindFirstObjectByType<DisplayInventory>();
        }
    }

    private void DeviceChangeUpdate(InputDevice device)
    {
        switch (LastDevice)
        {
            case Keyboard:
            case Mouse:
                if (device is Mouse || device is Keyboard)
                    return;
                break;
            case Gamepad:
                if (device is Gamepad)
                    return;
                break;
            case null: break;
            default: return;
        }

        if (inventory)
            inventory.DeviceChangeUpdate(device);

        LastDevice = device;

    }
}
