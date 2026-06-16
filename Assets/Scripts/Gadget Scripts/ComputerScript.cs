using UnityEngine;

public class ComputerScript : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 2f;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CamerasScript camerasScript;
    [SerializeField] private PlayerStateMachine movementScript;
    [SerializeField] private PlayerView lookScript;

    [Header("Computer")]
    [SerializeField] private GameObject screenPC;
    [SerializeField] private Transform computerViewPoint;
    [SerializeField] private Transform playerAnchor;

    [Header("Animation")]
    [SerializeField] private float transitionSpeed = 5f;

    private bool usingComputer = false;

    private Vector3 startCamPosition;
    private Vector3 cameraLocalPos;
    private Quaternion startCamRotation;

    private void Start()
    {
        if (screenPC != null)
            screenPC.SetActive(false);

        cameraLocalPos = playerCamera.transform.localPosition;
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (!usingComputer)
        {
            if (distance <= interactionDistance &&
                Input.GetKeyDown(KeyCode.E))
            {
                EnterComputer();
            }
        }
        else
        {
            MoveCameraToComputer();

            if (Input.GetKeyDown(KeyCode.E))
            {
                ExitComputer();
            }
        }
    }

    private void EnterComputer()
    {
        usingComputer = true;

        startCamPosition = playerCamera.transform.position;
        startCamRotation = playerCamera.transform.rotation;

        movementScript.enabled = false;
        lookScript.enabled = false;

        screenPC.SetActive(true);

        camerasScript.OpenCameraView();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.transform.position = playerAnchor.transform.position;
    }

    private void ExitComputer()
    {
        usingComputer = false;

        movementScript.enabled = true;
        lookScript.enabled = true;

        playerCamera.transform.localPosition = cameraLocalPos;
        playerCamera.transform.rotation = startCamRotation;

        screenPC.SetActive(false);

        camerasScript.CloseCameraView();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MoveCameraToComputer()
    {
        playerCamera.transform.position = Vector3.Lerp(
            playerCamera.transform.position,
            computerViewPoint.position,
            transitionSpeed * Time.deltaTime);

        playerCamera.transform.rotation = Quaternion.Slerp(
            playerCamera.transform.rotation,
            computerViewPoint.rotation,
            transitionSpeed * Time.deltaTime);
    }
}