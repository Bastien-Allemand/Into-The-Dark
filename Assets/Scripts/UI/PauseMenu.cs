using UnityEngine;
using UnityEngine.InputSystem;
using static UIManager;

public class PauseMenu : UI
{
    public bool exit = false;
    PlayerAction controls;

    [SerializeField] public Transform GO_Controls_Content;
    [SerializeField] public GameObject GO_Prefab_Keybind;

    public override bool EnterCondition()
    {
        return controls.Menu.Pause.WasPressedThisFrame();
    }
    public override bool ExitCondition()
    {
        //  obliger avec les bouton
        if (controls.Menu.Pause.WasPressedThisFrame())
            exit = true;
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
        exit = false;
        //  ok donc code qui ne devrai pas avoir besoin d'être là mais bug sans
        if (!manager)
            manager = UIManager.Instance;
        Debug.Log($"manager : {manager}");
        manager.Pause(true);
    }
    public override void M_Update()
    {

    }
    public override void M_FixedUpdate()
    {

    }
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
