using Unity.VisualScripting;
using UnityEngine;

public class CameraBattery : MonoBehaviour
{
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryRemaining;
    [SerializeField] private float timeToReload = 1f;
    [SerializeField] private float currentTimeReload;

    public bool canReload = false;
    private Camera myCam;
    void Start()
    {
        batteryRemaining = maxBattery;
        myCam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        UpdateBattery();
        UpdateReload();
    }

    void UpdateBattery()
    {

        Camera cam = GetComponentInChildren<Camera>();

        if(CamerasScript.instance.camActive == cam)
        {
            batteryRemaining -= Time.deltaTime;
            if(batteryRemaining <= 0)
            {

            }
        }
    }
    //Add verif for left or right hand by use the PickUpScript => isLeftHandEmpty/isRightHandEmpty
    public void Reload()
    {
        batteryRemaining = maxBattery;
        if (myCam != null) myCam.enabled = true;
        Debug.Log($"Cam {gameObject.name} reload");
    }

    void UpdateReload()
    {
        if(canReload)
        {
            currentTimeReload += Time.deltaTime;
            if(currentTimeReload >= timeToReload)
            {
                Reload();
                currentTimeReload = 0;
                canReload = false;
            }
        }
    }
}
