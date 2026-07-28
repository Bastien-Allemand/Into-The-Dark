using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DisplayUI : MonoBehaviour
{
    [SerializeField] public PlayerAction controls;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject PauseUi;
    [SerializeField] public GameObject PlayerUi;
    [SerializeField] public MouseState locker;
    public bool status = false;
    private void Awake()
    {
        controls = InputManager.controls;
    }

    public void OnInteract(InputAction.CallbackContext _context)
    {
        if (status)
        {
            if (!PauseUi.activeSelf)
                return;
                player.GetComponent<PlayerView>().canLook = true;
            PlayerUi.SetActive(true);
            PauseUi.SetActive(false);
            locker.lockMouse();
            Time.timeScale = 1f;
            status = false;
        }
        else
        {
            player.GetComponent<PlayerView>().canLook = false;
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

    public void ResumeButton()
    {
        player.GetComponent<PlayerView>().canLook = true;
        PlayerUi.SetActive(true);
        PauseUi.SetActive(false);
        locker.lockMouse();
        Time.timeScale = 1f;
        status = false;
    }

}
