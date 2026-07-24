using UnityEngine;
using UnityEngine.Rendering;

public class DroneScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStateMachine playerMovement;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private DroneMovement droneMovement;
    [SerializeField] private Camera droneCamera;
    [SerializeField] private PhoneView phoneView;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private DroneBattery droneBattery;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip droneSound;

    public void Start()
    {
        droneMovement.enabled = false;

        if (audioSource != null && droneSound != null)
        {
            audioSource.clip = droneSound;
            audioSource.loop = true;
            audioSource.playOnAwake = false;   
        }
    }

    public void PlaceDrone()
    {
        if (!droneBattery.HasBattery)
        { 
            return;
        }

        playerMovement.enabled = false;
        playerView.enabled = false;
        droneMovement.enabled = true;

        droneBattery.StartUsing();

        phoneController.RegisterDrone(this);

        phoneView.OpenDroneCamera(droneCamera);

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PickupDrone()
    {
        StopControl();
    }

    public void StopControl()
    {
        droneBattery.StopUsing();

        droneMovement.enabled = false;
        playerMovement.enabled = true;
        playerView.enabled = true;

        phoneView.CloseDroneCamera();

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}