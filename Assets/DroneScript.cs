using Assets.Scripts.Items;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    [SerializeField] private Camera droneCamera;
    [SerializeField] private PhoneScript phone;

    public void PlaceDrone()
    {
        phone.OpenDroneCamera(droneCamera);
    }

    public void PickupDrone()
    {
        phone.CloseDroneCamera();
    }
}
