using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : UI
{
    PlayerAction controls => InputManager.controls;

                        //  first letter of var, then what it does
    [SerializeField] public GameObject GO_Controls_Content;
    [SerializeField] public GameObject GO_Prefab_Keybind;
    [SerializeField] private Bouton b_continue;
    public override bool EnterCondition()
    {
        if (controls.Menu.Pause.WasPressedThisFrame())
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
        if (controls.Menu.Pause.WasPressedThisFrame())
        {
            exit = true;
            Debug.Log("Pause Menu Input Pressed");
        }
        bool result = exit;
        exit = false;
        return result;
    }
    public override void Init()
    {
        cursorWantedState = CursorLockMode.None;

        Transform target = manager.GetUIs<MainMenu>();
        if (target)
        {
            b_continue.show_target.Add(target);
            Debug.Log(target.transform);
        }
        else
        {
            Debug.Log("No MainMenu");
        }

        foreach (InputAction action in controls.GamePlay.Get())
        {
            if (action.name == "Look")
                continue;
            CreateBoutonFromAction(action,GO_Controls_Content.transform);
        }
    }
    public override void Enter()
    {
        Debug.Log("Pause Menu : Enter");
        exit = false;
        Pause(true);
    }
    public override void Exit()
    {
        Pause(false);
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

    public void Pause(bool pause)
    {
        //  il faut rajouter la pause pour les entité
        if (pause)
        {
            Debug.Log("Time : Pause");
            controls.GamePlay.Disable();
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.Log("Time : Continue");
            controls.GamePlay.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
