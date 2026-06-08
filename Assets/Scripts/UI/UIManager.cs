using System;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject GO_controls;
    [SerializeField] GameObject GO_Prefab_Keybind;

    PlayerAction controls;

    enum actionType
    {
        None,
        Bool,
        Axis1,
        Vector2
    }
    private void Awake()
    {
        controls = InputManager.controls;
        Init();
    }
    void Update()
    {

    }
    private actionType GetActionType(InputAction action)
    {
        actionType type = actionType.None;
        switch (action.expectedControlType)
        {
            case "Button":
                type = actionType.Bool;
                break;
            case "Axis":
                type = actionType.Axis1;
                break;
            case "Vector2":
                type = actionType.Vector2;
                break;
        }
        return type;
    }
    private void Init()
    {
        GO_controls.SetActive(true);
        Transform t = getContent(GO_controls.transform);
        foreach (InputAction action in controls.GamePlay.Get())
        {
            if (action.name == "Look")
                continue;


            int boucle = 0;
            switch (action.type)
            {
                case InputActionType.Value:
                    GetActionType(action);
                    break;
                case InputActionType.Button:
                    buffer.GetComponent<TextMeshPro>().text = action.name;
                    if (action.bindings.Any(b => b.isComposite))
                    {

                    }
                        break;
                case InputActionType.PassThrough:
                    //  nothing ...
                    break;
                default:
                    break;
            }
            GameObject buffer = Instantiate(GO_Prefab_Keybind, t, false);
            Debug.Log(action.name);
        }
    }
    private Transform getContent(Transform of)
    {
        Transform result = of.Find("ViewPort");
        result = result.Find("Content");
        return result;
    }
}
