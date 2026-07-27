using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayUI : MonoBehaviour
{
    [SerializeField] public PlayerAction controls;
    [SerializeField] public GameObject PauseUi;
    [SerializeField] public GameObject PlayerUi;
    [SerializeField] public MouseState locker;
    public bool status = false;
    private void Awake()
    {
        controls = InputManager.controls;
    }

    void OnInteract(InputAction.CallbackContext _context)
    {
        if (status)
        {
            PlayerUi.SetActive(true);
            PauseUi.SetActive(false);
            locker.lockMouse();
            Time.timeScale = 1f;
            status = false;
        }
        else
        {
            PlayerUi.SetActive(false);
            PauseUi.SetActive(true);
            locker.unlockMouse();
            Time.timeScale = 0f;
            status = true;
        }
    }
    private void OnEnable()
    {
        controls.Global.Pause.performed += OnInteract;
    }
    private void OnDisable()
    {
        controls.Global.Pause.performed -= OnInteract;
    }
}
