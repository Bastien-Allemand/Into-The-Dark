using Unity.VisualScripting;
using UnityEngine;

public class CameraBattery : MonoBehaviour
{
    PlayerAction controls => InputManager.controls;

    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryRemaining;
    private Camera myCam;
    void Start()
    {
        batteryRemaining = maxBattery;
        myCam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        UpdateBattery();
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

    public void Reload()
    {
        batteryRemaining = maxBattery;
        if (myCam != null) myCam.enabled = true;
        Debug.Log($"Caméra {gameObject.name} rechargée !");
    }
}
