using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Metadata;

public class PauseMenu : UI
{
    PlayerAction controls => InputManager.controls;

    //  first letter of var, then what it does
    [SerializeField] public GameObject Controls_ViewPort;
    [SerializeField] public GameObject Content_Prefab;
    [SerializeField] public GameObject GO_Prefab_Keybind;
    [SerializeField] private Bouton b_continue;
    [SerializeField] private Bouton b_main_menu;

    private List<GameObject> gameObjects_Contents = new List<GameObject>();
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
    private void Start()
    {
        foreach (Transform child in Controls_ViewPort.transform)
        {
            gameObjects_Contents.Add(child.gameObject);
        }
        int i = Mathf.Min(gameObjects_Contents.Count, controls.controlSchemes.Count);
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

        Action<IEnumerable<InputDevice>, GameObject> InitBouton = (usedDevices, content) =>
        {
            if (!usedDevices.All(device => device == null))
                foreach (InputActionMap map in controls.asset.actionMaps)
                {
                    foreach (InputAction action in map)
                    {
                        if (map.name == controls.GeneriqueMove.Get().name && action.name == "Look")
                            continue;

                        CreateBoutonFromAction(action, content.transform, usedDevices);
                    }
                }
        };

        for (int i = 0; i < controls.controlSchemes.Count; i++)
        {
            InputControlScheme scheme = controls.controlSchemes[i];

            List<InputDevice> usedDevices = new();

            foreach (var requirement in scheme.deviceRequirements)
            {
                var device = InputSystem.devices.FirstOrDefault(d =>
                    InputControlPath.Matches(requirement.controlPath, d));

                if (device != null)
                    usedDevices.Add(device);
            }

            if (i < gameObjects_Contents.Count)
                gameObjects_Contents[i].name = scheme.name;
            else
            {
                GameObject instance = Instantiate(Content_Prefab);
                instance.transform.SetParent(Controls_ViewPort.transform, false);
                instance.name = scheme.name;
                instance.SetActive(false);
                gameObjects_Contents.Add(instance);
            }

            InitBouton(usedDevices, gameObjects_Contents[i]);
        }
        if (controls.controlSchemes.Count < gameObjects_Contents.Count) // resize si trop grand (sécurité)
            gameObjects_Contents.RemoveRange(controls.controlSchemes.Count, gameObjects_Contents.Count - controls.controlSchemes.Count);

        gameObjects_Contents[0].SetActive(true);
    }
    private void OnEnable()
    {
        Debug.Log("Pause Menu : Enter");
        exit = false;
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

    private void CreateBoutonFromAction(InputAction action, Transform parent, IEnumerable<InputDevice> deviceUsed)
    {
        int bindingIndex = 0;
        foreach (InputBinding binding in action.bindings)
        {
            //   regarde si le chemain prie n'est pas l'un définit dedans deviceUsed (KeyBoard,Mouse,GamePad,etc..)
            if (binding.isComposite || !deviceUsed.Any(device => InputControlPath.Matches(binding.path, device)))
            {
                bindingIndex++;
                continue;
            }
            string displayName = binding.name switch
            {
                "Negative" => "Left",
                "Positive" => "Right",
                _ => binding.name
            };
            Debug.Log($"name={action.name + " " + displayName} | path={binding.path} | composite={binding.isComposite} | part={binding.isPartOfComposite}");

            GameObject buffer = Instantiate(GO_Prefab_Keybind, parent, false);
            var tmp = buffer.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = $"{action.name} {displayName}";

            Rebind script = buffer.GetComponentInChildren<Rebind>();
            if (script != null)
                script.Init(action, bindingIndex);
            else
                Debug.Log("script Rebind not found");

            bindingIndex++;
        }
    }
}
