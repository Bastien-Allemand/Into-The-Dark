using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu1 : MonoBehaviour
{
    [SerializeField] public bool debug;
    public PlayerAction controls;
    [SerializeField] public CanvasGroup pauseMenuUI;
    [SerializeField] public UI backgroundUI;

    public bool OnPause;
    void Awake()
    {
        controls = InputManager.controls;
        pauseMenuUI.enabled = false;
        OnPause = false;
    }
    void Update()
    {
        if (!OnPause)
            return;
        else if (!pauseMenuUI.enabled)
            pauseMenuUI.enabled = true;
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        OnPause = !OnPause;
        backgroundUI.enabled = !OnPause;
    }

    public void OnResumeButton()
    {
        OnPause = !OnPause;
        backgroundUI.enabled = !OnPause;
    }
    public void OnSettingButton()
    {

    }
    public void OnlvButton()
    {

    }
    public void OnQuitButton()
    {

    }
    private void OnEnable()
    {
        controls.Menu.Pause.performed += OnInteract;
    }
    private void OnDisable()
    {
        controls.Menu.Pause.performed -= OnInteract;
    }
}
