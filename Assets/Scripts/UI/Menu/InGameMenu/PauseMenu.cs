using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public bool debug;
    public PlayerAction controls;
    [SerializeField] public GameObject pauseContainerUI;
    [SerializeField] public GameObject otherUICanvas;
    [SerializeField] public Image backgroundUI;
    [SerializeField] public GameObject player;

    public bool OnPause;
     void Awake()
    {
        controls.Global.Enable();
        controls = InputManager.controls;
        pauseContainerUI.SetActive(false);
        OnPause = false;
    }
    void Update()
    {
        if (!OnPause && Time.timeScale != 1)
            TimeResume();
        else if (OnPause && Time.timeScale != 0)
            TimeStop();
    }
    private void TimeStop()
    {
        MonoBehaviour[] tousLesScripts = player.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in tousLesScripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        Time.timeScale = 0;
        player.GetComponent<PlayerView>().canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void TimeResume()
    {
        MonoBehaviour[] tousLesScripts = player.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in tousLesScripts)
        {
            if (script != this)
            {
                script.enabled = true;
            }
        }
        Time.timeScale = 1;
        player.GetComponent<PlayerView>().canLook = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        OnPause = !OnPause;
        backgroundUI.enabled = OnPause;
        pauseContainerUI.SetActive(OnPause);
        otherUICanvas.SetActive(!OnPause);
    }
    public void EscapeFunction()
    {
        pauseContainerUI.SetActive(true);
    }
    public void OnResumeButton()
    {
        OnPause = !OnPause;
        backgroundUI.enabled = OnPause;
        pauseContainerUI.SetActive(OnPause);
        otherUICanvas.SetActive(!OnPause);
    }
    public void OnSettingButton()
    {
        GetComponent<SettingMenu>().ActivateSettingUI();
        pauseContainerUI.SetActive(false);
    }

    public void OnlvButton()
    {

    }
    public void OnQuitButton()
    {

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
