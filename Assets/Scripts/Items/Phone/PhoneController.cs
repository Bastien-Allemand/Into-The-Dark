using System;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public event Action<PhoneState> OnPhoneStateChanged;

    [Header("References")]
    [SerializeField] private UIBattery uiBattery;

    private PhoneState currentState = PhoneState.Hidden;

    public bool IsLookingCamera => currentState == PhoneState.Camera;

    public PhoneState GetCurrentPhoneState() => currentState;

    public bool CanWatchCamera()
    {
        return uiBattery != null && uiBattery.HaveBattery && currentState == PhoneState.Idle;
    }

    public void TogglePhone()
    {
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
        if (currentState == PhoneState.Hidden) return;

        if (currentState == PhoneState.Camera)
        {
            ChangeState(PhoneState.Idle);
        }
        else
        {
            ChangeState(PhoneState.Camera);
        }
    }

    private void ChangeState(PhoneState newState)
    {
        if (currentState == newState) return;

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
}
