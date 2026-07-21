using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [SerializeField] public GameObject PageLayout;
    [SerializeField] public GameObject SchemeBouton_Prefab;

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

        //  lambdas
        Action<InputControlScheme, GameObject> InitBouton = (scheme, content) =>
        {
            foreach (InputActionMap map in controls.asset.actionMaps)
            {
                foreach (InputAction action in map)
                {
                    if (map.name == controls.GeneriqueMove.Get().name && action.name == "Look")
                        continue;

                    CreateBoutonFromAction(action, content.transform, scheme);
                }
            }
        };
        Action<GameObject> nullChild = (parent) =>
        {
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }
        };
        Action delegatedAction = () =>
        {
            foreach (Transform child in Controls_ViewPort.transform)
            {
                child.gameObject.SetActive(false);
            }
        };

        nullChild(Controls_ViewPort);
        nullChild(PageLayout);

        for (int i = 0; i < controls.controlSchemes.Count; i++) //  pour chaque scheme (lors de la creation il y a : 'Keyboard&Mouse' et 'Gamepad')
        {
            InputControlScheme scheme = controls.controlSchemes[i];


            //  les boutons ainsi que le "content" qui tiens les bouton
            GameObject instance = Instantiate(Content_Prefab);
            instance.transform.SetParent(Controls_ViewPort.transform, false);
            instance.name = scheme.name;
            instance.SetActive(false);
            gameObjects_Contents.Add(instance);

            InitBouton(scheme, gameObjects_Contents[i]);

            //  creer le bouton lié au "content" pour l'afficher
            GameObject instance2 = Instantiate(SchemeBouton_Prefab);
            instance2.transform.SetParent(PageLayout.transform, false);
            instance2.GetComponentInChildren<TextMeshProUGUI>().text = scheme.name;
            instance2.GetComponent<Bouton>().show_target.Add(instance.transform);
            instance2.GetComponent<Bouton>().@delegate = delegatedAction;

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

    private void CreateBoutonFromAction(InputAction action, Transform parent, InputControlScheme scheme)
    {
        int bindingIndex = 0;
        foreach (InputBinding binding in action.bindings)
        {
            //   regarde si le chemain prie n'est pas l'un définit dedans deviceUsed (KeyBoard,Mouse,GamePad,etc..)
            if (binding.isComposite || !binding.groups.Contains(scheme.bindingGroup))
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
