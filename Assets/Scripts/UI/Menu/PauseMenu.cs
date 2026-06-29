using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : UI
{
    PlayerAction controls_buffer;
    //  first letter of var, then what it does
    [SerializeField] public GameObject GO_Controls_Content;
    [SerializeField] public GameObject GO_Prefab_Keybind;
    [SerializeField] private Bouton b_continue;
    [SerializeField] private Bouton b_main_menu;

    public override bool EnterCondition()
    {
        Debug.Log(JsonManager.controls);
        if (JsonManager.controls.Menu.Pause.WasPressedThisFrame())
        {
            enter = true;
            Debug.Log("Pause Menu Input Pressed");
        }
        bool result = enter;
        enter = false;
        return result;
    }
    public override bool ExitCondition()
    {
        if (JsonManager.controls.Menu.Pause.WasPressedThisFrame())
        {
            exit = true;
            Debug.Log("Pause Menu Input Pressed");
        }
        bool result = exit;
        exit = false;
        return result;
    }
    private void Awake()
    {

        cursorWantedState = CursorLockMode.None;

        Transform target = manager.GetUIs<MainMenu>();
        if (target)
        {
            b_continue.show_target.Add(target);
            Debug.Log(target.transform);
            b_main_menu.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No MainMenu");
            b_main_menu.gameObject.SetActive(true);
        }

        foreach (InputAction action in controls_buffer.GamePlay.Get())
        {
            if (action.name == "Look")
                continue;
            CreateBoutonFromAction(action, GO_Controls_Content.transform);
        }
    }
    private void OnEnable()
    {
        Debug.Log("Pause Menu : Enter");

        controls_buffer = new PlayerAction();

        exit = false;
        //  can be opti, but a bit weird to t
        foreach (var ui in UIManager.Instance.UIs)
        {
            if (ui.GetComponent<MainMenu>() != null)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        foreach (var ui in UIManager.Instance.UIs)
        {
            if (ui.GetComponent<MainMenu>() != null)
            {
                ui.gameObject.SetActive(true);
            }
        }
    }

    private void CreateBoutonFromAction(InputAction action, Transform parent)
    {
        int bindingIndex = 0;
        foreach (InputBinding binding in action.bindings)
        {
            if (binding.isComposite)
            {
                bindingIndex++;
                continue;
            }
            Debug.Log($"name={action.name + " " + binding.name} | path={binding.path} | composite={binding.isComposite} | part={binding.isPartOfComposite}");

            GameObject buffer = Instantiate(GO_Prefab_Keybind, parent, false);
            var tmp = buffer.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = $"{action.name} {binding.name}";

            Rebind script = buffer.GetComponentInChildren<Rebind>();
            if (script != null)
                script.Init(action, bindingIndex);
            else
                Debug.Log("script Rebind not found");

            bindingIndex++;
        }
    }
}
