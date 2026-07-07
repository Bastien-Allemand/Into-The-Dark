using UnityEngine;

public class DroneScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStateMachine playerMovement;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private DroneMovement droneMovement;
    [SerializeField] private Camera droneCamera;
    [SerializeField] private PhoneView phoneView;
    [SerializeField] private PhoneController phoneController;

    public void Start()
    {
        droneMovement.enabled = false;
    }

    public void PlaceDrone()
    {
        playerMovement.enabled = false;
        playerView.enabled = false;
        droneMovement.enabled = true;

        phoneController.RegisterDrone(this);
        phoneView.OpenDroneCamera(droneCamera);
    }

    public void PickupDrone()
    {
        StopControl();
    }

    public void StopControl()
    {
        droneMovement.enabled = false;
        playerMovement.enabled = true;
        playerView.enabled = true;

        phoneView.CloseDroneCamera();
    }
}