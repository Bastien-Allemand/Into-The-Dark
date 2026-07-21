using System;
using UnityEngine;

public enum PhoneState
{
    Hidden,
    Idle,
    Camera
}

public class PhoneController : MonoBehaviour
{
    public event Action<PhoneState> OnPhoneStateChanged;
    [Header("References")]
    [SerializeField] private UIBattery uiBattery;

    private DroneScript currentDrone;

    private PhoneState currentState = PhoneState.Hidden;

    public bool IsLookingCamera => currentState == PhoneState.Camera;

    public PhoneState GetCurrentPhoneState() => currentState;

    public void RegisterDrone(DroneScript drone)
    {
        currentDrone = drone;
    }
    public bool CanWatchCamera()
    {
        return uiBattery != null && uiBattery.HaveBattery && currentState == PhoneState.Idle;
    }

    public void TogglePhone()
    {
        if (!uiBattery.HaveBattery)
            return;

        if (currentState == PhoneState.Hidden)
        {
            ChangeState(PhoneState.Idle);
        }
        else
        {
            ChangeState(PhoneState.Hidden);
        }
    }

    public void ToggleCameraMode()
    {
        if (!uiBattery.HaveBattery)
            return;

        if (currentState == PhoneState.Hidden)
            return;

        if (currentState == PhoneState.Camera)
            ChangeState(PhoneState.Idle);
       
        else if (currentState == PhoneState.Idle && uiBattery != null && uiBattery.HaveBattery)
        ChangeState(PhoneState.Camera);
    }

    private void ChangeState(PhoneState newState)
    {
        if (currentState == newState)
            return;

        if (newState == PhoneState.Hidden && currentDrone != null)
        {
            currentDrone.StopControl();
        }

        currentState = newState;
        OnPhoneStateChanged?.Invoke(currentState);
    }

    private void Update()
    {
        if (currentState != PhoneState.Hidden && uiBattery != null)
        {
            uiBattery.HandleBatteryDrain();
        }
    }
    public void OpenDroneCamera()
    {
        if (currentState == PhoneState.Hidden)
        {
            ChangeState(PhoneState.Idle);
        }

        ChangeState(PhoneState.Camera);
    }

    public void CloseDroneCamera()
    {
        ChangeState(PhoneState.Idle);
    }

    public void BatteryEmpty()
    {
        if (currentDrone != null)
        {
            currentDrone.StopControl();
        }

        ChangeState(PhoneState.Hidden);
    }

}
