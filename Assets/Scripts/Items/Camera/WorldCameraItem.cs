using UnityEngine;

public class WorldCameraItem : MonoBehaviour
{
    public bool isPickedUp = false;
    private Camera cam;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();

        cam.enabled = false;
    }

    public void PickUp()
    {
        isPickedUp = true;

        CamerasScript.instance.RegisterCamera(cam);
    }

    public void Drop()
    {
        isPickedUp = false;

        CamerasScript.instance.UnregisterCamera(cam);
        cam.enabled = false;
    }
}