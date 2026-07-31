using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DisplayUI : MonoBehaviour
{
    GameManager.CharacterData? character;
    [SerializeField] public PlayerAction controls;
     private GameObject player;
    [SerializeField] public GameObject PauseUi;
    [SerializeField] public GameObject PlayerUi;
    [SerializeField] public MouseState locker;
    [SerializeField] public UIStack uiStack;

    public bool status = false;
    private bool InGame = false;
    private void Awake()
    {
        controls = InputManager.controls;
        if (!character.HasValue)
        {
            InGame = false;
            return;
        }
        player = character.Value.PlayerObject;
    }
    private void OnEnable()
    {
        controls.Global.Pause.performed += OnInteract;
    }

    private void OnDisable()
    {
        controls.Global.Pause.performed -= OnInteract;
    }

    public void OnInteract(InputAction.CallbackContext _context)
    {
        if (status)
        {
            if (uiStack.HasHistory)
            {
                uiStack.GoBack();
            }
            else
            {
                ResumeGame();
            }
        }
        else
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        status = true;
        Time.timeScale = 0f;

        if (player != null)
            player.GetComponent<PlayerView>().canLook = false;

        if (PlayerUi != null)
            PlayerUi.SetActive(false);

        if (locker != null)
            locker.unlockMouse();

        if (uiStack != null && PauseUi != null)
            uiStack.OpenPanelDirectly(PauseUi);
    }

    public void ResumeGame()
    {
        status = false;
        Time.timeScale = 1f;

        if (player != null)
            player.GetComponent<PlayerView>().canLook = true;

        if (PlayerUi != null)
            PlayerUi.SetActive(true);

        if (locker != null)
            locker.lockMouse();

        if (uiStack != null)
            uiStack.CloseAll();
    }

    public void ResumeButton()
    {
        ResumeGame();
    }
    public void ActivateUI(GameManager.CharacterData characterSelection) 
    {
        player = characterSelection.PlayerObject;
        Awake();
    }
}
