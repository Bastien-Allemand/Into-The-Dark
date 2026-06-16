using UnityEngine;
using UnityEngine.InputSystem;
using static UIManager;

public class PauseMenu : UI
{
    PlayerAction controls;

    [SerializeField] public Transform GO_Controls_Content;
    [SerializeField] public GameObject GO_Prefab_Keybind;
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
        controls = InputManager.controls;
        foreach (InputAction action in controls.GamePlay.Get())
        {
            if (action.name == "Look")
                continue;
            CreateBoutonFromAction(action,GO_Controls_Content);
        }
    }
    public override void Enter()
    {
        Debug.Log("Pause Menu : Enter");
        exit = false;
        manager.Pause(true);
    }
    //private void Update()
    //{
    //    Cursor.lockState = CursorLockMode.None;
    //}
    public override void Exit()
    {
        manager.Pause(false);
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
