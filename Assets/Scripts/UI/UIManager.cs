using System;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform GO_Controls_Content;
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
    public void Init()
    {
        foreach (InputAction action in controls.GamePlay.Get())
        {
            if (action.name == "Look")
                continue;
            CreateBoutonFromAction(action, GO_Controls_Content);
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
            Debug.Log($"name={ action.name+" "+binding.name} | path={binding.path} | composite={binding.isComposite} | part={binding.isPartOfComposite}");

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
