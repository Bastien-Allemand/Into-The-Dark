using System;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public static event Action<Camera> OnCameraDeployed;
    public static event Action<Camera> OnCameraRemoved;

    [Header("Status")]
    public bool deployed = false;
    public bool canBeRepickUp = false;
    public bool needsToBePlaced = false;

    [Header("Timer Settings")]
    public bool usingTimer = false;
    public float placingDelay = 0f;
    public float timer = 0f;

    [Header("Energy Settings")]
    public bool usingEnergy = false;
    public float energy = 200f;
    public float maxEnergy = 200f;
    public bool rechargable = false;

    [Header("References")]
    [SerializeField] private BatteryScript batteryScript;

    private Camera _camera;

    public enum ItemType
    {
        NONE,
        ITEM_CAMERA
    }

    [SerializeField] public ItemType type = ItemType.NONE;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        batteryScript = GetComponentInChildren<BatteryScript>();

        if (batteryScript != null)
        {
            batteryScript.SetMaxBattery(maxEnergy);
        }

        if (deployed && type == ItemType.ITEM_CAMERA && _camera != null && gameObject.layer == 17)
        {
            OnCameraDeployed?.Invoke(_camera);
        }
    }

    public void Update()
    {
        if (deployed && usingTimer)
        {
            if (timer < 0f)
            {
                Destroy(gameObject);
                return;
            }
            timer -= Time.deltaTime;
            placingDelay -= Time.deltaTime;
        }

        if (usingEnergy)
        {
            energy -= Time.deltaTime;

            if (energy <= 0f)
            {
                energy = 0f;
            }

            if (batteryScript != null)
            {
                batteryScript.SetBattery(energy);
            }
        }
    }

    public void DeployItem()
    {
        deployed = true;

        if (type == ItemType.ITEM_CAMERA && _camera != null && gameObject.layer == 17)
        {
            OnCameraDeployed?.Invoke(_camera);
        }
    }
    public void PickUpItem()
    {
        deployed = false;
        
        if (type == ItemType.ITEM_CAMERA && _camera != null && gameObject.layer == 17)
        {
            OnCameraRemoved?.Invoke(_camera);
        }
    }

    private void OnDestroy()
    {
        if (type == ItemType.ITEM_CAMERA && _camera != null)
        {
            OnCameraRemoved?.Invoke(_camera);
        }
    }
}