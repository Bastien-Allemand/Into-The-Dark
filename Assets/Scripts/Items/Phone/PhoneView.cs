using Assets.Scripts.Items;
using System;
using UnityEngine;

public class PhoneView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private Transform phoneTransform;
    [SerializeField] private MeshRenderer phoneMeshRenderer;
    [SerializeField] private GameObject screenPhone;
    [SerializeField] private GameObject batteryModel;
    [SerializeField] private Light phoneLight;

    [Header("Anchors")]
    [SerializeField] private Transform hiddenAnchor;
    [SerializeField] private Transform idleAnchor;
    [SerializeField] private Transform cameraAnchor;

    [Header("Animation Settings")]
    [SerializeField] private float transitionSpeed = 8f;
    [SerializeField] private float hideDistanceThreshold = 0.01f;

    private Transform targetAnchor;
    private bool waitingForHide;

    private void Awake()
    {
        targetAnchor = hiddenAnchor;
    }

    private void OnEnable()
    {
        if (phoneController != null)
            phoneController.OnPhoneStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (phoneController != null)
            phoneController.OnPhoneStateChanged -= HandleStateChanged;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void HandleStateChanged(PhoneState newState)
    {
        targetAnchor = newState switch
        {
            PhoneState.Hidden => hiddenAnchor,
            PhoneState.Camera => cameraAnchor,
            _ => idleAnchor
        };
        switch (newState)
        {
            case PhoneState.Hidden:
                phoneMeshRenderer.enabled = false;
                batteryModel.SetActive(false);
                phoneLight.enabled = false;
                screenPhone.SetActive(false);

                if (CamerasScript.instance != null)
                    CamerasScript.instance.SetCameraViewActive(false);

                waitingForHide = true;
                break;

            case PhoneState.Idle:
                phoneMeshRenderer.enabled = true;
                batteryModel.SetActive(true);
                screenPhone.SetActive(false);
                phoneLight.enabled = false;

                if (CamerasScript.instance != null)
                    CamerasScript.instance.SetCameraViewActive(false);

                waitingForHide = false;
                break;

            case PhoneState.Camera:
                phoneMeshRenderer.enabled = true;
                batteryModel.SetActive(true);
                screenPhone.SetActive(true);
               
                if (CamerasScript.instance != null)
                    CamerasScript.instance.SetCameraViewActive(true);

                waitingForHide = false;
                break;
        }
    }

    private void UpdatePosition()
    {
        if (targetAnchor == null) return;

        phoneTransform.localPosition = Vector3.Lerp(phoneTransform.localPosition, targetAnchor.localPosition, transitionSpeed * Time.deltaTime);
        phoneTransform.localRotation = Quaternion.Slerp(phoneTransform.localRotation, targetAnchor.localRotation, transitionSpeed * Time.deltaTime);
        phoneTransform.localScale = Vector3.Lerp(phoneTransform.localScale, targetAnchor.localScale, transitionSpeed * Time.deltaTime);

        if (waitingForHide && Vector3.Distance(phoneTransform.localPosition, targetAnchor.localPosition) < hideDistanceThreshold)
        {
            phoneTransform.localPosition = targetAnchor.localPosition;
            waitingForHide = false;
        }
    }
    public void OpenDroneCamera(Camera droneCamera)
    {
        if (CamerasScript.instance != null)
            CamerasScript.instance.SetExternalCamera(droneCamera);

        phoneController.OpenDroneCamera();
    }

    public void CloseDroneCamera()
    {
        if (CamerasScript.instance != null)
            CamerasScript.instance.DisableExternalCamera();

        phoneController.CloseDroneCamera();
    }
}
