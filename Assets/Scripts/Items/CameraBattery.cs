using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraBattery : MonoBehaviour
{
    [SerializeField] public float maxBattery = 100f;
    [SerializeField] public float batteryRemaining;
    [SerializeField] private float timeToReload = 1f;
    [SerializeField] private float currentTimeReload;
    [SerializeField] private GameObject blackScreen;

    public bool canReload = false;
    private Camera myCam;
    void Start()
    {
        blackScreen.SetActive(false);
        batteryRemaining = maxBattery;
        myCam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        UpdateReload();
        
        UpdateBattery();
    }

    void UpdateBattery()
    {

        Camera cam = GetComponentInChildren<Camera>();

        if(CamerasScript.instance.camActive == cam && CamerasScript.instance.camActive != null && cam.enabled)
        {
            batteryRemaining -= Time.deltaTime;
            if(batteryRemaining <= 0)
            {
                batteryRemaining = 0;
                blackScreen.SetActive(true);
            }
        }
        else
        {
            blackScreen.SetActive(false);
        }
    }
    //Add verif for left or right hand by use the PickUpScript => isLeftHandEmpty/isRightHandEmpty
    public void Reload()
    {
        batteryRemaining = maxBattery;
        if (myCam != null) myCam.enabled = true;
        //Debug.Log($"Cam {gameObject.name} reload");
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
                blackScreen.SetActive(false);
            }
        }
    }
}
