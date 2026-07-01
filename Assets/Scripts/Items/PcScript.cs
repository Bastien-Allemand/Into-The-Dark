using UnityEngine;
using UnityEngine.InputSystem;

public class PcScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public bool debug;
    [SerializeField] public Transform posCam;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] private CanvasGroup uiElement;

    public PlayerAction controls;
    public bool OnPC = false;
    public bool ExecutableCamera = false;
    void Awake()
    {
        controls = InputManager.controls;
        uiElement.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (!OnPC && Input.GetKeyDown(KeyCode.T))
                ActivatePC();
            else if (Input.GetKeyDown(KeyCode.Y))
                DesactivatePC();
        }
        if (!OnPC)
            return;
        if (!ExecutableCamera)
            return;

    }
    void ActivatePC()
    {
        controls.GamePlay.Disable();
        controls.PcInteract.Enable();
        player.GetComponent<PlayerView>().canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Rigidbody rigidbody = player.GetComponent<Rigidbody>();

        rigidbody.position = posCam.position;
        playerCamera.transform.rotation = posCam.rotation;
        OnPC = true;

    }
    void DesactivatePC()
    {

        controls.PcInteract.Disable();
        controls.GamePlay.Enable();
        player.GetComponent<PlayerView>().canLook = true;
        OnPC = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ActivateExe()
    {
        uiElement.alpha = 1;
    }
    public void DeactivateExe()
    {
        uiElement.alpha = 0;
    }
}
